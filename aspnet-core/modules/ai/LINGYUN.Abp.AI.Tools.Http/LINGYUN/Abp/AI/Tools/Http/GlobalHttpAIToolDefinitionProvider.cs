using LINGYUN.Abp.AI.Localization;
using LINGYUN.Abp.AI.Tools.Http.ApplicationConfigurations;
using Microsoft.Extensions.Options;
using System.Net.Http;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.AI.Tools.Http;
public class GlobalHttpAIToolDefinitionProvider : AIToolDefinitionProvider
{
    protected AbpAIToolsApplicationConfigurationOptions Options { get; }
    public GlobalHttpAIToolDefinitionProvider(IOptions<AbpAIToolsApplicationConfigurationOptions> options)
    {
        Options = options.Value;
    }

    public override void Define(IAIToolDefinitionContext context)
    {
        if (Options.IsEnabled)
        {
            context.Add(
                new AIToolDefinition(
                    "GetApplicationConfiguration",
                    HttpAIToolProvider.ProviderName,
                    L("Tools:GetApplicationConfiguration"))
                .WithHttpEndpoint(Options.EndPoint)
                .WithHttpMethod(HttpMethod.Get)
                .WithHttpHeaders("_AbpDontWrapResult", "true")// 无需包装结果
                .UseHttpCurrentAccessToken());
        }
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AbpAIResource>(name);
    }
}
