using LINGYUN.Abp.AI.Workspaces;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using OllamaSharp;
using System;
using System.Threading.Tasks;
using Volo.Abp;

namespace LINGYUN.Abp.AI.Ollama;
public class OllamaKernelProvider : KernelProvider
{
    public const string ProviderName = "Ollama";
    public override string Name => ProviderName;

    public OllamaKernelProvider(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public override Task<Kernel> CreateAsync(WorkspaceDefinition workspace)
    {
        Check.NotNull(workspace, nameof(workspace));

        var options = ServiceProvider.GetRequiredService<IOptions<AbpAICoreOptions>>().Value;

        var ollamaApiClient = new OllamaApiClient(workspace.ApiBaseUrl ?? OllamaChatClientProvider.DefaultEndpoint);

        var kernelBuilder = Kernel.CreateBuilder()
            .AddOllamaChatClient(ollamaApiClient);

        foreach (var handlerAction in options.KernelBuildActions)
        {
            handlerAction(workspace, kernelBuilder);
        }

        return Task.FromResult(kernelBuilder.Build());
    }
}
