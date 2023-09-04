using System.Linq;

namespace System;
public static class StringArrayArgsExtensions
{
    public static string GetStringPrarm(this string[] args, string key)
    {
        if (!args.Any())
        {
            return null;
        }

        return args
            .Where(arg => arg.StartsWith(key))
            .Select(arg => arg.Substring(key.Length))
            .FirstOrDefault();
    }

    public static string GetInt32Prarm(this string[] args, string key)
    {
        if (!args.Any())
        {
            return null;
        }

        return args
            .Where(arg => arg.StartsWith(key))
            .Select(arg => arg.Substring(key.Length))
            .FirstOrDefault(arg => int.TryParse(arg, out _));
    }
}
