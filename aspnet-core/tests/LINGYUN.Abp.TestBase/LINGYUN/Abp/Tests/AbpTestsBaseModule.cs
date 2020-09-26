using LINGYUN.Abp.Tests.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp;
using Volo.Abp.Authorization;
using Volo.Abp.Autofac;
using Volo.Abp.Features;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Tests
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpTestBaseModule),
        typeof(AbpAuthorizationModule),
        typeof(AbpFeaturesModule)
        )]
    public class AbpTestsBaseModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAlwaysAllowAuthorization();

            context.Services.Replace(ServiceDescriptor.Singleton<IFeatureStore, FakeFeatureStore>());
        }
    }
}
