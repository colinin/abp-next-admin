using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Mvc;
public class SseAsyncEnumerableResult : IActionResult
{
    private readonly IAsyncEnumerable<string> _asyncEnumerable;
    public SseAsyncEnumerableResult(IAsyncEnumerable<string> asyncEnumerable)
    {
        _asyncEnumerable = asyncEnumerable;
    }
    public async Task ExecuteResultAsync(ActionContext context)
    {
        var response = context.HttpContext.Response;

        response.Headers.Append("Content-Type", "text/event-stream");
        response.Headers.Append("Cache-Control", "no-cache");
        response.Headers.Append("Connection", "keep-alive");
        response.Headers.Append("X-Accel-Buffering", "no");

        try
        {
            await foreach (var content in _asyncEnumerable)
            {
                if (!string.IsNullOrEmpty(content))
                {
                    await response.WriteAsync($"data: {content}\n\n");
                    await response.Body.FlushAsync();
                }
            }

            await response.WriteAsync("data: [DONE]\n\n");
            await response.Body.FlushAsync();
        }
        catch (OperationCanceledException)
        {
            // ignore
        }
    }
}
