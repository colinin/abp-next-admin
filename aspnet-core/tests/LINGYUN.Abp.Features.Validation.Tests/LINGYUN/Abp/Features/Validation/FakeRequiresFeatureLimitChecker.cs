using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Authorization;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Features.Validation
{
    [Dependency(ServiceLifetime.Singleton, ReplaceServices = true)]
    [ExposeServices(typeof(IRequiresLimitFeatureChecker))]
    public class FakeRequiresFeatureLimitChecker : IRequiresLimitFeatureChecker
    {
        private readonly IDictionary<string, int> limitFeatures;

        public FakeRequiresFeatureLimitChecker()
        {
            limitFeatures = new Dictionary<string, int>();
        }

        public virtual Task CheckAsync(RequiresLimitFeatureContext context, CancellationToken cancellation = default)
        {
            if (!limitFeatures.ContainsKey(context.Feature))
            {
                limitFeatures.Add(context.Feature, 0);
            }
            if (limitFeatures[context.Feature] > context.Limit)
            {
                throw new AbpAuthorizationException("已经超出功能次数限制,请联系管理员");
            }
            return Task.CompletedTask;
        }

        public Task ProcessAsync(RequiresLimitFeatureContext context, CancellationToken cancellation = default)
        {
            if (!limitFeatures.ContainsKey(context.Feature))
            {
                limitFeatures.Add(context.Feature, 1);
            }
            limitFeatures[context.Feature] += 1;
            return Task.CompletedTask;
        }
    }
}
