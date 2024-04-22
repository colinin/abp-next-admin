namespace System;
internal static class StringContainsExtensions
{
    public static bool Contains(this string source, string check, StringComparison comparison = StringComparison.InvariantCultureIgnoreCase)
    {
        return source.IndexOf(check, comparison) >= 0;
    }
}
