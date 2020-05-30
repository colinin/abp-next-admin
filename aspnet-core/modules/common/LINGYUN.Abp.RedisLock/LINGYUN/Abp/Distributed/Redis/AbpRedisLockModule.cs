using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Distributed.Redis
{
    public class AbpRedisLockModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            Configure<RedisLockOptions>(configuration.GetSection("DistributedLock:Redis"));
        }
    }
}
