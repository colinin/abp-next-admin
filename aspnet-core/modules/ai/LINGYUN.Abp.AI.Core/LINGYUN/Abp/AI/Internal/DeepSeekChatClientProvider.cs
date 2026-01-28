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
}
