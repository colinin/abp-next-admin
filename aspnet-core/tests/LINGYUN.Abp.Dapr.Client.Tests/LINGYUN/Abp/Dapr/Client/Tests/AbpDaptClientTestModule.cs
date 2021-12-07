using LINGYUN.Abp.Tests;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.IO;
using LINGYUN.Abp.Dapr.Client.Wrapper;

namespace LINGYUN.Abp.Dapr.Client.Tests
{
    [DependsOn(
        typeof(AbpDaprTestModule),
        typeof(AbpTestsBaseModule),
        typeof(AbpDaprClientModule),
        typeof(AbpDaprClientWrapperModule))]
    public class AbpDaptClientTestModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.ReplaceConfiguration(
                ConfigurationHelper.BuildConfiguration(
                    new AbpConfigurationBuilderOptions
                    {
                        EnvironmentName = "Testing",
                        BasePath = Directory.GetCurrentDirectory()
                    }));
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddDaprClientProxies(
                typeof(AbpDaprTestModule).Assembly,
                "TestDapr");
        }
    }
}
