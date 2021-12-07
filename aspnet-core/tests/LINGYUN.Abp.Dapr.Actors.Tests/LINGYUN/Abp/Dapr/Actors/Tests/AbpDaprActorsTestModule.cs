using LINGYUN.Abp.Tests;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Dapr.Actors.Tests
{
    [DependsOn(
        typeof(AbpDaprTestModule),
        typeof(AbpTestsBaseModule),
        typeof(AbpDaprActorsModule))]
    public class AbpDaprActorsTestModule : AbpModule
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
            context.Services.AddDaprActorProxies(
                typeof(AbpDaprTestModule).Assembly,
                "TestDapr");
        }
    }
}
