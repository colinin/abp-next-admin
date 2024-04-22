using LINGYUN.Abp.Idempotent;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Volo.Abp.Aspects;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http;

namespace LINGYUN.Abp.AspNetCore.Mvc.Idempotent;

public class AbpIdempotentActionFilter : IAsyncActionFilter, ITransientDependency
{
    [IgnoreIdempotent]
    public async virtual Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!ShouldCheckIdempotent(context))
        {
            await next();
            return;
        }

        using (AbpCrossCuttingConcerns.Applying(context.Controller, IdempotentCrossCuttingConcerns.Idempotent))
        {
            var idempotentChecker = context.GetRequiredService<IIdempotentChecker>();
            var options = context.GetRequiredService<IOptions<AbpIdempotentOptions>>().Value;

            var targetType = context.ActionDescriptor.AsControllerActionDescriptor().ControllerTypeInfo.AsType();
            var methodInfo = context.ActionDescriptor.AsControllerActionDescriptor().MethodInfo;

            var actionArguments = context.ActionArguments.ToImmutableDictionary();
            string idempotentKey;
            // 可以用户传递幂等性token
            // 否则通过用户定义action创建token
            if (context.HttpContext.Request.Headers.TryGetValue(options.IdempotentTokenName, out var key))
            {
                idempotentKey = key.ToString();
            }
            else
            {
                var idempotentKeyNormalizer = context.GetRequiredService<IIdempotentKeyNormalizer>();

                var keyNormalizerContext = new IdempotentKeyNormalizerContext(
                    targetType,
                    methodInfo,
                    actionArguments);

                idempotentKey = idempotentKeyNormalizer.NormalizeKey(keyNormalizerContext);
            }

            var checkContext = new IdempotentCheckContext(
                targetType,
                methodInfo,
                idempotentKey,
                actionArguments);
            // 进行幂等性校验
            await using var grant = await idempotentChecker.IsGrantAsync(checkContext);
            if (grant.Successed)
            {
                await next();
                return ;
            }
            var wrapContext = new IdempotentWrapContext(context, options, grant);
            WrapGrantException(wrapContext);
        }
    }

    protected virtual bool ShouldCheckIdempotent(ActionExecutingContext context)
    {
        var options = context.GetRequiredService<IOptions<AbpIdempotentOptions>>().Value;
        if (!options.IsEnabled)
        {
            return false;
        }

        var mvcIdempotentOptions = context.GetRequiredService<IOptions<AbpAspNetCoreMvcIdempotentOptions>>().Value;
        if (!mvcIdempotentOptions.SupportedMethods.Contains(context.HttpContext.Request.Method))
        {
            return false;
        }

        return true;
    }

    protected virtual void WrapGrantException(IdempotentWrapContext context)
    {
        var errorInfoConverter = context.ExecutingContext.GetRequiredService<IExceptionToErrorInfoConverter>();
        var exceptionHandlingOptions = context.ExecutingContext.GetRequiredService<IOptions<AbpExceptionHandlingOptions>>().Value;

        var errorInfo = new RemoteServiceErrorResponse(
            errorInfoConverter.Convert(context.GrantResult.Exception, options =>
            {
                options.SendExceptionsDetailsToClients = exceptionHandlingOptions.SendExceptionsDetailsToClients;
                options.SendStackTraceToClients = exceptionHandlingOptions.SendStackTraceToClients;
            })
        );
        context.ExecutingContext.Result = new JsonResult(errorInfo)
        {
            StatusCode = context.IdempotentOptions.HttpStatusCode,
        };

        context.ExecutingContext.HttpContext.Response.Headers.Append(
            context.IdempotentOptions.IdempotentTokenName,
            context.GrantResult.IdempotentKey);
        context.ExecutingContext.HttpContext.Response.Headers.Append(AbpHttpConsts.AbpErrorFormat, "true");
        context.ExecutingContext.HttpContext.Response.Headers.Append("Content-Type", "application/json");
    }
}
