using LINGYUN.Abp.Tests.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
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
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            var configurationOptions = new AbpConfigurationBuilderOptions
            {
                EnvironmentName = "Development",
                // 测试环境机密配置
                UserSecretsId = Environment.GetEnvironmentVariable("APPLICATION_USER_SECRETS_ID"),
                UserSecretsAssembly = typeof(AbpTestsBaseModule).Assembly
            };

            context.Services.ReplaceConfiguration(ConfigurationHelper.BuildConfiguration(configurationOptions));
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAlwaysAllowAuthorization();

            context.Services.Replace(ServiceDescriptor.Singleton<IFeatureStore, FakeFeatureStore>());
        }
    }
}
