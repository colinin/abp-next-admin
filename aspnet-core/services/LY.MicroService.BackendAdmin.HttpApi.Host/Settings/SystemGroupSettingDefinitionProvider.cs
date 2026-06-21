using LINGYUN.Abp.SettingManagement;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Emailing;
using Volo.Abp.Localization;
using Volo.Abp.SettingManagement;
using Volo.Abp.SettingManagement.Localization;
using Volo.Abp.Settings;
using Volo.Abp.Timing;
using ValueType = Volo.Abp.Settings.ValueType;

namespace LY.MicroService.BackendAdmin.Settings;

public class SystemGroupSettingDefinitionProvider : SettingDefinitionProvider
{
    private const string GroupName = "System";

    protected ITimezoneProvider TimezoneProvider { get; }
    protected AbpLocalizationOptions LocalizationOptions { get; }
    protected IStringLocalizerFactory StringLocalizerFactory { get; }

    public SystemGroupSettingDefinitionProvider(
        ITimezoneProvider timezoneProvider,
        IStringLocalizerFactory stringLocalizerFactory,
        IOptions<AbpLocalizationOptions> localizationOptions)
    {
        TimezoneProvider = timezoneProvider;
        StringLocalizerFactory = stringLocalizerFactory;
        LocalizationOptions = localizationOptions.Value;
    }

    public override void Define(ISettingDefinitionContext context)
    {
        SetSystemSettingGroup(context);
        SetEmailSettingGroup(context);
    }

    private void SetSystemSettingGroup(ISettingDefinitionContext context)
    {
        context.GetOrNull(LocalizationSettingNames.DefaultLanguage)
            ?.WithGroup(GroupName, L("Settings:System"), order: 0)
            ?.WithParent("Language", L("Settings:System.Language"), order: 0)
            ?.WithOrder(0)
            ?.WithOptions(LocalizationOptions.Languages.Select(l => new NameValue<string>(l.DisplayName, l.CultureName)))
            ?.ReplaceProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName,
                UserSettingValueProvider.ProviderName);

