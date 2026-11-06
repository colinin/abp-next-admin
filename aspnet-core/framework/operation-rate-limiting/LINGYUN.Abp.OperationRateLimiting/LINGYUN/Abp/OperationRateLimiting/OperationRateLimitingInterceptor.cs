using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Reflection;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;

namespace LINGYUN.Abp.OperationRateLimiting;

/// <summary>
/// Interceptor that enforces <see cref="OperationRateLimitingAttribute"/> on application and
/// domain service methods.
/// </summary>
public class OperationRateLimitingInterceptor : AbpInterceptor, ITransientDependency
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public OperationRateLimitingInterceptor(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public override async Task InterceptAsync(IAbpMethodInvocation invocation)
    {
        using (var serviceScope = _serviceScopeFactory.CreateScope())
        {
            var options = serviceScope.ServiceProvider.GetRequiredService<IOptions<AbpOperationRateLimitingOptions>>().Value;
            var attribute = ResolveAttribute(invocation, options);
            if (attribute == null)
            {
                await invocation.ProceedAsync();
                return;
            }

            var checker = serviceScope.ServiceProvider.GetRequiredService<IOperationRateLimitingChecker>();
            var logger = serviceScope.ServiceProvider.GetService<ILogger<OperationRateLimitingInterceptor>>();
            var parameter = OperationRateLimitingParameterResolver.Resolve(
                invocation.Method,
                invocation.Arguments,
                warning => logger?.LogWarning(warning));

            await checker.CheckAsync(attribute!.PolicyName,
                new OperationRateLimitingContext
                {
                    Parameter = parameter
                });

            await invocation.ProceedAsync();
        }
    }

    protected virtual OperationRateLimitingAttribute? ResolveAttribute(
        IAbpMethodInvocation invocation,
        AbpOperationRateLimitingOptions options)
    {
        if (!options.IsEnabled)
        {
            return null;
        }

        var attribute = invocation.Method.GetCustomAttribute<OperationRateLimitingAttribute>(inherit: true)
            ?? invocation.Method.DeclaringType!.GetCustomAttribute<OperationRateLimitingAttribute>(inherit: true);

        return attribute;
    }
}
