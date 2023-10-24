using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.Features.LimitValidation.Redis
{
    public class RedisLimitFeatureNamingNormalizer : IRedisLimitFeatureNamingNormalizer, ISingletonDependency
    {
        protected ICurrentTenant CurrentTenant { get; }

        public RedisLimitFeatureNamingNormalizer(
            ICurrentTenant currentTenant)
        {
            CurrentTenant = currentTenant;
        }

        public virtual string NormalizeFeatureName(string instance, RequiresLimitFeatureContext context)
        {
            if (CurrentTenant.IsAvailable)
            {
                return $"{instance}t:RequiresLimitFeature;t:{CurrentTenant.Id};f:{context.LimitFeature}";
            }
            return $"{instance}c:RequiresLimitFeature;f:{context.LimitFeature}";
        }
    }
}
