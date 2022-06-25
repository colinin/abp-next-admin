using LINGYUN.Abp.IM.Localization;
using LINGYUN.Abp.MessageService.Chat;
using LINGYUN.Abp.MessageService.Localization;
using LINGYUN.Abp.MessageService.Mapper;
using LINGYUN.Abp.MessageService.ObjectExtending;
using LINGYUN.Abp.Notifications.Localization;
using Volo.Abp.AutoMapper;
using Volo.Abp.Caching;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending.Modularity;

namespace LINGYUN.Abp.MessageService
{
    [DependsOn(
        typeof(AbpAutoMapperModule),
        typeof(AbpCachingModule),
        typeof(AbpMessageServiceDomainSharedModule))]
    public class AbpMessageServiceDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<MessageServiceDomainAutoMapperProfile>(validate: true);
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<MessageServiceResource>()
                    .AddBaseTypes(typeof(NotificationsResource), typeof(AbpIMResource));
            });
        }

        public override void PostConfigureServices(ServiceConfigurationContext context)
        {
            ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToEntity(
                MessageServiceModuleExtensionConsts.ModuleName,
                MessageServiceModuleExtensionConsts.EntityNames.Message,
                typeof(Message)
            );
        }
    }
}
