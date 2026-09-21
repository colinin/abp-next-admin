using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace System;
internal static class StringsExtensions
{
    internal static bool HasApplicationFormContentType(this HttpRequest request)
    {
        if (request.ContentType is null) return false;

        if (MediaTypeHeaderValue.TryParse(request.ContentType, out var header))
        {
            // Content-Type: application/x-www-form-urlencoded; charset=utf-8
            return header.MediaType.Equals("application/x-www-form-urlencoded", StringComparison.OrdinalIgnoreCase);
        }

        return false;
    }
}
