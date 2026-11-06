using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.OperationRateLimiting;

[OperationRateLimiting(TestPolicyNames.ClassLevel)]
public class TestClassLevelRateLimitingService : ITransientDependency
{
    public virtual Task InvokeAsync([RateLimitingParameter] string key)
    {
        return Task.CompletedTask;
    }
}
