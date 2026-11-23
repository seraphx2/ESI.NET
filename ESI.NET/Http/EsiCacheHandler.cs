using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ESI.NET.Http
{
    /// <summary>
    /// Caches ESI <c>GET</c> responses for as long as ESI says they are good (<c>Expires</c>),
    /// then revalidates them with their <c>ETag</c> so an unchanged resource costs a <c>304</c>
    /// instead of a full download. Entries live in the app's <see cref="IDistributedCache"/>
    /// (in-memory unless the app registers another, e.g. Redis), so several app instances can
    /// share one cache. Added by <c>AddEsiCaching()</c>, as the outermost handler: a hit never
    /// triggers a token refresh or waits on the error-limit guard.
    /// <para>
    /// Rules: only <c>GET</c>s; only <c>200</c>s are stored; an authenticated response is stored
    /// per character, so one character never sees another's data (even for shared resources
    /// like a corporation's wallets, where ESI checks roles per character). A call that sets
    /// its own <see cref="EsiCallOptions.IfNoneMatch"/> bypasses the cache entirely and gets
    /// ESI's raw answer, including a <c>304</c>. Concurrent misses for the same key in one
    /// process share a single ESI request. A failing cache store never fails the call; the
    /// request goes to ESI and the failure is logged.
    /// </para>
    /// </summary>
    public sealed class EsiCacheHandler : DelegatingHandler
    {
        /// <summary>Response header carrying the <see cref="EsiCacheStatus"/> to <see cref="EsiResponse{T}"/>.</summary>
        internal const string StatusHeader = "X-ESI-NET-Cache";

        // What a stored entry keeps. Per-request headers (error-limit budget, request id, Date)
        // are deliberately not stored: on a hit they would be stale and misleading.
        //
        // Time: freshness is judged on ESI's clock, never the local one. Each process learns its
        // offset from ESI's Date header (EsiCacheState), stored expiries are ESI times, and store
        // TTLs are relative - so a skewed local clock, or app instances with different clocks
        // sharing one Redis, neither serve stale data nor discard fresh data.
        private static readonly string[] StoredHeaders =
            { "Cache-Control", "Content-Type", "ETag", "Expires", "Last-Modified", "X-Pages" };

        // What a 304's expiry is worked out from: the stored headers it refreshes, plus when ESI
        // generated it (Date) and how long it already spent in a cache on the way (Age).
        private static readonly string[] FreshnessHeaders = StoredHeaders.Concat(new[] { "Date", "Age" }).ToArray();

        // Describe a body that is no longer the one being sent; HttpContent sets its own.
        private static readonly string[] BodyFramingHeaders = { "Content-Length", "Transfer-Encoding", "Content-Encoding" };

        private readonly IDistributedCache _cache;
        private readonly EsiCacheState _state;
        private readonly EsiCacheOptions _options;
        private readonly ILogger<EsiCacheHandler> _logger;

        /// <summary>Created by <c>AddEsiCaching()</c>; there is no need to construct it.</summary>
        /// <param name="cache">Where responses are stored.</param>
        /// <param name="state">State shared by every handler instance: ESI's clock and the in-flight requests.</param>
        /// <param name="options">The cache options.</param>
        /// <param name="logger">Logs store failures; optional.</param>
        public EsiCacheHandler(IDistributedCache cache, EsiCacheState state, IOptions<EsiCacheOptions> options, ILogger<EsiCacheHandler> logger = null)
        {
            Guard.NotNull(cache, nameof(cache));
            Guard.NotNull(state, nameof(state));
            Guard.NotNull(options, nameof(options));

            _cache = cache;
            _state = state;
            _options = options.Value ?? new EsiCacheOptions();
            _logger = logger;
        }

        /// <inheritdoc/>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Guard.NotNull(request, nameof(request));

            // Only ESI API calls built by EsiRequest - never SSO (token, JWKS) or anything else
            // sent through this client.
            var key = EsiRequestState.IsEsiRequest(request)
                      && request.Method == HttpMethod.Get
                      && !request.Headers.Contains("If-None-Match")
                ? KeyFor(request)
                : null;
            if (key == null)
                return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            var bypass = EsiRequestState.GetBypassCache(request);
            var cached = await ReadAsync(key, cancellationToken).ConfigureAwait(false);

            // A hit needs ESI's clock: until this process has seen an ESI Date, an entry another
            // instance wrote can't be judged, so it is revalidated (a cheap 304) instead.
            if (!bypass && _state.ClockKnown && cached != null && !cached.MustRevalidate
                && cached.Expires > _state.ServerNow())
                return Materialize(cached, EsiCacheStatus.Hit, request);

            var flightKey = _state.FlightKey(key, EsiRequestState.GetCharacter(request));
            var result = await _state.RunOnceAsync(
                flightKey, () => FetchAsync(request, key, cached, cancellationToken), cancellationToken).ConfigureAwait(false);

            return Materialize(result.Response, result.Status, request);
        }

        private async Task<EsiCacheResult> FetchAsync(HttpRequestMessage request, string key, EsiCachedResponse cached, CancellationToken cancellationToken)
        {
            // Sent back exactly as ESI gave it: ESI uses weak validators (W/"...") on some routes.
            if (cached?.ETag != null)
                request.Headers.TryAddWithoutValidation("If-None-Match", cached.ETag);

            var started = Stopwatch.GetTimestamp();
            using (var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false))
            {
                // The round trip starts when the request left for ESI (EsiSendClockHandler, innermost),
                // not here: the handlers in between can wait out the error budget or refresh a token,
                // and that wait would push ESI's clock ahead until the next response.
                var sentAt = EsiRequestState.GetSentAt(request) ?? started;
                var roundTrip = TimeSpan.FromTicks((long)((Stopwatch.GetTimestamp() - sentAt) * ((double)TimeSpan.TicksPerSecond / Stopwatch.Frequency)));
                _state.ObserveServerDate(response.Headers.Date, response.Headers.Age, roundTrip);
                var now = _state.ServerNow();

                if (response.StatusCode == HttpStatusCode.NotModified && cached != null)
                {
                    // Unchanged: keep the stored body, take ESI's fresh cache headers. The expiry is
                    // worked out from the whole 304 (its Date and Age too), then only the stored
                    // headers are kept.
                    var live = Capture(response, null, now);
                    var refreshed = cached.With(Only(live.Headers, FreshnessHeaders), now).WithOnly(StoredHeaders);

                    if (IsStorable(refreshed, now))
                        await WriteAsync(key, refreshed, now, cancellationToken).ConfigureAwait(false);
                    else
                        await RemoveAsync(key, cancellationToken).ConfigureAwait(false);

                    // The caller also gets this request's live per-request headers (error budget, ...).
                    return new EsiCacheResult(refreshed.With(live.Headers.Where(h => !refreshed.Headers.ContainsKey(h.Key)), now, keepExpires: true), EsiCacheStatus.Revalidated);
                }

                // Content can be null on .NET Framework (netstandard2.0).
                var body = response.Content == null
                    ? Array.Empty<byte>()
#if NET
                    : await response.Content.ReadAsByteArrayAsync(cancellationToken).ConfigureAwait(false);
#else
                    : await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
#endif
                var captured = Capture(response, body, now);

                if (response.StatusCode == HttpStatusCode.OK && IsJson(captured) && IsStorable(captured, now))
                    await WriteAsync(key, captured.WithOnly(StoredHeaders), now, cancellationToken).ConfigureAwait(false);
                else if (cached != null && (int)response.StatusCode < 500)
                {
                    // ESI's newer answer replaces what was cached: a 403 after a lost role, a 404, a
                    // no-store 200, a non-JSON 200 - a later call must not get the old 200 back.
                    // A 5xx is ESI failing, not an answer about the resource, so the entry stays.
                    await RemoveAsync(key, cancellationToken).ConfigureAwait(false);
                }

                return new EsiCacheResult(captured, EsiCacheStatus.Miss);
            }
        }

        /// <summary>
        /// The cache key, or <c>null</c> when the request must not be cached. It holds everything
        /// that can change the answer: compatibility date (a package upgrade must not read entries
        /// shaped for another contract out of a shared store), tenant, the character, language and
        /// the full URL (path, query, page).
        /// </summary>
        private string KeyFor(HttpRequestMessage request)
        {
            string principal;
            if (request.Headers.Authorization != null)
            {
                // Authenticated but not via EsiCallOptions.Character (a hand-built request):
                // there is no safe identity to key on, so don't cache it.
                var character = EsiRequestState.GetCharacter(request);
                if (character == null || character.CharacterID <= 0)
                    return null;

                // The owner hash changes when a character is transferred to another account, so the
                // new owner never sees what was cached for the old one. (A lost role or scope still
                // shows only once ESI's Expires passes - ESI itself caches for that long.)
                principal = "c" + character.CharacterID.ToString(CultureInfo.InvariantCulture)
                            + ":" + (character.CharacterOwnerHash ?? string.Empty);
            }
            else
                principal = "public";

            return string.Join("|",
                _options.KeyPrefix + "v1",
                Header(request, "X-Compatibility-Date"),
                Header(request, "X-Tenant"),
                principal,
                Header(request, "Accept-Language"),
                request.RequestUri?.AbsoluteUri);
        }

        private static string Header(HttpRequestMessage request, string name)
            => request.Headers.TryGetValues(name, out var values) ? string.Join(",", values) : string.Empty;

        private static bool IsStorable(EsiCachedResponse response, DateTimeOffset now)
        {
            if (response.Headers.TryGetValue("Cache-Control", out var cc)
                && Directives(cc).Any(d => d.Name.Equals("no-store", StringComparison.OrdinalIgnoreCase)))
                return false;

            // Worth keeping if it is still fresh, or if it can be revalidated cheaply later.
            return response.Expires > now || response.ETag != null;
        }

        /// <summary>
        /// ESI answers in JSON. Anything else under a 200 (a gateway's HTML page, ...) is a fault
        /// that EsiResponse rejects; storing it would make every later hit fail the same way.
        /// </summary>
        private static bool IsJson(EsiCachedResponse response)
        {
            if (response.Headers.TryGetValue("Content-Type", out var type))
                return type.Any(t => CultureInfo.InvariantCulture.CompareInfo.IndexOf(t, "json", CompareOptions.OrdinalIgnoreCase) >= 0);

            // No Content-Type: judge by the first non-blank byte, as EsiResponse would.
            var body = response.Body ?? Array.Empty<byte>();
            var first = body.SkipWhile(b => b == ' ' || b == '\t' || b == '\r' || b == '\n').Cast<byte?>().FirstOrDefault();
            return first == null || JsonStarts.Contains((char)first.Value);
        }

        private static readonly char[] EqualsSign = { '=' };

        // What a JSON text can start with: object, array, string, number, true/false/null.
        private static readonly HashSet<char> JsonStarts = new HashSet<char>("{[\"-0123456789tfn");

        /// <summary>Splits <c>Cache-Control</c> values into <c>name[=value]</c> directives.</summary>
        internal static IEnumerable<(string Name, string Value)> Directives(IEnumerable<string> values)
            => values
                .SelectMany(v => v.Split(','))
                .Select(p => p.Split(EqualsSign, 2))
                .Select(kv => (kv[0].Trim(), kv.Length > 1 ? kv[1].Trim().Trim('"') : null))
                .Where(d => d.Item1.Length > 0);

        private static readonly Action<ILogger, string, Exception> LogReadFailed = LoggerMessage.Define<string>(
            LogLevel.Warning, new EventId(1, "EsiCacheReadFailed"), "ESI cache read failed for {Key}; asking ESI instead.");

        private static readonly Action<ILogger, string, Exception> LogWriteFailed = LoggerMessage.Define<string>(
            LogLevel.Warning, new EventId(2, "EsiCacheWriteFailed"), "ESI cache write failed for {Key}; the response is returned uncached.");

        // A cache store is the app's infrastructure (Redis, SQL, ...) and can fail in any way its
        // client library likes. None of that may fail an ESI call, so both catch everything except
        // the caller's own cancellation, log it, and fall back to ESI / an uncached response.
