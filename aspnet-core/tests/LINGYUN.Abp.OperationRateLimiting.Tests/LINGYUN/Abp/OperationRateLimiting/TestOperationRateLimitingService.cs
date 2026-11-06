using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.OperationRateLimiting;

public class TestOperationRateLimitingService : ITransientDependency
{
    public int InvokedCount { get; private set; }

    [OperationRateLimiting(TestPolicyNames.Attribute)]
    public virtual Task InvokeAsync([RateLimitingParameter] string key)
    {
        InvokedCount++;
        return Task.CompletedTask;
    }

    [OperationRateLimiting(TestPolicyNames.Attribute)]
    public virtual Task InvokeWithDtoAsync(TestRateLimitingInput input)
    {
        InvokedCount++;
        return Task.CompletedTask;
    }

    [OperationRateLimiting(TestPolicyNames.Attribute)]
    public virtual Task InvokeWithTwoParametersAsync(
        [RateLimitingParameter] string first,
        [RateLimitingParameter] string second)
    {
        return Task.CompletedTask;
    }
}
