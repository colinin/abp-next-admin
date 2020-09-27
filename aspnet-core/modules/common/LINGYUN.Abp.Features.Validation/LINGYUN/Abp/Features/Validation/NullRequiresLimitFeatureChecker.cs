using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Features.Validation
{
    public class NullRequiresLimitFeatureChecker : IRequiresLimitFeatureChecker, ISingletonDependency
    {
        public Task CheckAsync(RequiresLimitFeatureContext context, CancellationToken cancellation = default)
        {
            return Task.CompletedTask;
        }

        public Task ProcessAsync(RequiresLimitFeatureContext context, CancellationToken cancellation = default)
        {
            return Task.CompletedTask;
        }
    }
}
