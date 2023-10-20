using LINGYUN.Abp.AspNetCore.Wrapper;
using LINGYUN.Abp.Idempotent;
using LINGYUN.Abp.Wrapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http;

namespace LINGYUN.Abp.AspNetCore.Mvc.Idempotent.Wrapper;

[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(AbpIdempotentActionFilter))]
public class AbpWrapIdempotentActionFilter : AbpIdempotentActionFilter, ITransientDependency
{
    protected override void WrapGrantException(IdempotentWrapContext context)
    {
        var options = context.ExecutingContext.GetRequiredService<IOptions<AbpWrapperOptions>>().Value;
        if (!options.IsEnabled)
        {
            base.WrapGrantException(context);
            return;
        }
        var httpResponseWrapper = context.ExecutingContext.GetRequiredService<IHttpResponseWrapper>();
        var errorInfoConverter = context.ExecutingContext.GetRequiredService<IExceptionToErrorInfoConverter>();
        var exceptionHandlingOptions = context.ExecutingContext.GetRequiredService<IOptions<AbpExceptionHandlingOptions>>().Value;

        var errorInfo = new RemoteServiceErrorResponse(
            errorInfoConverter.Convert(context.GrantResult.Exception, options =>
            {
                options.SendExceptionsDetailsToClients = exceptionHandlingOptions.SendExceptionsDetailsToClients;
                options.SendStackTraceToClients = exceptionHandlingOptions.SendStackTraceToClients;
            })
        );
        var result = new WrapResult(errorInfo.Error.Code, errorInfo.Error.Message, errorInfo.Error.Details);
        context.ExecutingContext.Result = new JsonResult(result)
        {
            StatusCode = context.IdempotentOptions.HttpStatusCode,
        };

        var wrapperHeaders = new Dictionary<string, string>()
        {
            { "Content-Type", "application/json" },
            { AbpHttpWrapConsts.AbpWrapResult, "true" },
            { context.IdempotentOptions.IdempotentTokenName, context.GrantResult.IdempotentKey }
        };
        var responseWrapperContext = new HttpResponseWrapperContext(
            context.ExecutingContext.HttpContext,
            context.IdempotentOptions.HttpStatusCode,
            wrapperHeaders);

        httpResponseWrapper.Wrap(responseWrapperContext);
    }
}
