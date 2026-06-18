using Volo.Abp.Identity.Localization;
using Volo.Abp.Identity.Settings;
using Volo.Abp.Localization;
using Volo.Abp.Settings;
using ValueType = Volo.Abp.Settings.ValueType;

namespace LINGYUN.Abp.Identity;

public class IdentityGroupSettingDefinitionProvider : SettingDefinitionProvider
{
    private const string GroupName = "Identity";
    private const int GroupOrder = 10;

    public override void Define(ISettingDefinitionContext context)
    {
        SetLockoutSettingGroup(context);
        SetUserSettingGroup(context);
        SetSignInSettingGroup(context);
        SetPasswordSettingGroup(context);
        SetOrganizationUnitSettingGroup(context);
    }

    private static void SetLockoutSettingGroup(ISettingDefinitionContext context)
    {
        context.GetOrNull(IdentitySettingNames.Lockout.AllowedForNewUsers)
            ?.WithGroup(GroupName, L("Settings:Identity"), GroupOrder)
            ?.WithParent("Lockout", L("Settings:Identity.Lockout"), order: 3)
            ?.WithOrder(0)
            ?.WithValueType(ValueType.Boolean)
            ?.ReplaceProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName);
        context.GetOrNull(IdentitySettingNames.Lockout.LockoutDuration)
            ?.WithGroup(GroupName, L("Settings:Identity"), GroupOrder)
            ?.WithParent("Lockout", L("Settings:Identity.Lockout"), order: 3)
            ?.WithOrder(1)
            ?.WithValueType(ValueType.Number)
            ?.ReplaceProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName);
        context.GetOrNull(IdentitySettingNames.Lockout.MaxFailedAccessAttempts)
            ?.WithGroup(GroupName, L("Settings:Identity"), GroupOrder)
            ?.WithParent("Lockout", L("Settings:Identity.Lockout"), order: 3)
            ?.WithOrder(2)
            ?.WithValueType(ValueType.Number)
            ?.ReplaceProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName);
    }

    private static void SetUserSettingGroup(ISettingDefinitionContext context)
    {
        context.GetOrNull(IdentitySettingNames.User.IsEmailUpdateEnabled)
            ?.WithGroup(GroupName, L("Settings:Identity"), GroupOrder)
            ?.WithParent("User", L("Settings:Identity.User"), order: 1)
            ?.WithOrder(0)
            ?.WithValueType(ValueType.Boolean)
            ?.ReplaceProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName);
        context.GetOrNull(IdentitySettingNames.User.IsUserNameUpdateEnabled)
            ?.WithGroup(GroupName, L("Settings:Identity"), GroupOrder)
            ?.WithParent("User", L("Settings:Identity.User"), order: 1)
            ?.WithOrder(1)
            ?.WithValueType(ValueType.Boolean)
            ?.ReplaceProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName);
    }

    private static void SetSignInSettingGroup(ISettingDefinitionContext context)
    {
        context.GetOrNull(IdentitySettingNames.SignIn.RequireConfirmedEmail)
            ?.WithGroup(GroupName, L("Settings:Identity"), GroupOrder)
            ?.WithParent("SignIn", L("Settings:Identity.SignIn"), order: 2)
            ?.WithOrder(0)
            ?.WithValueType(ValueType.Boolean)
            ?.ReplaceProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName);
        context.GetOrNull(IdentitySettingNames.SignIn.RequireEmailVerificationToRegister)
            ?.WithGroup(GroupName, L("Settings:Identity"), GroupOrder)
            ?.WithParent("SignIn", L("Settings:Identity.SignIn"), order: 2)
            ?.WithOrder(1)
            ?.WithValueType(ValueType.Boolean)
            ?.ReplaceProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName);
        context.GetOrNull(IdentitySettingNames.SignIn.EnablePhoneNumberConfirmation)
            ?.WithGroup(GroupName, L("Settings:Identity"), GroupOrder)
            ?.WithParent("SignIn", L("Settings:Identity.SignIn"), order: 2)
            ?.WithOrder(2)
            ?.WithValueType(ValueType.Boolean)
            ?.ReplaceProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName);
        context.GetOrNull(IdentitySettingNames.SignIn.RequireConfirmedPhoneNumber)
            ?.WithGroup(GroupName, L("Settings:Identity"), GroupOrder)
            ?.WithParent("SignIn", L("Settings:Identity.SignIn"), order: 2)
            ?.WithOrder(3)
            ?.WithValueType(ValueType.Boolean)
            ?.ReplaceProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName);
    }

    private static void SetPasswordSettingGroup(ISettingDefinitionContext context)
    {
        context.GetOrNull(IdentitySettingNames.Password.RequireDigit)
            ?.WithGroup(GroupName, L("Settings:Identity"), GroupOrder)
            ?.WithParent("Password", L("Settings:Identity.Password"), order: 4)
            ?.WithOrder(0)
            ?.WithValueType(ValueType.Boolean)
            ?.ReplaceProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName);
        context.GetOrNull(IdentitySettingNames.Password.RequiredLength)
            ?.WithGroup(GroupName, L("Settings:Identity"), GroupOrder)
            ?.WithParent("Password", L("Settings:Identity.Password"), order: 4)
            ?.WithOrder(1)
            ?.WithValueType(ValueType.Number)
            ?.ReplaceProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName);
        context.GetOrNull(IdentitySettingNames.Password.RequiredUniqueChars)
            ?.WithGroup(GroupName, L("Settings:Identity"), GroupOrder)
            ?.WithParent("Password", L("Settings:Identity.Password"), order: 4)
            ?.WithOrder(2)
            ?.WithValueType(ValueType.Number)
            ?.ReplaceProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName);
        context.GetOrNull(IdentitySettingNames.Password.RequireLowercase)
            ?.WithGroup(GroupName, L("Settings:Identity"), GroupOrder)
            ?.WithParent("Password", L("Settings:Identity.Password"), order: 4)
            ?.WithOrder(3)
            ?.WithValueType(ValueType.Boolean)
            ?.ReplaceProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName);
        context.GetOrNull(IdentitySettingNames.Password.RequireUppercase)
            ?.WithGroup(GroupName, L("Settings:Identity"), GroupOrder)
            ?.WithParent("Password", L("Settings:Identity.Password"), order: 4)
            ?.WithOrder(4)
            ?.WithValueType(ValueType.Boolean)
            ?.ReplaceProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName);
        context.GetOrNull(IdentitySettingNames.Password.RequireNonAlphanumeric)
            ?.WithGroup(GroupName, L("Settings:Identity"), GroupOrder)
            ?.WithParent("Password", L("Settings:Identity.Password"), order: 4)
            ?.WithOrder(5)
            ?.WithValueType(ValueType.Boolean)
            ?.ReplaceProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName);
        context.GetOrNull(IdentitySettingNames.Password.ForceUsersToPeriodicallyChangePassword)
            ?.WithGroup(GroupName, L("Settings:Identity"), GroupOrder)
            ?.WithParent("Password", L("Settings:Identity.Password"), order: 4)
            ?.WithOrder(6)
            ?.WithValueType(ValueType.Boolean)
            ?.ReplaceProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName);
        context.GetOrNull(IdentitySettingNames.Password.PasswordChangePeriodDays)
            ?.WithGroup(GroupName, L("Settings:Identity"), GroupOrder)
            ?.WithParent("Password", L("Settings:Identity.Password"), order: 4)
            ?.WithOrder(7)
            ?.WithValueType(ValueType.Number)
            ?.ReplaceProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName);
        context.GetOrNull(IdentitySettingNames.Password.EnablePreventPasswordReuse)
            ?.WithGroup(GroupName, L("Settings:Identity"), GroupOrder)
            ?.WithParent("Password", L("Settings:Identity.Password"), order: 4)
            ?.WithOrder(8)
            ?.WithValueType(ValueType.Boolean)
            ?.ReplaceProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName);
        context.GetOrNull(IdentitySettingNames.Password.PreventPasswordReuseCount)
            ?.WithGroup(GroupName, L("Settings:Identity"), GroupOrder)
            ?.WithParent("Password", L("Settings:Identity.Password"), order: 4)
            ?.WithOrder(9)
            ?.WithValueType(ValueType.Number)
            ?.ReplaceProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName);
    }

    private static void SetOrganizationUnitSettingGroup(ISettingDefinitionContext context)
    {
        context.GetOrNull(IdentitySettingNames.OrganizationUnit.MaxUserMembershipCount)
            ?.WithGroup(GroupName, L("Settings:Identity"), GroupOrder)
            ?.WithParent("OrganizationUnit", L("Settings:Identity.OrganizationUnit"), order: 6)
            ?.WithOrder(0)
            ?.WithValueType(ValueType.Number)
            ?.ReplaceProviders(
                DefaultValueSettingValueProvider.ProviderName,
                ConfigurationSettingValueProvider.ProviderName,
                GlobalSettingValueProvider.ProviderName,
                TenantSettingValueProvider.ProviderName);
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<IdentityResource>(name);
    }
}
