using LINGYUN.Abp.Tests;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Elasticsearch;

[DependsOn(
    typeof(AbpTestsBaseModule),
    typeof(AbpElasticsearchModule))]
public class AbpElasticsearchTestModule : AbpModule
{
    private const string UserSecretsId = "D4327320-718E-4A7F-A987-85838EDD8675";

    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.ReplaceConfiguration(ConfigurationHelper.BuildConfiguration(builderAction: builder =>
        {
            builder.AddUserSecrets(UserSecretsId);
        }));
    }
}
