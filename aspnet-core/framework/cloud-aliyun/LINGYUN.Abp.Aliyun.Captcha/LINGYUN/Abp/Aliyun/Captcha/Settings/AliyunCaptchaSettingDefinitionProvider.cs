using LINGYUN.Abp.Aliyun.Features;
using LINGYUN.Abp.Aliyun.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.Aliyun.Captcha.Settings;

public class AliyunCaptchaSettingDefinitionProvider : SettingDefinitionProvider
{
    private const string GroupName = "Aliyun";

    public override void Define(ISettingDefinitionContext context)
    {
        context.Add(
            new SettingDefinition(
                AliyunCaptchaSettingNames.SceneId,
                displayName: L("DisplayName:SceneId"),
                description: L("Description:SceneId"),
                isVisibleToClients: false,
                isEncrypted: true
            )
            .WithProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName)
            .WithGroup(
                GroupName, 
                L("DisplayName:Aliyun"),
                requiredFeatures: [AliyunFeatureNames.Enable])
            .WithParent("Captcha", L("DisplayName:Aliyun.Captcha"), 10)
            .WithOrder(1),
            new SettingDefinition(
                AliyunCaptchaSettingNames.Prefix,
                displayName: L("DisplayName:Prefix"),
                description: L("Description:Prefix"),
                isVisibleToClients: false,
                isEncrypted: true
            )
            .WithProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName)
            .WithGroup(
                GroupName,
                L("DisplayName:Aliyun"),
                requiredFeatures: [AliyunFeatureNames.Enable])
            .WithParent("Captcha", L("DisplayName:Aliyun.Captcha"), 10)
            .WithOrder(2),
            new SettingDefinition(
                AliyunCaptchaSettingNames.UseEncryptedSceneId,
                defaultValue: true.ToString(),
                displayName: L("DisplayName:UseEncryptedSceneId"),
                description: L("Description:UseEncryptedSceneId"),
                isVisibleToClients: false
            )
            .WithProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName)
            .WithGroup(
                GroupName,
                L("DisplayName:Aliyun"),
                requiredFeatures: [AliyunFeatureNames.Enable])
            .WithParent("Captcha", L("DisplayName:Aliyun.Captcha"), 10)
            .WithOrder(3)
            .WithValueType(ValueType.Boolean),
            new SettingDefinition(
                AliyunCaptchaSettingNames.EKey,
                displayName: L("DisplayName:EKey"),
                description: L("Description:EKey"),
                isVisibleToClients: false,
                isEncrypted: true
            )
            .WithProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName)
            .WithGroup(
                GroupName,
                L("DisplayName:Aliyun"),
                requiredFeatures: [AliyunFeatureNames.Enable])
            .WithParent("Captcha", L("DisplayName:Aliyun.Captcha"), 10)
            .WithOrder(4),
            new SettingDefinition(
                AliyunCaptchaSettingNames.EncryptedExpireTimeSec,
                defaultValue: 300.ToString(),
                displayName: L("DisplayName:EncryptedExpireTimeSec"),
                description: L("Description:EncryptedExpireTimeSec"),
                isVisibleToClients: false
            )
            .WithProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName)
            .WithGroup(
                GroupName,
                L("DisplayName:Aliyun"),
                requiredFeatures: [AliyunFeatureNames.Enable])
            .WithParent("Captcha", L("DisplayName:Aliyun.Captcha"), 10)
            .WithOrder(5)
            .WithValueType(ValueType.Number));
    }
    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AliyunResource>(name);
    }
}
