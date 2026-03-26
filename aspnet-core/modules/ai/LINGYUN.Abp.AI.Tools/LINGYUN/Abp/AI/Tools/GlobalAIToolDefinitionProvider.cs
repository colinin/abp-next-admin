using LINGYUN.Abp.AI.Localization;
using LINGYUN.Abp.AI.Tools.Settings;
using LINGYUN.Abp.AI.Tools.Timing;
using LINGYUN.Abp.AI.Tools.Users;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.AI.Tools;
public class GlobalAIToolDefinitionProvider : AIToolDefinitionProvider
{
    public override void Define(IAIToolDefinitionContext context)
    {
        context.Add(
            new AIToolDefinition(
                NowTimeTool.Name,
                FunctionAIToolProvider.ProviderName,
                L("Tools:GetNowTime"))
            .WithFunction<NowTimeTool>(),

            new AIToolDefinition(
                CurrentUserTool.Name,
                FunctionAIToolProvider.ProviderName,
                L("Tools:GetUserInfo"))
            .WithFunction<CurrentUserTool>(),

            new AIToolDefinition(
                GetSettingTool.Name,
                FunctionAIToolProvider.ProviderName,
                L("Tools:GetSetting"))
            .WithFunction<GetSettingTool>(),
            new AIToolDefinition(
                GetSettingsTool.Name,
                FunctionAIToolProvider.ProviderName,
                L("Tools:GetSettings"))
            .WithFunction<GetSettingsTool>());
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AbpAIResource>(name);
    }
}
