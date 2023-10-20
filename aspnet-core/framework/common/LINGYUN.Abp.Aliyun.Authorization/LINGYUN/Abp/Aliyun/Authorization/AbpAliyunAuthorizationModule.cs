using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Aliyun.Authorization
{
    public class AbpAliyunAuthorizationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            Configure<AbpAliyunOptions>(configuration.GetSection("Aliyun:Auth"));
        }
    }
}
