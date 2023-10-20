using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using Volo.Abp.Json;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Sonatype.Nexus;

[DependsOn(
    typeof(AbpJsonModule))]
public class AbpSonatypeNexusModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        Configure<AbpSonatypeNexusOptions>(configuration.GetSection("Sonatype:Nexus"));

        context.Services.AddHttpClient(
            SonatypeNexusConsts.ApiClient,
            (serviceProvider, client) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<AbpSonatypeNexusOptions>>().Value;
                client.BaseAddress = new Uri(options.BaseUrl);
                client.DefaultRequestHeaders.Add("X-Nexus-Ui", "true");
                client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
            });
    }
}
