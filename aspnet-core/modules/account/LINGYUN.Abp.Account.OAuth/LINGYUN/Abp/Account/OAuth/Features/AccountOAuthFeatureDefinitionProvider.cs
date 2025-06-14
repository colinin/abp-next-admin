using LINGYUN.Abp.Account.OAuth.Localization;
using Volo.Abp.Features;
using Volo.Abp.Localization;
using Volo.Abp.Validation.StringValues;

namespace LINGYUN.Abp.Account.OAuth.Features;

public class AccountOAuthFeatureDefinitionProvider : FeatureDefinitionProvider
{
    public override void Define(IFeatureDefinitionContext context)
    {
        var group = context.AddGroup(
              name: AccountOAuthFeatureNames.GroupName,
              displayName: L("Features:ExternalOAuthLogin"));

        group.AddFeature(
            name: AccountOAuthFeatureNames.GitHub.Enable,
            defaultValue: "false",
            displayName: L("Features:GithubOAuthEnable"),
            description: L("Features:GithubOAuthEnableDesc"),
            valueType: new ToggleStringValueType(new BooleanValueValidator()));
        group.AddFeature(
            name: AccountOAuthFeatureNames.QQ.Enable,
            defaultValue: "false",
            displayName: L("Features:QQOAuthEnable"),
            description: L("Features:QQOAuthEnableDesc"),
            valueType: new ToggleStringValueType(new BooleanValueValidator()));
        group.AddFeature(
            name: AccountOAuthFeatureNames.WeChat.Enable,
            defaultValue: "false",
            displayName: L("Features:WeChatOAuthEnable"),
            description: L("Features:WeChatOAuthEnableDesc"),
            valueType: new ToggleStringValueType(new BooleanValueValidator()));
        group.AddFeature(
            name: AccountOAuthFeatureNames.WeCom.Enable,
            defaultValue: "false",
            displayName: L("Features:WeComOAuthEnable"),
            description: L("Features:WeComOAuthEnableDesc"),
            valueType: new ToggleStringValueType(new BooleanValueValidator()));
        group.AddFeature(
            name: AccountOAuthFeatureNames.Bilibili.Enable,
            defaultValue: "false",
            displayName: L("Features:BilibiliOAuthEnable"),
            description: L("Features:BilibiliOAuthEnableDesc"),
            valueType: new ToggleStringValueType(new BooleanValueValidator()));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AccountOAuthResource>(name);
    }
}
