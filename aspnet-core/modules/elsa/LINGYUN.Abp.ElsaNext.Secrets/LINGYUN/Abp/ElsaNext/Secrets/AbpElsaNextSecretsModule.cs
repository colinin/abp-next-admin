using Elsa.Extensions;
using Elsa.Features.Services;
using Microsoft.Extensions.Configuration;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.ElsaNext.Secrets;

[DependsOn(
    typeof(AbpElsaNextModule))]
public class AbpElsaNextSecretsModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Configuration.GetSection("Elsa");
        PreConfigure<IModule>(elsa =>
        {
            elsa.UseSecrets(secret =>
            {
                secret.ConfigureOptions = options => configuration.GetSection("Secrets").Bind(options);
            });
            elsa.UseSecretsJavaScript();
        });
    }
}
