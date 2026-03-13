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
            new ChatModel("deepseek-chat", "DeepSeek-V3", "DeepSeek-Chat是全能高效的“快枪手”，擅长日常对话与通用任务"),
            new ChatModel("deepseek-reasoner", "DeepSeek-R1", "DeepSeek-Reasoner是深思熟虑的“解题家”，专攻复杂推理与逻辑难题"),
        ];
    }
}
