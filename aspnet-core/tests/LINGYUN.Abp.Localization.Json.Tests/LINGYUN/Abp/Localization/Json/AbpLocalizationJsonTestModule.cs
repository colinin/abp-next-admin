using LINGYUN.Abp.Tests;
using System.IO;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Localization.Json
{
    [DependsOn(
       typeof(AbpTestsBaseModule),
       typeof(AbpLocalizationJsonModule))]
    public class AbpLocalizationJsonTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<LocalizationTestResource>("en")
                    .AddPhysicalJson(Path.Combine(Directory.GetCurrentDirectory(), "TestResources"));
            });
        }
    }
}
