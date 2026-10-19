using LINGYUN.Abp.AuditLogging.OpenObserve.Localization;
using LINGYUN.Abp.OpenObserve;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Json;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.AuditLogging.OpenObserve;

/// <summary>
/// 审计日志 OpenObserve 模块
/// </summary>
/// <remarks>
/// 模块不对数据流做任何处理(不探测、不创建、不修改设置):
/// OpenObserve 会在首次写入时自动创建数据流, 保留策略等设置请在 OpenObserve 侧配置
/// </remarks>
[DependsOn(
    typeof(AbpAuditLoggingModule),
    typeof(AbpOpenObserveModule),
    typeof(AbpJsonModule),
    typeof(AbpLocalizationModule))]
public class AbpAuditLoggingOpenObserveModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        Configure<AbpAuditLoggingOpenObserveOptions>(configuration.GetSection("AuditLogging:OpenObserve"));

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<AuditLoggingOpenObserveResource>()
                .AddVirtualJson("/LINGYUN/Abp/AuditLogging/OpenObserve/Localization/Resources");
        });

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpAuditLoggingOpenObserveModule>();
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace(AuditLoggingOpenObserveErrorCodes.Namespace, typeof(AuditLoggingOpenObserveResource));
        });
    }
}
