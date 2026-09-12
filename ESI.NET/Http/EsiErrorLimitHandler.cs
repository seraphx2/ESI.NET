using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ESI.NET.Http
{
    /// <summary>
    /// Client-side guard for ESI's rolling error budget. Reads <c>X-Esi-Error-Limit-Remain</c> /
    /// <c>X-Esi-Error-Limit-Reset</c> from every response; once the budget is spent (or a
    /// <c>420</c> comes back) it blocks further sends until the window resets, so a burst of
    /// failures can't get the caller's IP fully cut off. On a <c>420</c> it throws
    /// <see cref="EsiErrorLimitException"/>.
    /// </summary>
    public sealed class EsiErrorLimitHandler : DelegatingHandler
    {
        private const string RemainHeader = "X-Esi-Error-Limit-Remain";
        private const string ResetHeader = "X-Esi-Error-Limit-Reset";
        private const int Status420 = 420;

        private readonly EsiErrorLimitState _state;

        public EsiErrorLimitHandler(EsiErrorLimitState state)
        {
            _state = state;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var wait = _state.TimeUntilReset(DateTimeOffset.UtcNow);
            if (wait > TimeSpan.Zero)
                await Task.Delay(wait, cancellationToken).ConfigureAwait(false);

            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            var resetSeconds = ReadInt(response, ResetHeader);
            var remain = ReadInt(response, RemainHeader);

            if (((int)response.StatusCode == Status420 || remain <= 0) && resetSeconds > 0)
                _state.BlockUntil(DateTimeOffset.UtcNow.AddSeconds(resetSeconds));

            if ((int)response.StatusCode == Status420)
            {
                response.Dispose();
                throw new EsiErrorLimitException(TimeSpan.FromSeconds(resetSeconds > 0 ? resetSeconds : 60));
            }

            return response;
        }

        private static int ReadInt(HttpResponseMessage response, string header)
        {
            if (response.Headers.TryGetValues(header, out var values))
                foreach (var v in values)
                    if (int.TryParse(v, out var n))
                        return n;
            return int.MaxValue;
        }
    }

    /// <summary>
    /// Shared error-limit window, tracked across every request on a client. Registered as a
    /// singleton by <c>AddEsi</c>.
    /// </summary>
    public sealed class EsiErrorLimitState
    {
        private long _blockedUntilTicks; // DateTimeOffset.UtcTicks, 0 = not blocked

        internal void BlockUntil(DateTimeOffset until)
        {
            var ticks = until.UtcTicks;
            long current;
            do
            {
                current = Interlocked.Read(ref _blockedUntilTicks);
                if (ticks <= current) return;
            }
            while (Interlocked.CompareExchange(ref _blockedUntilTicks, ticks, current) != current);
        }

        internal TimeSpan TimeUntilReset(DateTimeOffset now)
        {
            var ticks = Interlocked.Read(ref _blockedUntilTicks);
            if (ticks == 0) return TimeSpan.Zero;
            var until = new DateTimeOffset(ticks, TimeSpan.Zero);
            var delta = until - now;
            return delta > TimeSpan.Zero ? delta : TimeSpan.Zero;
        }
    }
}
