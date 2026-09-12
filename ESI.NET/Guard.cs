using System;

namespace ESI.NET
{
    /// <summary>
    /// Centralizes the <c>if (x == null) throw new ArgumentNullException(nameof(x))</c> pattern used
    /// throughout the public constructors. On net8.0 this is exactly
    /// <see cref="ArgumentNullException.ThrowIfNull(object, string)"/> (which doesn't exist on
    /// netstandard2.0, hence the split) - one place to hold the <c>#if NET</c> instead of repeating it
    /// at every call site.
    /// </summary>
    internal static class Guard
    {
        public static void NotNull(object value, string paramName)
        {
#if NET
            ArgumentNullException.ThrowIfNull(value, paramName);
#else
            if (value == null) throw new ArgumentNullException(paramName);
#endif
        }
    }
}
