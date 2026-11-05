using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;

namespace LINGYUN.Abp.OperationRateLimiting;

/// <summary>
/// Interceptor that enforces <see cref="OperationRateLimitingAttribute"/> on application and
/// domain service methods.
/// </summary>
public class OperationRateLimitingInterceptor : AbpInterceptor, ITransientDependency
{
    protected IOperationRateLimitingChecker Checker { get; }

    protected ILogger<OperationRateLimitingInterceptor> Logger { get; }

    public OperationRateLimitingInterceptor(
        IOperationRateLimitingChecker checker,
        ILogger<OperationRateLimitingInterceptor> logger)
    {
        Checker = checker;
        Logger = logger;
    }

    public override async Task InterceptAsync(IAbpMethodInvocation invocation)
    {
        var attribute = invocation.Method.GetCustomAttribute<OperationRateLimitingAttribute>(inherit: true)
            ?? invocation.TargetObject?.GetType().GetCustomAttribute<OperationRateLimitingAttribute>(inherit: true);

        if (attribute == null)
        {
            await invocation.ProceedAsync();
            return;
        }

        var parameter = OperationRateLimitingParameterResolver.Resolve(
            invocation.Method,
            invocation.Arguments,
            warning => Logger.LogWarning(warning));

        await Checker.CheckAsync(attribute.PolicyName, new OperationRateLimitingContext
        {
            Parameter = parameter
        });

        await invocation.ProceedAsync();
    }
}