#pragma warning disable CA1031
        private async Task<EsiCachedResponse> ReadAsync(string key, CancellationToken cancellationToken)
        {
            try
            {
                var bytes = await _cache.GetAsync(key, cancellationToken).ConfigureAwait(false);
                if (bytes == null)
                    return null;

                return EsiCachedResponse.FromBytes(bytes);   // null for another format version
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception ex)
            {
                if (_logger != null)
                    LogReadFailed(_logger, key, ex);
                return null;
            }
        }

        private async Task WriteAsync(string key, EsiCachedResponse entry, DateTimeOffset now, CancellationToken cancellationToken)
        {
            try
            {
                // Relative TTL: `now` is ESI time, and an absolute deadline would be read against
                // the store client's own clock.
                var freshFor = entry.Expires > now ? entry.Expires.Value - now : TimeSpan.Zero;
                var keepFor = freshFor + _options.RetainAfterExpiry;
                if (keepFor <= TimeSpan.Zero)
                {
                    // Stale already and nothing kept for revalidation: nothing to store, and an older
                    // entry (with older headers) must not outlive the response that replaced it.
                    await RemoveAsync(key, cancellationToken).ConfigureAwait(false);
                    return;
                }

                await _cache.SetAsync(key, entry.ToBytes(), new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = keepFor,
                }, cancellationToken).ConfigureAwait(false);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception ex)
            {
                if (_logger != null)
                    LogWriteFailed(_logger, key, ex);
            }
        }

        private async Task RemoveAsync(string key, CancellationToken cancellationToken)
        {
            try
            {
                await _cache.RemoveAsync(key, cancellationToken).ConfigureAwait(false);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception ex)
            {
                if (_logger != null)
                    LogWriteFailed(_logger, key, ex);
            }
        }
