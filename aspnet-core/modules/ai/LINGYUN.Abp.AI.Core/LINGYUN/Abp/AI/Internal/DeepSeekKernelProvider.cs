using System;

namespace LINGYUN.Abp.AI.Internal;
public class DeepSeekKernelProvider : OpenAIKernelProvider
{
    protected override string DefaultEndpoint => "https://api.deepseek.com/v1";

    public new const string ProviderName = "DeepSeek";
    public override string Name => ProviderName;
    public DeepSeekKernelProvider(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
}
