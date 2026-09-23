using Lazy.Captcha.Core.Storage;
using LINGYUN.Abp.Captcha;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Caching;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.LazyCaptcha;

[DependsOn(
    typeof(AbpCaptchaAbstractionsModule),
    typeof(AbpCachingModule),
    typeof(AbpMultiTenancyAbstractionsModule),
    typeof(AbpSettingsModule))]
public class AbpLazyCaptchaModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddCaptcha(context.Configuration);

        context.Services.Replace(
            ServiceDescriptor.Scoped<IStorage, AbpCaptchaStorage>());

        context.Services.AddScoped<ICodeCaptchaProvider, LazyCodeCaptchaProvider>();
    }
}
