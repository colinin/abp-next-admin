using LINGYUN.Abp.AI.Workspaces;
using Microsoft.SemanticKernel;
using OpenAI;
using System;
using System.ClientModel;
using System.Threading.Tasks;
using Volo.Abp;

namespace LINGYUN.Abp.AI;
public class OpenAIKernelProvider : KernelProvider
{
    private const string DefaultEndpoint = "https://api.openai.com/v1";

    public const string ProviderName = "OpenAI";
    public override string Name => ProviderName;
    public OpenAIKernelProvider(IServiceProvider serviceProvider) 
        : base(serviceProvider)
    {
    }

    public override Task<Kernel> CreateAsync(WorkspaceDefinition workspace)
    {
        Check.NotNull(workspace, nameof(workspace));
        Check.NotNullOrWhiteSpace(workspace.ApiKey, nameof(WorkspaceDefinition.ApiKey));

        var openAIClient = new OpenAIClient(
            new ApiKeyCredential(workspace.ApiKey),
            new OpenAIClientOptions
            {
                Endpoint = new Uri(workspace.ApiBaseUrl ?? DefaultEndpoint),
            });

        var kernel = Kernel.CreateBuilder()
            .AddOpenAIChatClient(workspace.ModelName, openAIClient)
            .Build();

        return Task.FromResult(kernel);
    }
}
