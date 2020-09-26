using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Features.Validation
{
    public interface IRequiresLimitFeatureChecker
    {
        Task CheckAsync(RequiresLimitFeatureContext context, CancellationToken cancellation = default);

        Task ProcessAsync(RequiresLimitFeatureContext context, CancellationToken cancellation = default);
    }
}
