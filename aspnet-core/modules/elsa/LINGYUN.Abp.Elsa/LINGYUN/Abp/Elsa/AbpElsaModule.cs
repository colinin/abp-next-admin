using Elsa;
using Elsa.Services;
using LINGYUN.Abp.Elsa.Localization;
using LINGYUN.Abp.Elsa.Scripting.JavaScript;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Features;
using Volo.Abp.Json;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;
using ElsaOptionsBuilder = Elsa.Options.ElsaOptionsBuilder;

namespace LINGYUN.Abp.Elsa;

[DependsOn(
    typeof(AbpFeaturesModule),
    typeof(AbpThreadingModule),
    typeof(AbpJsonModule))]
public class AbpElsaModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var builder = context.Services.GetPreConfigureActions<ElsaOptionsBuilder>();

        context.Services
            .AddElsa(options =>
            {
                options.AddCustomTenantAccessor<AbpTenantAccessor>();
                options.AddConsoleActivities();
                options.AddJavaScriptActivities();
                //options.UseJsonSerializer((provider) =>
                //{
                //    var jsonOptions = provider.GetRequiredService<IOptions<AbpNewtonsoftJsonSerializerOptions>>();
                //    var jsonConverters = jsonOptions.Value.Converters
                //        .Select(c => (JsonConverter)provider.GetRequiredService(c))
                //        .ToList();
                //    var jsonSettings = new JsonSerializerSettings();
                //    jsonSettings.Converters.InsertRange(0, jsonConverters);

                //    return JsonSerializer.Create(jsonSettings);
                //});

                builder.Configure(options);
            })
            .AddNotificationHandlers(typeof(ConfigureJavaScriptEngine))
            .Replace<IIdGenerator, AbpElsaIdGenerator>(ServiceLifetime.Singleton);

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources.Add<ElsaResource>();
        });
    }
}
