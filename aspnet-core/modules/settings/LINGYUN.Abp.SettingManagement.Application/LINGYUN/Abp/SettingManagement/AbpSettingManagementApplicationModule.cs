using LINGYUN.Abp.Account;
using LINGYUN.Abp.Account.OAuth;
using LINGYUN.Abp.Account.OAuth.Localization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement;
using Volo.Abp.SettingManagement.Localization;
using Volo.Abp.VirtualFileSystem;
using VoloAbpSettingManagementApplicationContractsModule = Volo.Abp.SettingManagement.AbpSettingManagementApplicationContractsModule;

namespace LINGYUN.Abp.SettingManagement;

[DependsOn(
    typeof(AbpSettingManagementDomainModule),
    typeof(AbpSettingManagementApplicationContractsModule),
    typeof(VoloAbpSettingManagementApplicationContractsModule),
    typeof(AbpAccountApplicationContractsModule),
    typeof(AbpAccountOAuthModule),
    typeof(AbpDddApplicationModule)
    )]
public class AbpSettingManagementApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddScoped<ISettingTestAppService, SettingAppService>();

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpSettingManagementApplicationModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources.Get<AbpSettingManagementResource>()
                .AddVirtualJson("/LINGYUN/Abp/SettingManagement/Localization/Resources")
                .AddBaseTypes(typeof(AccountOAuthResource));
        });
    }
}
