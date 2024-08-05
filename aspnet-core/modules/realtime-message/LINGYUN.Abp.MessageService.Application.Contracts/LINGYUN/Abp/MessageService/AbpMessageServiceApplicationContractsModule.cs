using LINGYUN.Abp.MessageService.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.MessageService;

[DependsOn(typeof(AbpMessageServiceDomainSharedModule))]
public class AbpMessageServiceApplicationContractsModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpMessageServiceApplicationContractsModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                   .Get<MessageServiceResource>()
                   .AddVirtualJson("/LINGYUN/Abp/MessageService/Localization/ApplicationContracts");
        });
    }
}
