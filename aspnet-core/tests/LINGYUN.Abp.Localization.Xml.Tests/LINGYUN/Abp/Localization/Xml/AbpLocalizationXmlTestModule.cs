using LINGYUN.Abp.Tests;
using System.IO;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.Localization.Xml
{
    [DependsOn(
        typeof(AbpTestsBaseModule),
        typeof(AbpLocalizationXmlModule))]
    public class AbpLocalizationXmlTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpLocalizationXmlTestModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<LocalizationTestResource>("en")
                    .AddVirtualXml("/LINGYUN/Abp/Localization/Xml/Resources")
                    .AddPhysicalXml(Path.Combine(Directory.GetCurrentDirectory(), "TestResources"));
            });
        }
    }
}
