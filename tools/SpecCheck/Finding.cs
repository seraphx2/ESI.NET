namespace ESI.NET.Tools.SpecCheck;

public enum Severity
{
    Info,     // reported, never fails the run
    Warning,  // fails the run (a field / endpoint / enum changed)
    Error,    // fails the run (a type or shape is wrong)
}

/// <summary>A single drift observation. <paramref name="Where"/> is an endpoint key or type name.</summary>
public sealed record Finding(Severity Severity, string Category, string Where, string Message)
{
    public override string ToString() => $"[{Severity.ToString().ToUpperInvariant()}] {Category}: {Where} - {Message}";
}
