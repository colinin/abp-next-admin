using LINGYUN.Platform.Datas;
using LINGYUN.Platform.ObjectExtending;
using LINGYUN.Platform.Routes;
using LINGYUN.Platform.Versions;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.BlobStoring;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending.Modularity;

namespace LINGYUN.Platform
{
    [DependsOn(
        typeof(PlatformDomainSharedModule),
        typeof(AbpBlobStoringModule),
        typeof(AbpEventBusModule))]
    public class PlatformDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<PlatformDomainModule>();

            Configure<DataItemMappingOptions>(options =>
            {
                options.SetDefaultMapping();
            });

            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<PlatformDomainMappingProfile>(validate: true);
            });

            Configure<AbpBlobStoringOptions>(options =>
            {
                options.Containers.Configure<VersionContainer>(containerConfiguration =>
                {
                    containerConfiguration.IsMultiTenant = true;
                });
            });

            Configure<AbpDistributedEntityEventOptions>(options =>
            {
                options.EtoMappings.Add<Route, RouteEto>(typeof(PlatformDomainModule));

                options.EtoMappings.Add<AppVersion, AppVersionEto>(typeof(PlatformDomainModule));

                options.AutoEventSelectors.Add<AppVersion>();
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
                PlatformModuleExtensionConsts.EntityNames.AppVersion,
                typeof(AppVersion)
            );
        }
    }
}
