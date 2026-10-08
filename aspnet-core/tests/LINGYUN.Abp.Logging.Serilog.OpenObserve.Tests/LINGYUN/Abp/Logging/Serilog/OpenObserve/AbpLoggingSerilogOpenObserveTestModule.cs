using LINGYUN.Abp.Tests;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Logging.Serilog.OpenObserve;

[DependsOn(
    typeof(AbpTestsBaseModule),
    typeof(AbpLoggingTestModule),
    typeof(AbpLoggingSerilogOpenObserveModule))]
public class AbpLoggingSerilogOpenObserveTestModule : AbpModule
{
    private const string UserSecretsId = "F8F32996-613A-4C26-B604-3BD048B18A39";

    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.ReplaceConfiguration(ConfigurationHelper.BuildConfiguration(builderAction: builder =>
        {
            builder.AddUserSecrets(UserSecretsId);
        }));
    }
}
