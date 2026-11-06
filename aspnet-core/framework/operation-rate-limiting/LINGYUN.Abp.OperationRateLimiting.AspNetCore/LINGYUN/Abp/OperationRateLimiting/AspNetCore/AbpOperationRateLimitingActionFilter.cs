using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.OperationRateLimiting.AspNetCore;

/// <summary>
/// Enforces <see cref="OperationRateLimitingAttribute"/> on MVC controller actions.
/// It is registered automatically by <see cref="AbpOperationRateLimitingAspNetCoreModule"/>.
/// </summary>
public class AbpOperationRateLimitingActionFilter : IAsyncActionFilter, ITransientDependency
{
    public virtual async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.ActionDescriptor is not ControllerActionDescriptor descriptor)
        {
            await next();
            return;
        }

        var attribute = ResolveAttribute(context, descriptor);
        if (attribute == null)
        {
            await next();
            return;
        }

        var checker = context.GetRequiredService<IOperationRateLimitingChecker>();
        var logger = context.GetService<ILogger<AbpOperationRateLimitingActionFilter>>();

        var parameter = OperationRateLimitingParameterResolver.Resolve(
            descriptor.MethodInfo,
            BuildArguments(descriptor, context),
            warning => logger?.LogWarning(warning));

        await checker.CheckAsync(attribute.PolicyName, new OperationRateLimitingContext
        {
            Parameter = parameter
        });

        await next();
    }

    protected virtual OperationRateLimitingAttribute? ResolveAttribute(
        ActionExecutingContext context,
        ControllerActionDescriptor descriptor)
    {
        var options = context.GetRequiredService<IOptions<AbpOperationRateLimitingOptions>>().Value;
        if (!options.IsEnabled)
        {
            return null;
        }

        var endpointAttribute = context.ActionDescriptor.EndpointMetadata
            .OfType<OperationRateLimitingAttribute>()
            .FirstOrDefault();

        if (endpointAttribute != null)
        {
            return endpointAttribute;
        }

        return descriptor.MethodInfo.GetCustomAttribute<OperationRateLimitingAttribute>(inherit: true)
               ?? descriptor.ControllerTypeInfo.GetCustomAttribute<OperationRateLimitingAttribute>(inherit: true);
    }

    protected virtual object?[] BuildArguments(
        ControllerActionDescriptor descriptor,
        ActionExecutingContext context)
    {
        var parameters = descriptor.MethodInfo.GetParameters();
        var arguments = new object?[parameters.Length];

        for (var index = 0; index < parameters.Length; index++)
        {
            arguments[index] = parameters[index].Name != null &&
                               context.ActionArguments.TryGetValue(parameters[index].Name!, out var value)
                ? value
                : null;
        }

        return arguments;
    }
}
