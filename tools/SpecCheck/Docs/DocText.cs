using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace ESI.NET.Tools.SpecCheck.Docs;

/// <summary>Text helpers: judging spec descriptions, and turning text into XML doc lines.</summary>
public static class DocText
{
    /// <summary>
    /// A description that only restates the field name ("Alliance's name" for <c>name</c>,
    /// "type_id integer") tells the reader nothing the name didn't. Those are "weak": used
    /// only when nothing better exists.
    /// </summary>
    public static bool IsWeak(string? description, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(description))
            return true;

        var text = Squash(description);
        var name = Squash(fieldName);
        var words = description.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);

        if (text == name || text.Length == 0)
            return true;
        if (text == name + "integer" || text == name + "string" || text == name + "number" || text == name + "boolean")
            return true;

        // "Alliance's name", "The character's ID", "Corporation ID": a possessive or noun
        // followed by the field name itself, nothing more.
        var nameWords = Regex.Replace(fieldName, "_id$", "").Split('_');
        if (words.Length <= 4 && nameWords.All(w => text.Contains(Squash(w), StringComparison.Ordinal)))
            return true;

        return false;
    }

    private static string Squash(string s) => Regex.Replace(s.ToLowerInvariant(), "[^a-z0-9]", "");

    /// <summary>
    /// Spec text as XML doc content: XML-escaped, <c>`code`</c> spans as <c>&lt;c&gt;</c>, and a
    /// trailing period. Text from docs.json is already XML (it may hold <c>&lt;see cref&gt;</c>), so
    /// it is passed with <paramref name="isXml"/> and only normalized.
    /// </summary>
    public static string Sentence(string text, bool isXml = false)
    {
        var t = Regex.Replace(text.Trim(), @"\s+", " ");
        if (t.Length > 1 && t[0] == '"' && t[^1] == '"')
            t = t[1..^1].Trim();
        if (!isXml)
        {
            t = WebUtility.HtmlEncode(t).Replace("&#39;", "'").Replace("&quot;", "\"");
            t = Regex.Replace(t, "`([^`]+)`", "<c>$1</c>");
        }
        if (t.Length > 0 && char.IsLower(t[0]) && !isXml)
            t = char.ToUpperInvariant(t[0]) + t[1..];
        if (t.Length > 0 && !".!?:".Contains(t[^1]) && !t.EndsWith(">", StringComparison.Ordinal))
            t += ".";
        return t;
    }

    /// <summary>
    /// Wraps <paramref name="text"/> into <c>///</c> lines no wider than <paramref name="width"/>,
    /// never breaking inside an XML tag.
    /// </summary>
    public static IEnumerable<string> Wrap(string text, int width)
    {
        var tokens = Regex.Matches(text, @"(?:<[^>]*>|[^\s<])+").Select(m => m.Value);
        var line = new StringBuilder();
        foreach (var token in tokens)
        {
            if (line.Length > 0 && line.Length + 1 + token.Length > width)
            {
                yield return line.ToString();
                line.Clear();
            }
            if (line.Length > 0)
                line.Append(' ');
            line.Append(token);
        }
        if (line.Length > 0)
            yield return line.ToString();
    }

    /// <summary><c>fromId</c> -&gt; <c>from_id</c>.</summary>
    public static string Snake(string camel) => Regex.Replace(camel, "(?<=[a-z0-9])([A-Z])", "_$1").ToLowerInvariant();

    /// <summary>A cache TTL in seconds as prose: 300 -&gt; "5 minutes".</summary>
    public static string Duration(int seconds) => seconds switch
    {
        < 60 => seconds == 1 ? "1 second" : $"{seconds} seconds",
        < 3600 when seconds % 60 == 0 => seconds == 60 ? "1 minute" : $"{seconds / 60} minutes",
        < 86400 when seconds % 3600 == 0 => seconds == 3600 ? "1 hour" : $"{seconds / 3600} hours",
        _ when seconds % 86400 == 0 => seconds == 86400 ? "1 day" : $"{seconds / 86400} days",
        _ => $"{seconds} seconds",
    };
}
