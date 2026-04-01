using LINGYUN.Abp.AI.Models;
using LINGYUN.Abp.AI.Workspaces;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenAI;
using System;
using System.ClientModel;
using System.Threading.Tasks;
using Volo.Abp;

namespace LINGYUN.Abp.AI.Internal;
public class OpenAIChatClientProvider : ChatClientProvider
{
    protected virtual string DefaultEndpoint => "https://api.openai.com/v1";

    public const string ProviderName = "OpenAI";
    public override string Name => ProviderName;
    public OpenAIChatClientProvider(IServiceProvider serviceProvider) 
        : base(serviceProvider)
    {
    }

    public override ChatModel[] GetModels()
    {
        return [
            new ChatModel("gpt-4.1", "GPT-4.1", "Smartest non-reasoning model"),
            new ChatModel("gpt-5", "GPT-5", "Previous intelligent reasoning model for coding and agentic tasks with configurable reasoning effort"),
            new ChatModel("gpt-5-nano", "GPT-5 nano", "Fastest, most cost-efficient version of GPT-5"),
            new ChatModel("gpt-5-mini", "GPT-5 mini", "Near-frontier intelligence for cost sensitive, low latency, high volume workloads"),
            new ChatModel("gpt-5.4", "GPT-5.4", "Best intelligence at scale for agentic, coding, and professional workflows"),
            new ChatModel("gpt-5.4-pro", "GPT-5.4 pro", "Version of GPT-5.4 that produces smarter and more precise responses."),
        ];
    }

    public override async Task<IChatClient> CreateAsync(WorkspaceDefinition workspace)
    {
        Check.NotNull(workspace, nameof(workspace));
        Check.NotNullOrWhiteSpace(workspace.ApiKey, nameof(WorkspaceDefinition.ApiKey));

        var openAIClient = new OpenAIClient(
            new ApiKeyCredential(workspace.ApiKey),
            new OpenAIClientOptions
            {
                Endpoint = new Uri(workspace.ApiBaseUrl ?? DefaultEndpoint),
            });

        var options = ServiceProvider.GetRequiredService<IOptions<AbpAICoreOptions>>().Value;

        var chatClientBuilder = openAIClient
            .GetChatClient(workspace.ModelName)
            .AsIChatClient()
            .AsBuilder();

        foreach (var handlerAction in options.ChatClientBuildActions)
        {
            chatClientBuilder = await handlerAction(workspace, ServiceProvider, chatClientBuilder);
        }

        return chatClientBuilder.Build(ServiceProvider);
    }
}
