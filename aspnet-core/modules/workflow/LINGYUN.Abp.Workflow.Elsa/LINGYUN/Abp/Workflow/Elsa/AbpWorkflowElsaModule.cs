using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Json.Newtonsoft;
using Volo.Abp.Modularity;
using ElsaOptionsBuilder = Elsa.Options.ElsaOptionsBuilder;

namespace LINGYUN.Abp.Workflow.Elsa;

public class AbpWorkflowElsaModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var builder = context.Services.GetPreConfigureActions<ElsaOptionsBuilder>();

        context.Services.AddElsa(options =>
        {
            options.AddCustomTenantAccessor<AbpTenantAccessor>();
            options.AddConsoleActivities();
            options.UseJsonSerializer((provider) =>
            {
                var jsonOptions = provider.GetRequiredService<IOptions<AbpNewtonsoftJsonSerializerOptions>>();
                var jsonConverters = jsonOptions.Value.Converters
                    .Select(c => (JsonConverter)provider.GetRequiredService(c))
                    .ToList();
                var jsonSettings = new JsonSerializerSettings();
                jsonSettings.Converters.InsertRange(0, jsonConverters);

                return JsonSerializer.Create(jsonSettings);
            });

            builder.Configure(options);
        });
    }
}
