using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Features.LimitValidation
{
    public class NullRequiresLimitFeatureChecker : IRequiresLimitFeatureChecker, ISingletonDependency
    {
        public Task<bool> CheckAsync(RequiresLimitFeatureContext context, CancellationToken cancellation = default)
        {
            return Task.FromResult(true);
        }

        public Task ProcessAsync(RequiresLimitFeatureContext context, CancellationToken cancellation = default)
        {
            return Task.CompletedTask;
        }
    }
}
