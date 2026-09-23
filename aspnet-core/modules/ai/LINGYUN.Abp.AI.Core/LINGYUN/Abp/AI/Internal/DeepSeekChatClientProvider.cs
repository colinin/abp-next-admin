using LINGYUN.Abp.AI.Models;
using System;

namespace LINGYUN.Abp.AI.Internal;
public class DeepSeekChatClientProvider : OpenAIChatClientProvider
{
    protected override string DefaultEndpoint => "https://api.deepseek.com/v1";

    public new const string ProviderName = "DeepSeek";
    public override string Name => ProviderName;
    public DeepSeekChatClientProvider(
        IServiceProvider serviceProvider) 
        : base(serviceProvider)
    {
    }

    public override ChatModel[] GetModels()
    {
        return [
            new ChatModel("deepseek-v4-flash", "deepseek-v4-flash"),
            new ChatModel("deepseek-v4-pro", "deepseek-v4-pro"),
            new ChatModel("deepseek-v4-flash-vision-exp", "deepseek-v4-flash-vision-exp"),
        ];
    }
}
