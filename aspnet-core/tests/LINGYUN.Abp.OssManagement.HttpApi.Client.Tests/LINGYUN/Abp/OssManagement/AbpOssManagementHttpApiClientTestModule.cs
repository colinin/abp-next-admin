using LINGYUN.Abp.Tests;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.OssManagement
{
    [DependsOn(
        typeof(AbpTestsBaseModule),
        typeof(AbpOssManagementHttpApiClientModule))]
    public class AbpOssManagementHttpApiClientTestModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.ReplaceConfiguration(
                ConfigurationHelper.BuildConfiguration(
                    new AbpConfigurationBuilderOptions
                    {
                        BasePath = @"D:\Projects\Development\Abp\OssManagement",
                        EnvironmentName = "Development"
                    }));
        }
    }
}
