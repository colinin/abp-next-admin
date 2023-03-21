using LINGYUN.Abp.Idempotent;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.AspNetCore.Mvc.Idempotent;

public class AbpIdempotentActionFilter : IAsyncActionFilter, ITransientDependency
{
    public async virtual Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!ShouldCheckIdempotent(context))
        {
            await next();
            return;
        }

        var idempotentChecker = context.GetRequiredService<IIdempotentChecker>();
        var options = context.GetRequiredService<IOptions<AbpIdempotentOptions>>().Value;

        var targetType = context.ActionDescriptor.AsControllerActionDescriptor().ControllerTypeInfo.AsType();
        var methodInfo = context.ActionDescriptor.AsControllerActionDescriptor().MethodInfo;

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
                context.ActionArguments.ToImmutableDictionary());

            idempotentKey = idempotentKeyNormalizer.NormalizeKey(keyNormalizerContext);
        }

        var checkContext = new IdempotentCheckContext(
            targetType,
            methodInfo,
            idempotentKey,
            context.ActionArguments.ToImmutableDictionary());
        // 进行幂等性校验
        await idempotentChecker.CheckAsync(checkContext);

        await next();
    }

    protected virtual bool ShouldCheckIdempotent(ActionExecutingContext context)
    {
        var options = context.GetRequiredService<IOptions<AbpIdempotentOptions>>().Value;
        if (!options.IsEnabled)
        {
            return false;
        }

        var mvcIdempotentOptions = context.GetRequiredService<IOptions<AbpAspNetCoreMvcIdempotentOptions>>().Value;
        if (mvcIdempotentOptions.SupportedMethods.Any() &&
            !mvcIdempotentOptions.SupportedMethods.Contains(context.HttpContext.Request.Method))
        {
            return false;
        }

        return true;
    }
}
