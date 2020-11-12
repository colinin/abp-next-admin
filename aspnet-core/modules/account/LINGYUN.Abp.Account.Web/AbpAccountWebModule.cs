using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Emailing;
using Volo.Abp.Modularity;
using Volo.Abp.Sms;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.Account.Web
{
    [DependsOn(
        typeof(AbpSmsModule),
        typeof(AbpEmailingModule),
        typeof(Volo.Abp.Account.Web.AbpAccountWebModule))]
    public class AbpAccountWebModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpAccountWebModule>();
            });

            context.Services
               .AddAuthorization(options =>
               {
                   options
                    .AddPolicy("TwoFactorEnabled", policy =>
                    {
                        policy.RequireClaim("amr", "mfa");
                    });
               });
        }
    }
}
