using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Clients;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.Features.LimitValidation.Redis.Client
{
    [Dependency(ServiceLifetime.Singleton, ReplaceServices = true)]
    [ExposeServices(
        typeof(IRedisLimitFeatureNamingNormalizer),
        typeof(RedisLimitFeatureNamingNormalizer))]
    public class RedisClientLimitFeatureNamingNormalizer : RedisLimitFeatureNamingNormalizer
    {
        protected ICurrentClient CurrentClient { get; }
        public RedisClientLimitFeatureNamingNormalizer(
            ICurrentClient currentClient,
            ICurrentTenant currentTenant) : base(currentTenant)
        {
            CurrentClient = currentClient;
        }

        public override string NormalizeFeatureName(string instance, RequiresLimitFeatureContext context)
        {
            if (CurrentClient.IsAuthenticated)
            {
                return CurrentTenant.IsAvailable
                    ? $"{instance}t:RequiresLimitFeature;t:{CurrentTenant.Id};c:{CurrentClient.Id};f:{context.LimitFeature}"
                    : $"{instance}tc:RequiresLimitFeature;c:{CurrentClient.Id};f:{context.LimitFeature}";
            }
            return base.NormalizeFeatureName(instance, context);
        }
    }
}
