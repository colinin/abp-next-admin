using LINGYUN.Abp.Idempotent.Localization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Json;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.Idempotent;

[DependsOn(
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpDistributedLockingAbstractionsModule),
    typeof(AbpJsonAbstractionsModule))]
public class AbpIdempotentModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.OnRegistered(IdempotentInterceptorRegistrar.RegisterIfNeeded);
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpIdempotentModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources.Add<IdempotentResource>()
                .AddVirtualJson("/LINGYUN/Abp/Idempotent/Localization/Resources");
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace(IdempotentErrorCodes.Namespace, typeof(IdempotentResource));
        });
    }
}
