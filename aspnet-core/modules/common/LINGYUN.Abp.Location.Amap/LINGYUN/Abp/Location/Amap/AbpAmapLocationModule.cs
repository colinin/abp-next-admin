using LINYUN.Abp.Location.Amap.Localization;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using System;
using Volo.Abp.Json;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.Location.Amap
{
    [DependsOn(
        typeof(AbpLocationModule),
        typeof(AbpJsonModule),
        typeof(AbpThreadingModule),
        typeof(AbpLocalizationModule))]
    public class AbpAmapLocationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            Configure<AmapLocationOptions>(configuration.GetSection("Location:Amap"));

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpAmapLocationModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                       .Add<AmapLocationResource>("en")
                       .AddVirtualJson("/LINGYUN/Abp/Location/Amap/Localization/Resources");
            });

            context.Services.AddHttpClient(AmapHttpConsts.HttpClientName)
                .AddTransientHttpErrorPolicy(builder =>
                    builder.WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(Math.Pow(2, i))));
        }
    }
}
