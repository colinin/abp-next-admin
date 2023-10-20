using LINGYUN.Abp.Location.Tencent.Localization;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using System;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.Location.Tencent
{
    [DependsOn(
        typeof(AbpLocationModule),
        typeof(AbpThreadingModule))]
    public class AbpTencentLocationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            Configure<TencentLocationOptions>(configuration.GetSection("Location:Tencent"));

            context.Services.AddHttpClient(TencentLocationHttpConsts.HttpClientName)
               .AddTransientHttpErrorPolicy(builder =>
                   builder.WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(Math.Pow(2, i))));
            
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpTencentLocationModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                       .Add<TencentLocationResource>("en")
                       .AddVirtualJson("/LINGYUN/Abp/Location/Tencent/Localization/Resources");
            });
        }
    }
}
