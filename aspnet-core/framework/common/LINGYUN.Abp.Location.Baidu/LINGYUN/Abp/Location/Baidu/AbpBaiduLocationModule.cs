using LINGYUN.Abp.Location.Baidu.Localization;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using System;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.Location.Baidu
{
    [DependsOn(
        typeof(AbpLocationModule),
        typeof(AbpThreadingModule))]
    public class AbpBaiduLocationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            Configure<BaiduLocationOptions>(configuration.GetSection("Location:Baidu"));

            context.Services.AddHttpClient(BaiduLocationHttpConsts.HttpClientName)
                .AddTransientHttpErrorPolicy(builder =>
                    builder.WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(Math.Pow(2, i))));

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpBaiduLocationModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                       .Add<BaiduLocationResource>("en")
                       .AddVirtualJson("/LINGYUN/Abp/Location/Baidu/Localization/Resources");
            });
        }
    }
}
