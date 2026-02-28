using LINGYUN.Platform.Datas;
using LINGYUN.Platform.Layouts;
using LINGYUN.Platform.Menus;
using LINGYUN.Platform.Messages;
using LINGYUN.Platform.ObjectExtending;
using LINGYUN.Platform.Packages;
using LINGYUN.Platform.Routes;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.BlobStoring;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.Emailing;
using Volo.Abp.EventBus;
using Volo.Abp.Mapperly;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending.Modularity;
using Volo.Abp.Sms;
using SmsMessage = LINGYUN.Platform.Messages.SmsMessage;

namespace LINGYUN.Platform;

[DependsOn(
    typeof(AbpSmsModule),
    typeof(AbpEmailingModule),
    typeof(AbpEventBusModule),
    typeof(AbpEventBusModule),
    typeof(AbpMapperlyModule),
    typeof(PlatformDomainSharedModule))]
public class PlatformDomainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMapperlyObjectMapper<PlatformDomainModule>();

        Configure<DataItemMappingOptions>(options =>
        {
            options.SetDefaultMapping();
        });

        Configure<AbpBlobStoringOptions>(options =>
        {
            options.Containers.Configure<PackageContainer>(containerConfiguration =>
            {
                containerConfiguration.IsMultiTenant = true;
            });
        });

        Configure<AbpDistributedEntityEventOptions>(options =>
        {
            options.EtoMappings.Add<Layout, LayoutEto>(typeof(PlatformDomainModule));

            options.EtoMappings.Add<Menu, MenuEto>(typeof(PlatformDomainModule));
            options.EtoMappings.Add<UserMenu, UserMenuEto>(typeof(PlatformDomainModule));
            options.EtoMappings.Add<RoleMenu, RoleMenuEto>(typeof(PlatformDomainModule));

            options.EtoMappings.Add<Package, PackageEto>(typeof(PlatformDomainModule));

            options.EtoMappings.Add<EmailMessage, EmailMessageEto>(typeof(PlatformDomainModule));
            options.EtoMappings.Add<SmsMessage, SmsMessageEto>(typeof(PlatformDomainModule));

            options.AutoEventSelectors.Add<EmailMessage>();
            options.AutoEventSelectors.Add<SmsMessage>();
        });
    }
    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToEntity(
            PlatformModuleExtensionConsts.ModuleName,
            PlatformModuleExtensionConsts.EntityNames.Route,
            typeof(Route)
        );
        ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToEntity(
            PlatformModuleExtensionConsts.ModuleName,
            PlatformModuleExtensionConsts.EntityNames.Package,
            typeof(Package)
        );
    }
}
