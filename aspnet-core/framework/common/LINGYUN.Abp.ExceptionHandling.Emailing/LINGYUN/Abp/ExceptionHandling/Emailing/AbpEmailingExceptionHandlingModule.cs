using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Emailing;
using Volo.Abp.ExceptionHandling.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.ExceptionHandling.Emailing
{
    [DependsOn(
        typeof(AbpExceptionHandlingModule),
        typeof(AbpEmailingModule))]
    public class AbpEmailingExceptionHandlingModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            Configure<AbpEmailExceptionHandlingOptions>(
                configuration.GetSection("ExceptionHandling:Emailing"));

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpEmailingExceptionHandlingModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<AbpExceptionHandlingResource>()
                    .AddVirtualJson("/LINGYUN/Abp/ExceptionHandling/Emailing/Localization/Resources");
            });
        }
    }
}
