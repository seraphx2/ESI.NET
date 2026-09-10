using System.Net;
using Newtonsoft.Json;

namespace ESI.NET.Tools.SpecCheck;

/// <summary>
/// Live check: for every public GET whose path parameters can be filled from a
/// small fixture set, fetch the real response and deserialize it into the
/// wrapper's model. Catches the case SchemaCheck cannot - a field the spec marks
/// non-null that ESI actually returns as <c>null</c>, which throws for every
/// caller of that endpoint.
/// </summary>
public static class Probe
{
    // Well-known ids that exist on Tranquility.
    private static readonly Dictionary<string, string> Fixtures = new(StringComparer.Ordinal)
    {
        ["type_id"] = "34",                 // Tritanium
        ["region_id"] = "10000002",         // The Forge
        ["constellation_id"] = "20000020",  // Kimotoro
        ["system_id"] = "30000142",         // Jita
        ["star_id"] = "40009076",
        ["planet_id"] = "40009077",
        ["asteroid_belt_id"] = "40000005",
        ["moon_id"] = "40009081",
        ["stargate_id"] = "50001248",
        ["station_id"] = "60003760",        // Jita 4-4 CNAP
        ["graphic_id"] = "10",
        ["category_id"] = "6",              // Ship
        ["group_id"] = "25",                // Frigate
        ["market_group_id"] = "2",
        ["attribute_id"] = "4",             // Mass
        ["effect_id"] = "10",
        ["schematic_id"] = "65",
        ["alliance_id"] = "99005338",
        ["corporation_id"] = "98356193",
        ["character_id"] = "2112625428",
        ["faction_id"] = "500001",
        ["war_id"] = "1",
        ["campaign_id"] = "0",
    };

    public static async Task<int> RunAsync(Spec spec, Wrapper wrapper, string baseUrl, string compatDate)
    {
        using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };
        http.DefaultRequestHeaders.UserAgent.ParseAdd("ESI.NET-spec-check-probe/1.0");
        http.DefaultRequestHeaders.Add("X-Compatibility-Date", compatDate);
        http.DefaultRequestHeaders.Add("X-Tenant", "tranquility");

        var specKeys = spec.Operations.Select(o => o.Key).ToHashSet();

        var targets = wrapper.Endpoints
            .Where(e => e.HttpMethod == "GET" && !e.Authenticated && e.ModelType is not null)
            .Where(e => specKeys.Contains(e.Key))
            .Where(e => Fillable(e.Path))
            .GroupBy(e => e.Key).Select(g => g.First())
            .OrderBy(e => e.Path)
            .ToList();

        Console.WriteLine();
        Console.WriteLine($"PROBE - {targets.Count} public GET endpoint(s), live against {baseUrl}");

        var failures = new List<string>();
        var okCount = 0;

        // A few endpoints 400 without a query parameter.
        var queryFor = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["GET /markets/{region_id}/history"] = "?type_id=34",
        };

        foreach (var endpoint in targets)
        {
            var path = Fill(endpoint.Path);
            var query = queryFor.TryGetValue(endpoint.Key, out var q) ? q : "";
            var url = $"{baseUrl.TrimEnd('/')}{path}{query}";
            try
            {
                using var response = await http.GetAsync(url);
                var body = await response.Content.ReadAsStringAsync();
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    Console.WriteLine($"  skip  {endpoint.Key}  ({(int)response.StatusCode})");
                    continue;
                }

                try
                {
                    JsonConvert.DeserializeObject(body.Trim(), endpoint.ModelType!);
                    okCount++;
                }
                catch (JsonException ex)
                {
                    var msg = ex.Message.Split('\n')[0].Trim();
                    failures.Add($"{endpoint.Key}  ->  {endpoint.ModelTypeText}\n      {msg}");
                    Console.WriteLine($"  FAIL  {endpoint.Key}");
                    Console.WriteLine($"        {msg}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  skip  {endpoint.Key}  (transport: {ex.Message.Split('\n')[0]})");
            }
        }

        Console.WriteLine();
        Console.WriteLine($"PROBE RESULT: {okCount} ok, {failures.Count} deserialization failure(s).");
        foreach (var f in failures)
            Console.WriteLine($"  - {f}");

        return failures.Count > 0 ? 1 : 0;
    }

    private static bool Fillable(string path)
    {
        foreach (var token in Tokens(path))
            if (!Fixtures.ContainsKey(token))
                return false;
        return true;
    }

    private static string Fill(string path)
    {
        var result = path;
        foreach (var token in Tokens(path))
            result = result.Replace($"{{{token}}}", Fixtures[token]);
        return result;
    }

    private static IEnumerable<string> Tokens(string path)
    {
        var i = 0;
        while ((i = path.IndexOf('{', i)) >= 0)
        {
            var j = path.IndexOf('}', i);
            if (j < 0) yield break;
            yield return path.Substring(i + 1, j - i - 1);
            i = j + 1;
        }
    }
}
