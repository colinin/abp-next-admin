using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.Http;
using Volo.Abp.Json;

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
            var jsonSerializer = context.HttpContext.RequestServices.GetRequiredService<IJsonSerializer>();

            await foreach (var content in _asyncEnumerable)
            {
                if (!string.IsNullOrEmpty(content))
                {
                    var sseResult = new SseResult(content);
                    await response.WriteAsync($"data: {jsonSerializer.Serialize(sseResult)}\n\n");
                    await response.Body.FlushAsync();
                }
            }

            await response.WriteAsync($"data: {jsonSerializer.Serialize(new SseResult("FINISHED"))}\n\n");
            await response.Body.FlushAsync();
        }
        catch (OperationCanceledException)
        {
            // ignore
        }
        catch (Exception ex)
        {
            var exceptionHandlingOptions = context.HttpContext.RequestServices.GetRequiredService<IOptions<AbpExceptionHandlingOptions>>().Value;

            var exceptionToErrorInfoConverter = context.HttpContext.RequestServices.GetRequiredService<IExceptionToErrorInfoConverter>();
            var remoteServiceErrorInfo = exceptionToErrorInfoConverter.Convert(ex, options =>
            {
                options.SendExceptionsDetailsToClients = exceptionHandlingOptions.SendExceptionsDetailsToClients;
                options.SendExceptionDataToClientTypes = exceptionHandlingOptions.SendExceptionDataToClientTypes;
                options.SendStackTraceToClients = exceptionHandlingOptions.SendStackTraceToClients;
            });

            if (exceptionHandlingOptions.ShouldLogException(ex))
            {
                var logger = context.HttpContext.RequestServices.GetService<ILogger<SseAsyncEnumerableResult>>();
                var logLevel = ex.GetLogLevel();
                logger?.LogException(ex, logLevel);
            }


            response.Headers.RemoveAll(x => x.Key == "Content-Type");
            response.Headers.Append("Content-Type", "application/json");
            response.Headers.Append(AbpHttpConsts.AbpErrorFormat, "true");
            response.StatusCode = (int)context.HttpContext.RequestServices
                .GetRequiredService<IHttpExceptionStatusCodeFinder>()
                .GetStatusCode(context.HttpContext, ex);

            await response.WriteAsJsonAsync(remoteServiceErrorInfo);
            await response.Body.FlushAsync();
        }
    }
}