#pragma warning restore CA1031

        private static EsiCachedResponse Capture(HttpResponseMessage response, byte[] body, DateTimeOffset now)
        {
            var headers = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);
            foreach (var h in response.Headers)
                headers[h.Key] = h.Value.ToArray();
            if (response.Content != null)
                foreach (var h in response.Content.Headers)
                    headers[h.Key] = h.Value.ToArray();

            return new EsiCachedResponse { Status = (int)response.StatusCode, Body = body }.With(headers, now);
        }

        private static IEnumerable<KeyValuePair<string, string[]>> Only(Dictionary<string, string[]> headers, string[] names)
            => headers.Where(h => names.Contains(h.Key, StringComparer.OrdinalIgnoreCase));

        private static HttpResponseMessage Materialize(EsiCachedResponse entry, EsiCacheStatus status, HttpRequestMessage request)
        {
            var response = new HttpResponseMessage((HttpStatusCode)entry.Status)
            {
                RequestMessage = request,
                Content = new ByteArrayContent(entry.Body ?? Array.Empty<byte>()),
            };

            foreach (var h in entry.Headers)
            {
                if (BodyFramingHeaders.Contains(h.Key, StringComparer.OrdinalIgnoreCase))
                    continue;
                if (!response.Headers.TryAddWithoutValidation(h.Key, h.Value))
                    response.Content.Headers.TryAddWithoutValidation(h.Key, h.Value);
            }

            response.Headers.TryAddWithoutValidation(StatusHeader, status.ToString());
            return response;
        }
    }

    /// <summary>
    /// Process-wide state for <see cref="EsiCacheHandler"/> (whose instances are recycled by
    /// <see cref="IHttpClientFactory"/>): the in-flight requests that concurrent misses share.
    /// Registered as a singleton by <c>AddEsiCaching()</c>.
    /// </summary>
    public sealed class EsiCacheState
    {
        private readonly ConcurrentDictionary<string, Task<EsiCacheResult>> _inflight =
            new ConcurrentDictionary<string, Task<EsiCacheResult>>(StringComparer.Ordinal);

        // ESI's clock minus ours, from the latest ESI response. Only ESI API responses feed it
        // (the handler only sees those), never SSO or other hosts.
        private long _serverOffsetTicks;
        private int _clockKnown;

        /// <summary>
        /// Whether an ESI <c>Date</c> has been seen yet. Until then <see cref="ServerNow"/> is just
        /// the local clock, so the handler serves no hits.
        /// </summary>
        internal bool ClockKnown => Volatile.Read(ref _clockKnown) == 1;

        /// <summary>
        /// Records ESI's clock from a response. <c>Date</c> is when ESI generated it, plus <c>Age</c>
        /// if it then sat in a cache on the way. ESI's clock on arrival is somewhere between that and
        /// that plus <c>Date</c>'s rounding (it has whole seconds) and the time the response took to
        /// come back (at most <paramref name="roundTrip"/>). The upper end is used: running a little
        /// ahead of ESI only expires entries a moment early, running behind would serve them late.
        /// </summary>
        internal void ObserveServerDate(DateTimeOffset? serverDate, TimeSpan? age, TimeSpan roundTrip)
        {
            if (!serverDate.HasValue)
                return;

            var serverNow = serverDate.Value + (age ?? TimeSpan.Zero) + DateResolution + roundTrip;
            Interlocked.Exchange(ref _serverOffsetTicks, (serverNow - DateTimeOffset.UtcNow).Ticks);
            Volatile.Write(ref _clockKnown, 1);
        }

        private static readonly TimeSpan DateResolution = TimeSpan.FromSeconds(1);

        // Distinct per AuthorizedCharacterData instance (weakly held), for FlightKey.
        private readonly System.Runtime.CompilerServices.ConditionalWeakTable<object, string> _instanceIds =
            new System.Runtime.CompilerServices.ConditionalWeakTable<object, string>();

        /// <summary>
        /// The key concurrent misses are merged on: the cache key, plus - for an authenticated call -
        /// the identity of the caller's <c>AuthorizedCharacterData</c> object. Only the merged
        /// request goes through the token refresh, which refreshes and reports (OnTokenRefreshed,
        /// the sink) the token of that one object; merging calls holding different objects for the
        /// same character would leave the others' tokens stale and their callbacks unrun.
        /// </summary>
        internal string FlightKey(string cacheKey, object character)
            => character == null
                ? cacheKey
                : cacheKey + "#" + _instanceIds.GetValue(character, _ => Guid.NewGuid().ToString("N"));

        /// <summary>The current time on ESI's clock, as best this process knows it.</summary>
        internal DateTimeOffset ServerNow()
            => DateTimeOffset.UtcNow + TimeSpan.FromTicks(Interlocked.Read(ref _serverOffsetTicks));

        /// <summary>
        /// Runs <paramref name="fetch"/> once per key at a time; concurrent callers for the same key
        /// await the first caller's result. If the first caller is cancelled, the others retry
        /// rather than inherit its cancellation.
        /// </summary>
        internal async Task<EsiCacheResult> RunOnceAsync(string key, Func<Task<EsiCacheResult>> fetch, CancellationToken cancellationToken)
        {
            while (true)
            {
                var mine = new TaskCompletionSource<EsiCacheResult>(TaskCreationOptions.RunContinuationsAsynchronously);
                var running = _inflight.GetOrAdd(key, mine.Task);

                if (running == mine.Task)
                {
                    // The entry is removed *before* the task completes, so a waiter woken by the
                    // completion that goes round again never finds this finished task still there.
                    EsiCacheResult result;
                    try
                    {
                        result = await fetch().ConfigureAwait(false);
                    }
                    catch (OperationCanceledException)
                    {
                        Leave(key, mine.Task);
                        mine.TrySetCanceled(cancellationToken);
                        throw;
                    }
                    catch (Exception ex)
                    {
                        // Waiters get the same exception - by design, not retried per waiter: when
                        // ESI is failing, N retries would spend N errors of ESI's error budget.
                        Leave(key, mine.Task);
                        mine.SetException(ex);
                        _ = mine.Task.Exception; // observed here; waiters rethrow it themselves
                        throw;
                    }

                    Leave(key, mine.Task);
                    mine.SetResult(result);
                    return result;
                }

                try
                {
                    return await WaitAsync(running, cancellationToken).ConfigureAwait(false);
                }
                catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
                {
                    // The first caller was cancelled, not us: go again.
                }
            }
        }

        private void Leave(string key, Task<EsiCacheResult> task)
            => ((ICollection<KeyValuePair<string, Task<EsiCacheResult>>>)_inflight)
                .Remove(new KeyValuePair<string, Task<EsiCacheResult>>(key, task));

        private static async Task<T> WaitAsync<T>(Task<T> task, CancellationToken cancellationToken)
        {
            if (!cancellationToken.CanBeCanceled)
                return await task.ConfigureAwait(false);

            var cancelled = new TaskCompletionSource<T>(TaskCreationOptions.RunContinuationsAsynchronously);
            using (cancellationToken.Register(() => cancelled.TrySetCanceled(cancellationToken)))
            {
                var done = await Task.WhenAny(task, cancelled.Task).ConfigureAwait(false);
                return await done.ConfigureAwait(false);
            }
        }
    }

    /// <summary>
    /// The store the cache uses: the app's <see cref="IDistributedCache"/>, or ESI.NET's private
    /// in-memory one. A holder rather than an <see cref="IDistributedCache"/> registration, so the
    /// private store is never visible to (or configured by) the rest of the app.
    /// </summary>
    internal sealed class EsiCacheStore
    {
        public EsiCacheStore(IDistributedCache cache) => Cache = cache;

        public IDistributedCache Cache { get; }
    }

    /// <summary>An ESI response as stored in, or served from, the cache.</summary>
    internal sealed class EsiCachedResponse
    {
        /// <summary>
        /// The stored layout's version, its first byte. Anything else reads as a miss, so a
        /// future layout never misreads entries an older build left in a shared store.
        /// </summary>
        public const byte FormatVersion = 2;

        private const int PreambleLength = 1 + 4; // version byte + metadata length (int32, little-endian)

        // Fixed, never JsonConvert.DefaultSettings: an app's global JSON settings must not change
        // the stored format, and bytes read back from a shared store must never resolve types.
        // JsonSerializer.Create, not JsonConvert: JsonConvert(..., settings) still applies
        // DefaultSettings first and only overlays these on top.
        private static readonly JsonSerializer Json = JsonSerializer.Create(new JsonSerializerSettings
        {
            DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK",
            TypeNameHandling = TypeNameHandling.None,
            DateParseHandling = DateParseHandling.DateTimeOffset,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver(),
            NullValueHandling = NullValueHandling.Include,
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
        });

        private static byte[] SerializeMetadata(EsiCachedResponse entry)
        {
            using (var writer = new System.IO.StringWriter(CultureInfo.InvariantCulture))
            {
                Json.Serialize(writer, entry);
                return Encoding.UTF8.GetBytes(writer.ToString());
            }
        }

        private static EsiCachedResponse DeserializeMetadata(byte[] bytes, int offset, int count)
        {
            using (var reader = new JsonTextReader(new System.IO.StringReader(Encoding.UTF8.GetString(bytes, offset, count))))
                return Json.Deserialize<EsiCachedResponse>(reader);
        }

        /// <summary>
        /// ESI said <c>no-cache</c>: the entry may be kept, but must be revalidated before every
        /// use - never served as a plain hit.
        /// </summary>
        [JsonIgnore]
        public bool MustRevalidate
            => Headers != null
               && Headers.TryGetValue("Cache-Control", out var cc)
               && EsiCacheHandler.Directives(cc).Any(d => d.Name.Equals("no-cache", StringComparison.OrdinalIgnoreCase));

        public int Status { get; set; }
        public Dictionary<string, string[]> Headers { get; set; } = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);

        /// <summary>ESI's body, stored byte-for-byte after the metadata - never inside the JSON.</summary>
        [JsonIgnore]
        public byte[] Body { get; set; }
        public DateTimeOffset? Expires { get; set; }
        public string ETag { get; set; }

        /// <summary>
        /// <c>[version][metadata length][metadata JSON][body]</c>. Only the few hundred bytes of
        /// metadata are serialized; the body is copied as-is - no base64, no JSON escaping - so a
        /// stored entry is about the size of ESI's response and a hit never re-encodes it.
        /// </summary>
        public byte[] ToBytes()
        {
            var metadata = SerializeMetadata(this);
            var body = Body ?? Array.Empty<byte>();

            var bytes = new byte[PreambleLength + metadata.Length + body.Length];
            bytes[0] = FormatVersion;
            WriteInt32(bytes, 1, metadata.Length);
            Buffer.BlockCopy(metadata, 0, bytes, PreambleLength, metadata.Length);
            Buffer.BlockCopy(body, 0, bytes, PreambleLength + metadata.Length, body.Length);
            return bytes;
        }

        /// <summary>The entry in <paramref name="bytes"/>, or <c>null</c> if it isn't one this build wrote.</summary>
        public static EsiCachedResponse FromBytes(byte[] bytes)
        {
            if (bytes == null || bytes.Length < PreambleLength || bytes[0] != FormatVersion)
                return null;

            var metadataLength = ReadInt32(bytes, 1);
            if (metadataLength <= 0 || metadataLength > bytes.Length - PreambleLength)
                return null;

            var entry = DeserializeMetadata(bytes, PreambleLength, metadataLength);
            if (entry == null)
                return null;

            var bodyStart = PreambleLength + metadataLength;
            entry.Body = new byte[bytes.Length - bodyStart];
            Buffer.BlockCopy(bytes, bodyStart, entry.Body, 0, entry.Body.Length);
            return entry;
        }

        // Fixed little-endian, so a store shared across machines reads the same on all of them.
        private static void WriteInt32(byte[] buffer, int offset, int value)
        {
            buffer[offset] = (byte)value;
            buffer[offset + 1] = (byte)(value >> 8);
            buffer[offset + 2] = (byte)(value >> 16);
            buffer[offset + 3] = (byte)(value >> 24);
        }

        private static int ReadInt32(byte[] buffer, int offset)
            => buffer[offset] | (buffer[offset + 1] << 8) | (buffer[offset + 2] << 16) | (buffer[offset + 3] << 24);

        /// <summary>
        /// A copy with <paramref name="overrides"/> applied and <c>Expires</c>/<c>ETag</c> re-derived
        /// - unless <paramref name="keepExpires"/>, for adding headers that say nothing about
        /// freshness to an entry whose expiry is already settled.
        /// </summary>
        public EsiCachedResponse With(IEnumerable<KeyValuePair<string, string[]>> overrides, DateTimeOffset now, bool keepExpires = false)
        {
            var headers = new Dictionary<string, string[]>(Headers ?? new Dictionary<string, string[]>(), StringComparer.OrdinalIgnoreCase);
            foreach (var h in overrides)
                headers[h.Key] = h.Value;

            return new EsiCachedResponse
            {
                Status = Status,
                Body = Body,
                Headers = headers,
                Expires = keepExpires ? Expires : ExpiresFrom(headers, now),
                ETag = headers.TryGetValue("ETag", out var etag) ? etag.FirstOrDefault() : null,
            };
        }

        /// <summary>A copy keeping only the headers in <paramref name="names"/>.</summary>
        public EsiCachedResponse WithOnly(string[] names)
            => new EsiCachedResponse
            {
                Status = Status,
                Body = Body,
                Expires = Expires,
                ETag = ETag,
                Headers = Headers
                    .Where(h => names.Contains(h.Key, StringComparer.OrdinalIgnoreCase))
                    .ToDictionary(h => h.Key, h => h.Value, StringComparer.OrdinalIgnoreCase),
            };

        /// <summary>
        /// When the response goes stale, in ESI's clock. <paramref name="now"/> is ESI time too
        /// (<see cref="EsiCacheState.ServerNow"/>).
        /// </summary>
        private static DateTimeOffset? ExpiresFrom(Dictionary<string, string[]> headers, DateTimeOffset now)
        {
            // max-age wins over Expires (RFC 9111 §5.3) and counts from when ESI generated the
            // response (Date). Caches along the way don't reset Date, so the time spent in them
            // (Age) is already inside "now - Date" - it is only needed when there is no Date.
            if (headers.TryGetValue("Cache-Control", out var cc))
                foreach (var (name, value) in EsiCacheHandler.Directives(cc))
                    if (name.Equals("max-age", StringComparison.OrdinalIgnoreCase)
                        && int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var seconds))
                    {
                        var generated = DateHeader(headers, "Date");
                        if (generated == null)
                        {
                            var age = headers.TryGetValue("Age", out var a)
                                && int.TryParse(a.FirstOrDefault(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var ageSeconds)
                                ? ageSeconds : 0;
                            generated = now.AddSeconds(-age);
                        }
                        return generated.Value.AddSeconds(seconds);
                    }

            // Expires is already an ESI time.
            return DateHeader(headers, "Expires");
        }

        private static DateTimeOffset? DateHeader(Dictionary<string, string[]> headers, string name)
            => headers.TryGetValue(name, out var values)
               && DateTimeOffset.TryParse(values.FirstOrDefault(), CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var at)
                ? at : (DateTimeOffset?)null;
    }

    /// <summary>What a fetch produced, shared between concurrent callers.</summary>
    internal sealed class EsiCacheResult
    {
        public EsiCacheResult(EsiCachedResponse response, EsiCacheStatus status)
        {
            Response = response;
            Status = status;
        }

        public EsiCachedResponse Response { get; }
        public EsiCacheStatus Status { get; }
    }
}
