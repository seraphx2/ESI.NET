namespace ESI.NET
{
    /// <summary>
    /// The ESI compatibility date this build of ESI.NET targets.
    /// <para>
    /// Sent as the <c>X-Compatibility-Date</c> header on every request, and the
    /// snapshot the models are shaped for. ESI freezes each dated snapshot, so a
    /// consumer stays on this contract until they upgrade the package. Bumping it
    /// is a release of its own, with a migration note - see MIGRATION.md. The
    /// published dates are listed at <c>/meta/compatibility-dates</c>.
    /// </para>
    /// </summary>
    public static class EsiVersion
    {
        public const string CompatibilityDate = "2026-08-18";
    }
}
