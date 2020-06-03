using LINGYUN.Abp.MessageService.Localization;
using LINGYUN.Abp.MessageService.Mapper;
using Volo.Abp.AutoMapper;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.MessageService
{
    [DependsOn(
        typeof(AbpAutoMapperModule),
        typeof(AbpMessageServiceDomainSharedModule))]
    public class AbpMessageServiceDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<MessageServiceDomainAutoMapperProfile>(validate: true);
            });

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpMessageServiceDomainModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                       .Get<MessageServiceResource>()
                       .AddVirtualJson("/LINGYUN/Abp/MessageService/Localization/Resources");
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("Messages:Group", typeof(MessageServiceResource));
                options.MapCodeNamespace("Messages:User", typeof(MessageServiceResource));
            });
        }
    }
}
