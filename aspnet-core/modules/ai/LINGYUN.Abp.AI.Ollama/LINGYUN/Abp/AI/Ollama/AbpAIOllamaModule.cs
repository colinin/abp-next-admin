using Volo.Abp.Modularity;

namespace LINGYUN.Abp.AI.Ollama;

[DependsOn(typeof(AbpAICoreModule))]
public class AbpAIOllamaModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAICoreOptions>(options =>
        {
            options.ChatClientProviders.Add<OllamaChatClientProvider>();

            options.KernelProviders.Add<OllamaKernelProvider>();
        });
    }
}
