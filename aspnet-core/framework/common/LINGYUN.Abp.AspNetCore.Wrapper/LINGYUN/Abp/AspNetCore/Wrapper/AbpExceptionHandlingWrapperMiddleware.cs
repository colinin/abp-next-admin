using LINGYUN.Abp.Wrapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Authorization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Json;

namespace LINGYUN.Abp.AspNetCore.Wrapper;

public class AbpExceptionHandlingWrapperMiddleware : IMiddleware, ITransientDependency
{
    private readonly ILogger<AbpExceptionHandlingWrapperMiddleware> _logger;

    private readonly Func<object, Task> _clearCacheHeadersDelegate;

    public AbpExceptionHandlingWrapperMiddleware(ILogger<AbpExceptionHandlingWrapperMiddleware> logger)
    {
        _logger = logger;

        _clearCacheHeadersDelegate = ClearCacheHeaders;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            // We can't do anything if the response has already started, just abort.
            if (context.Response.HasStarted)
            {
                _logger.LogWarning("An exception occurred, but response has already started!");
                throw;
            }

            if (context.Items["_AbpActionInfo"] is AbpActionInfoInHttpContext actionInfo)
            {
                if (actionInfo.IsObjectResult) //TODO: Align with AbpExceptionFilter.ShouldHandleException!
                {
                    await HandleAndWrapException(context, ex);
                    return;
                }
            }

            throw;
        }
    }

    private async Task HandleAndWrapException(HttpContext httpContext, Exception exception)
    {
        _logger.LogException(exception);

        await httpContext
            .RequestServices
            .GetRequiredService<IExceptionNotifier>()
            .NotifyAsync(
                new ExceptionNotificationContext(exception)
            );

        if (exception is AbpAuthorizationException)
        {
            await httpContext.RequestServices.GetRequiredService<IAbpAuthorizationExceptionHandler>()
                .HandleAsync(exception.As<AbpAuthorizationException>(), httpContext);
        }
        else
        {
            var jsonSerializer = httpContext.RequestServices.GetRequiredService<IJsonSerializer>();
            var wrapOptions = httpContext.RequestServices.GetRequiredService<IOptions<AbpWrapperOptions>>().Value;
            var httpResponseWrapper = httpContext.RequestServices.GetRequiredService<IHttpResponseWrapper>();
            var statusCodFinder = httpContext.RequestServices.GetRequiredService<IHttpExceptionStatusCodeFinder>();
            var exceptionWrapHandler = httpContext.RequestServices.GetRequiredService<IExceptionWrapHandlerFactory>();
            var errorInfoConverter = httpContext.RequestServices.GetRequiredService<IExceptionToErrorInfoConverter>();
            var exceptionHandlingOptions = httpContext.RequestServices.GetRequiredService<IOptions<AbpExceptionHandlingOptions>>().Value;

            var remoteServiceErrorInfo = errorInfoConverter.Convert(exception, options =>
            {
                options.SendExceptionsDetailsToClients = exceptionHandlingOptions.SendExceptionsDetailsToClients;
                options.SendStackTraceToClients = exceptionHandlingOptions.SendStackTraceToClients;
            });

            var exceptionWrapContext = new ExceptionWrapContext(
                exception,
                remoteServiceErrorInfo,
                httpContext.RequestServices,
                statusCodFinder.GetStatusCode(httpContext, exception));

            exceptionWrapHandler.CreateFor(exceptionWrapContext).Wrap(exceptionWrapContext);

            var wrapperHeaders = new Dictionary<string, string>()
            {
                { AbpHttpWrapConsts.AbpWrapResult, "true" }
            };
            var responseWrapperContext = new HttpResponseWrapperContext(
                httpContext,
                (int)wrapOptions.HttpStatusCode,
                wrapperHeaders);

            httpResponseWrapper.Wrap(responseWrapperContext);

            httpContext.Response.Clear();
            httpContext.Response.OnStarting(_clearCacheHeadersDelegate, httpContext.Response);
            httpContext.Response.Headers.Append("Content-Type", "application/json");

            var wrapResult = new WrapResult(
                exceptionWrapContext.ErrorInfo.Code,
                exceptionWrapContext.ErrorInfo.Message,
                exceptionWrapContext.ErrorInfo.Details);
            await httpContext.Response.WriteAsync(jsonSerializer.Serialize(wrapResult));
        }
    }

    private Task ClearCacheHeaders(object state)
    {
        var response = (HttpResponse)state;

        response.Headers[HeaderNames.CacheControl] = "no-cache";
        response.Headers[HeaderNames.Pragma] = "no-cache";
        response.Headers[HeaderNames.Expires] = "-1";
        response.Headers.Remove(HeaderNames.ETag);

        return Task.CompletedTask;
    }
}