        var timezoneSetting = context.GetOrNull(TimingSettingNames.TimeZone);
        if (timezoneSetting != null)
        {
            var stringLocalizer = StringLocalizerFactory.Create<AbpSettingManagementResource>();
            var timezones = TimeZoneHelper.GetTimezones(TimezoneProvider.GetIanaTimezones());
            timezones.Insert(0, new NameValue
            {
                Name = stringLocalizer["DefaultTimeZone"],
                Value = "Unspecified"
            });

            timezoneSetting
                .WithGroup(
                    GroupName,
                    L("Settings:System"),
                    order: 0,
                    requiredPermissions: [Volo.Abp.SettingManagement.SettingManagementPermissions.TimeZone])
                .WithParent("Timing", L("Settings:System.Timing"), order: 0)
                .WithOrder(1)
                .WithOptions(timezones)
                .ReplaceProviders(
                    DefaultValueSettingValueProvider.ProviderName,
                    ConfigurationSettingValueProvider.ProviderName,
                    GlobalSettingValueProvider.ProviderName,
                    TenantSettingValueProvider.ProviderName,
                    UserSettingValueProvider.ProviderName);
        }
    }

    private static void SetEmailSettingGroup(ISettingDefinitionContext context)
    {
        context.GetOrNull(EmailSettingNames.DefaultFromAddress)
            ?.WithGroup(
                "Emailing",
                L("Settings:Emailing"),
                order: 1,
                requiredFeatures: [SettingManagementFeatures.Enable, SettingManagementFeatures.AllowChangingEmailSettings],
                requiredPermissions: [Volo.Abp.SettingManagement.SettingManagementPermissions.Emailing])
            ?.WithParent(
                "Default",
                L("Settings:Emailing.Default"),
                order: 0)
            ?.WithOrder(0)
            ?.WithValueType(ValueType.String)
            ?.ReplaceProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName);
        context.GetOrNull(EmailSettingNames.DefaultFromDisplayName)
            ?.WithGroup(
                "Emailing",
                L("Settings:Emailing"),
                order: 1,
                requiredFeatures: [SettingManagementFeatures.Enable, SettingManagementFeatures.AllowChangingEmailSettings],
                requiredPermissions: [Volo.Abp.SettingManagement.SettingManagementPermissions.Emailing])
            ?.WithParent(
                "Default",
                L("Settings:Emailing.Default"),
                order: 0)
            ?.WithOrder(1)
            ?.WithValueType(ValueType.String)
            ?.ReplaceProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName);

        context.GetOrNull(EmailSettingNames.Smtp.Domain)
            ?.WithGroup(
                "Emailing",
                L("Settings:Emailing"),
                order: 1,
                requiredFeatures: [SettingManagementFeatures.Enable, SettingManagementFeatures.AllowChangingEmailSettings],
                requiredPermissions: [Volo.Abp.SettingManagement.SettingManagementPermissions.Emailing])
            ?.WithParent(
                "Smtp",
                L("Settings:Emailing.Smtp"),
                order: 1,
                requiredPermissions: [AbpSettingManagementPermissions.Settings.Manager])
            ?.WithOrder(0)
            ?.WithValueType(ValueType.String)
            ?.ReplaceProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName);
        context.GetOrNull(EmailSettingNames.Smtp.Host)
            ?.WithGroup(
                "Emailing",
                L("Settings:Emailing"),
                order: 1,
                requiredFeatures: [SettingManagementFeatures.Enable, SettingManagementFeatures.AllowChangingEmailSettings],
                requiredPermissions: [Volo.Abp.SettingManagement.SettingManagementPermissions.Emailing])
            ?.WithParent(
                "Smtp",
                L("Settings:Emailing.Smtp"),
                order: 1,
                requiredPermissions: [AbpSettingManagementPermissions.Settings.Manager])
            ?.WithOrder(1)
            ?.WithValueType(ValueType.String)
            ?.ReplaceProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName);
        context.GetOrNull(EmailSettingNames.Smtp.Port)
            ?.WithGroup(
                "Emailing",
                L("Settings:Emailing"),
                order: 1,
                requiredFeatures: [SettingManagementFeatures.Enable, SettingManagementFeatures.AllowChangingEmailSettings],
                requiredPermissions: [Volo.Abp.SettingManagement.SettingManagementPermissions.Emailing])
            ?.WithParent(
                "Smtp",
                L("Settings:Emailing.Smtp"),
                order: 1,
                requiredPermissions: [AbpSettingManagementPermissions.Settings.Manager])
            ?.WithOrder(2)
            ?.WithValueType(ValueType.Number)
            ?.ReplaceProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName);
        context.GetOrNull(EmailSettingNames.Smtp.UserName)
            ?.WithGroup(
                "Emailing",
                L("Settings:Emailing"),
                order: 1,
                requiredFeatures: [SettingManagementFeatures.Enable, SettingManagementFeatures.AllowChangingEmailSettings],
                requiredPermissions: [Volo.Abp.SettingManagement.SettingManagementPermissions.Emailing])
            ?.WithParent(
                "Smtp",
                L("Settings:Emailing.Smtp"),
                order: 1,
                requiredPermissions: [AbpSettingManagementPermissions.Settings.Manager])
            ?.WithOrder(3)
            ?.WithValueType(ValueType.String)
            ?.ReplaceProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName);
        context.GetOrNull(EmailSettingNames.Smtp.Password)
            ?.WithGroup(
                "Emailing",
                L("Settings:Emailing"),
                order: 1,
                requiredFeatures: [SettingManagementFeatures.Enable, SettingManagementFeatures.AllowChangingEmailSettings],
                requiredPermissions: [Volo.Abp.SettingManagement.SettingManagementPermissions.Emailing])
            ?.WithParent(
                "Smtp",
                L("Settings:Emailing.Smtp"),
                order: 1,
                requiredPermissions: [AbpSettingManagementPermissions.Settings.Manager])
            ?.WithOrder(4)
            ?.WithValueType(ValueType.String)
            ?.ReplaceProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName);
        context.GetOrNull(EmailSettingNames.Smtp.EnableSsl)
            ?.WithGroup(
                "Emailing",
                L("Settings:Emailing"),
                order: 1,
                requiredFeatures: [SettingManagementFeatures.Enable, SettingManagementFeatures.AllowChangingEmailSettings],
                requiredPermissions: [Volo.Abp.SettingManagement.SettingManagementPermissions.Emailing])
            ?.WithParent(
                "Smtp",
                L("Settings:Emailing.Smtp"),
                order: 1,
                requiredPermissions: [AbpSettingManagementPermissions.Settings.Manager])
            ?.WithOrder(5)
            ?.WithValueType(ValueType.Boolean)
            ?.ReplaceProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName);
        context.GetOrNull(EmailSettingNames.Smtp.UseDefaultCredentials)
            ?.WithGroup(
                "Emailing",
                L("Settings:Emailing"),
                order: 1,
                requiredFeatures: [SettingManagementFeatures.Enable, SettingManagementFeatures.AllowChangingEmailSettings],
                requiredPermissions: [Volo.Abp.SettingManagement.SettingManagementPermissions.Emailing])
            ?.WithParent(
                "Smtp",
                L("Settings:Emailing.Smtp"),
                order: 1,
                requiredPermissions: [AbpSettingManagementPermissions.Settings.Manager])
            ?.WithOrder(6)
            ?.WithValueType(ValueType.Boolean)
            ?.ReplaceProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName);
    }

    public static LocalizableString L(string name)
    {
        return LocalizableString.Create<AbpSettingManagementResource>(name);
    }
}

