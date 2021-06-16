using LINGYUN.Abp.Hangfire.Dashboard.Localization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Authorization;
using Volo.Abp.Hangfire;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.Hangfire.Dashboard
{
    [DependsOn(
        typeof(AbpLocalizationModule),
        typeof(AbpAuthorizationModule),
        typeof(AbpHangfireModule))]
    public class AbpHangfireDashboardModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpHangfireDashboardModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources.Add<HangfireDashboardResource>();
            });

            context.Services.AddTransient(serviceProvider =>
            {
                var options = serviceProvider.GetRequiredService<AbpHangfireDashboardOptionsProvider>().Get();
                return context.Services.ExecutePreConfiguredActions(options);
            });
        }
    }
}
