using LINGYUN.Abp.AI.Models;
using LINGYUN.Abp.AI.Workspaces;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OllamaSharp;
using System;
using System.Threading.Tasks;
using Volo.Abp;

namespace LINGYUN.Abp.AI.Ollama;
public class OllamaChatClientProvider : ChatClientProvider
{
    public const string DefaultEndpoint = "http://localhost:11434";

    public const string ProviderName = "Ollama";
    public override string Name => ProviderName;

    public OllamaChatClientProvider(
        IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public async override Task<IChatClient> CreateAsync(WorkspaceDefinition workspace)
    {
        Check.NotNull(workspace, nameof(workspace));
        Check.NotNullOrWhiteSpace(workspace.ModelName, nameof(WorkspaceDefinition.ModelName));

        var options = ServiceProvider.GetRequiredService<IOptions<AbpAICoreOptions>>().Value;

        var ollamaApiClient = new OllamaApiClient(workspace.ApiBaseUrl ?? DefaultEndpoint, workspace.ModelName);

        var chatClientBuilder = ChatClientBuilderChatClientExtensions.AsBuilder(ollamaApiClient);

        foreach (var handlerAction in options.ChatClientBuildActions)
        {
            chatClientBuilder = await handlerAction(workspace, ServiceProvider, chatClientBuilder);
        }

        return chatClientBuilder.Build(ServiceProvider);
    }

    public override ChatModel[] GetModels()
    {
        return [];
    }
}
