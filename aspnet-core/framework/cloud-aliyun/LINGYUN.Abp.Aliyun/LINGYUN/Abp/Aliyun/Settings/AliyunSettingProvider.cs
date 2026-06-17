using LINGYUN.Abp.Aliyun.Features;
using LINGYUN.Abp.Aliyun.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.Aliyun.Settings;

public class AliyunSettingProvider : SettingDefinitionProvider
{
    private const string GroupName = "Aliyun";

    public override void Define(ISettingDefinitionContext context)
    {
        context.Add(GetAuthorizationSettings());
        context.Add(GetSmsSettings());
        context.Add(GetSmsVerifyCodeSettings());
    }

    private SettingDefinition[] GetAuthorizationSettings()
    {
        return new SettingDefinition[]
        {
            new SettingDefinition(
                AliyunSettingNames.Authorization.AccessKeyId,
                displayName: L("DisplayName:AccessKeyId"),
                description: L("Description:AccessKeyId"),
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
            .WithParent("RAM", L("DisplayName:Aliyun.RAM")),
            new SettingDefinition(
                AliyunSettingNames.Authorization.AccessKeySecret,
                displayName: L("DisplayName:AccessKeySecret"),
                description: L("Description:AccessKeySecret"),
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
            .WithParent("RAM", L("DisplayName:Aliyun.RAM")),
            new SettingDefinition(
                AliyunSettingNames.Authorization.DurationSeconds,
                defaultValue: "3600",
                displayName: L("DisplayName:DurationSeconds"),
                description: L("Description:DurationSeconds"),
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
            .WithParent("RAM", L("DisplayName:Aliyun.RAM"))
            .WithValueType(ValueType.Number),
            new SettingDefinition(
                AliyunSettingNames.Authorization.Policy,
                displayName: L("DisplayName:Policy"),
                description: L("Description:Policy"),
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
            .WithParent("RAM", L("DisplayName:Aliyun.RAM")),
            new SettingDefinition(
                AliyunSettingNames.Authorization.RamRoleArn,
                displayName: L("DisplayName:RamRoleArn"),
                description: L("Description:RamRoleArn"),
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
            .WithParent("RAM", L("DisplayName:Aliyun.RAM")),
            new SettingDefinition(
                AliyunSettingNames.Authorization.RegionId,
                defaultValue: "cn-hangzhou",
                displayName: L("DisplayName:RegionId"),
                description: L("Description:RegionId"),
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
            .WithParent("RAM", L("DisplayName:Aliyun.RAM")),
            new SettingDefinition(
                AliyunSettingNames.Authorization.RoleSessionName,
                displayName: L("DisplayName:RoleSessionName"),
                description: L("Description:RoleSessionName"),
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
            .WithParent("RAM", L("DisplayName:Aliyun.RAM")),
            new SettingDefinition(
                AliyunSettingNames.Authorization.UseSecurityTokenService,
                defaultValue: true.ToString(),
                displayName: L("DisplayName:UseSecurityTokenService"),
                description: L("Description:UseSecurityTokenService"),
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
            .WithParent("RAM", L("DisplayName:Aliyun.RAM"))
            .WithValueType(ValueType.Boolean),
        };
    }

    private SettingDefinition[] GetSmsSettings()
    {
        return new SettingDefinition[]
        {
            new SettingDefinition(
                AliyunSettingNames.Sms.ActionName,
                defaultValue: "SendSms",
                displayName: L("DisplayName:ActionName"),
                description: L("Description:ActionName"),
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
                requiredFeatures: [AliyunFeatureNames.Sms.Enable])
            .WithParent("Sms", L("DisplayName:Aliyun.Sms")),
            new SettingDefinition(
                AliyunSettingNames.Sms.DefaultSignName,
                displayName: L("DisplayName:DefaultSignName"),
                description: L("Description:DefaultSignName"),
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
                requiredFeatures: [AliyunFeatureNames.Sms.Enable])
            .WithParent("Sms", L("DisplayName:Aliyun.Sms")),
            new SettingDefinition(
                AliyunSettingNames.Sms.DefaultTemplateCode,
                displayName: L("DisplayName:DefaultTemplateCode"),
                description: L("Description:DefaultTemplateCode"),
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
                requiredFeatures: [AliyunFeatureNames.Sms.Enable])
            .WithParent("Sms", L("DisplayName:Aliyun.Sms")),
            new SettingDefinition(
                AliyunSettingNames.Sms.DefaultPhoneNumber,
                displayName: L("DisplayName:DefaultPhoneNumber"),
                description: L("Description:DefaultPhoneNumber"),
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
                requiredFeatures: [AliyunFeatureNames.Sms.Enable])
            .WithParent("Sms", L("DisplayName:Aliyun.Sms")),
            new SettingDefinition(
                AliyunSettingNames.Sms.Domain,
                defaultValue: "dysmsapi.aliyuncs.com",
                displayName: L("DisplayName:SmsDomain"),
                description: L("Description:SmsDomain"),
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
                requiredFeatures: [AliyunFeatureNames.Sms.Enable])
            .WithParent("Sms", L("DisplayName:Aliyun.Sms")),
            new SettingDefinition(
                AliyunSettingNames.Sms.Version,
                defaultValue: "2017-05-25",
                displayName: L("DisplayName:Version"),
                description: L("Description:Version"),
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
                requiredFeatures: [AliyunFeatureNames.Sms.Enable])
            .WithParent("Sms", L("DisplayName:Aliyun.Sms")),
            new SettingDefinition(
                AliyunSettingNames.Sms.VisableErrorToClient,
                defaultValue: false.ToString(),
                displayName: L("DisplayName:VisableErrorToClient"),
                description: L("Description:VisableErrorToClient"),
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
                requiredFeatures: [AliyunFeatureNames.Sms.Enable])
            .WithParent("Sms", L("DisplayName:Aliyun.Sms"))
            .WithValueType(ValueType.Boolean)
        };
    }

    private SettingDefinition[] GetSmsVerifyCodeSettings()
    {
        return new SettingDefinition[]
        {
            new SettingDefinition(
                AliyunSettingNames.SmsVerifyCode.Domain,
                defaultValue: "dypnsapi.aliyuncs.com",
                displayName: L("DisplayName:SmsVerifyCodeDomain"),
                description: L("Description:SmsVerifyCodeDomain"),
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
                requiredFeatures: [AliyunFeatureNames.Sms.Enable])
            .WithParent("SmsVerifyCode", L("DisplayName:Aliyun.SmsVerifyCode")),
            new SettingDefinition(
                AliyunSettingNames.SmsVerifyCode.DefaultSignName,
                displayName: L("DisplayName:DefaultSignName"),
                description: L("Description:DefaultSignName"),
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
                requiredFeatures: [AliyunFeatureNames.Sms.Enable])
            .WithParent("SmsVerifyCode", L("DisplayName:Aliyun.SmsVerifyCode")),
            new SettingDefinition(
                AliyunSettingNames.SmsVerifyCode.DefaultTemplateCode,
                displayName: L("DisplayName:DefaultTemplateCode"),
                description: L("Description:DefaultTemplateCode"),
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
                requiredFeatures: [AliyunFeatureNames.Sms.Enable])
            .WithParent("SmsVerifyCode", L("DisplayName:Aliyun.SmsVerifyCode")),
        };
    }
    private LocalizableString L(string name)
    {
        return LocalizableString.Create<AliyunResource>(name);
    }
}
