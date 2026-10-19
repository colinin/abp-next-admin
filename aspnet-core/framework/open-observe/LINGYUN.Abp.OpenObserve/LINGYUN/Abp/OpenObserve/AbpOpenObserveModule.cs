using LINGYUN.Abp.OpenObserve.Serialization.Modifiers;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using Volo.Abp.Json;
using Volo.Abp.Json.SystemTextJson;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.OpenObserve;

[DependsOn(typeof(AbpJsonModule))]
public class AbpOpenObserveModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        Configure<AbpOpenObserveOptions>(configuration.GetSection("OpenObserve"));

        Configure<AbpSystemTextJsonSerializerModifiersOptions>(options =>
        {
            options.Modifiers.Add(OpenObserveIgnoreNullPropertiesModifier.Modify);
        });

        context.Services.AddOpenObserveClient();
    }
}
