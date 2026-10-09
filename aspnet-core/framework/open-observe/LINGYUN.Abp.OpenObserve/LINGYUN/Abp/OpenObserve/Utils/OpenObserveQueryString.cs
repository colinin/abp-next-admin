using System;
using System.Collections.Generic;
using System.Text;

namespace LINGYUN.Abp.OpenObserve.Utils;

/// <summary>
/// OpenObserve 查询字符串构造
/// </summary>
internal static class OpenObserveQueryString
{
    /// <summary>
    /// 构造查询字符串
    /// </summary>
    /// <returns>如 "?type=logs&amp;delete_all=true", 无参数时返回空字符串</returns>
    public static string Build(params (string Name, object? Value)[] parameters)
    {
        var builder = new StringBuilder();

        foreach (var (name, value) in parameters)
        {
            if (value == null)
            {
                continue;
            }

            var text = value switch
            {
                bool boolean => boolean ? "true" : "false",
                _ => value.ToString(),
            };

            if (text.IsNullOrWhiteSpace())
            {
                continue;
            }

            builder.Append(builder.Length == 0 ? '?' : '&');
            builder.Append(name);
            builder.Append('=');
            builder.Append(Uri.EscapeDataString(text!));
        }

        return builder.ToString();
    }

    /// <summary>
    /// 构造查询字符串
    /// </summary>
    public static string Build(IDictionary<string, object?> parameters)
    {
        var builder = new StringBuilder();

        foreach (var (name, value) in parameters)
        {
            if (value == null)
            {
                continue;
            }

            var text = value switch
            {
                bool boolean => boolean ? "true" : "false",
                _ => value.ToString(),
            };

            if (text.IsNullOrWhiteSpace())
            {
                continue;
            }

            builder.Append(builder.Length == 0 ? '?' : '&');
            builder.Append(name);
            builder.Append('=');
            builder.Append(Uri.EscapeDataString(text!));
        }

        return builder.ToString();
    }
}
