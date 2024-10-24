using LINGYUN.Abp.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Account.Settings;
using Volo.Abp.Application.Services;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.Caching;
using Volo.Abp.Emailing;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Features;
using Volo.Abp.Identity.Settings;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;
using Volo.Abp.SettingManagement;
using Volo.Abp.SettingManagement.Localization;
using Volo.Abp.Settings;
using Volo.Abp.Timing;
using Volo.Abp.Users;

namespace LINGYUN.Abp.SettingManagement;

[Authorize(AbpSettingManagementPermissions.Settings.Default)]
public class SettingAppService : ApplicationService, ISettingAppService, ISettingTestAppService
{
    protected AbpLocalizationOptions LocalizationOptions { get; }

    protected IDistributedEventBus EventBus { get; }
    protected ISettingManager SettingManager { get; }
    protected ITimezoneProvider TimezoneProvider { get; }
    protected ISettingDefinitionManager SettingDefinitionManager { get; }

    protected IDistributedCache<SettingCacheItem> Cache { get; }

    public SettingAppService(
        IDistributedEventBus eventBus,
        ISettingManager settingManager,
        ITimezoneProvider timezoneProvider,
        IDistributedCache<SettingCacheItem> cache,
        IOptions<AbpLocalizationOptions> localizationOptions,
        ISettingDefinitionManager settingDefinitionManager)
    {
        Cache = cache;
        EventBus = eventBus;
        SettingManager = settingManager;
        TimezoneProvider = timezoneProvider;
        SettingDefinitionManager = settingDefinitionManager;
        LocalizationOptions = localizationOptions.Value;
        LocalizationResource = typeof(AbpSettingManagementResource);
    }

    [Authorize(AbpSettingManagementPermissions.Settings.Manager)]
    public async virtual Task SetGlobalAsync(UpdateSettingsDto input)
    {
        if (CurrentTenant.IsAvailable)
        {
            throw new UserFriendlyException(L["TenantCannotChangeGlobalSetting"]);
        }

        // 增加特性检查
        await CheckFeatureAsync();

        foreach (var setting in input.Settings)
        {
            await SettingManager.SetGlobalAsync(setting.Name, setting.Value);
        }

        CurrentUnitOfWork.OnCompleted(async () =>
        {
            // 发送刷新用户缓存事件
            await EventBus.PublishAsync(new CurrentApplicationConfigurationCacheResetEventData());
        });

        await CurrentUnitOfWork.SaveChangesAsync();
    }

    [Authorize(AbpSettingManagementPermissions.Settings.Manager)]
    public async virtual Task SetCurrentTenantAsync(UpdateSettingsDto input)
    {
        // 增加特性检查
        await CheckFeatureAsync();

        if (CurrentTenant.IsAvailable)
        {
            foreach (var setting in input.Settings)
            {
                await SettingManager.SetForTenantAsync(CurrentTenant.GetId(), setting.Name, setting.Value);
            }

            CurrentUnitOfWork.OnCompleted(async () =>
            {
                // 发送刷新用户缓存事件
                await EventBus.PublishAsync(new CurrentApplicationConfigurationCacheResetEventData());
            });

            await CurrentUnitOfWork.SaveChangesAsync();
        }
    }

    //[Authorize]
    [AllowAnonymous]
    public async virtual Task<SettingGroupResult> GetAllForCurrentTenantAsync()
    {
        return await GetAllForProviderAsync(TenantSettingValueProvider.ProviderName, CurrentTenant.GetId().ToString());
    }

    [AllowAnonymous]
    public async virtual Task<SettingGroupResult> GetAllForGlobalAsync()
    {
        return await GetAllForProviderAsync(GlobalSettingValueProvider.ProviderName, null);
    }

    protected async virtual Task<SettingGroupResult> GetAllForProviderAsync(string providerName, string providerKey)
    {
        /*
         * 2020-11-19
         * colin@foxmail.com
         * 
         * 之所以重构为这种看似硬编码的设计,是因为硬编码的都是常用的自带的配置
         * 
         * 自定义的应用模块可以实现返回相同Dto的配置服务,然后通过路由的聚合功能,
         * 轻松加入到前端的设置管理(因为vue前端已设计为动态表单页面)
         * 
         * 最初的设计才是不合理的,前端不能硬编码设置管理界面,这应该是后端的事情
         */ 

        // 2021-12-11 重新约定返回格式, 当用户明确只需要对应提供者设置才返回，否则为空数组

        var settingGroups = new SettingGroupResult();

        #region 系统设置

        var sysSettingGroup = new SettingGroupDto(L["DisplayName:System"], L["Description:System"]);
        // 语言
        var languageSetting = sysSettingGroup.AddSetting(L["DisplayName:System.Language"], L["Description:System.Language"]);
        languageSetting.AddDetail(
            await SettingDefinitionManager.GetAsync(LocalizationSettingNames.DefaultLanguage),
            StringLocalizerFactory,
            await SettingManager.GetOrNullAsync(LocalizationSettingNames.DefaultLanguage, providerName, providerKey),
            ValueType.Option,
            providerName)
            ?.AddOptions(LocalizationOptions.Languages.Select(l => new OptionDto(l.DisplayName, l.CultureName)));
        // 时区
        var timezones = TimeZoneHelper.GetTimezones(TimezoneProvider.GetWindowsTimezones());
        var timingSetting = sysSettingGroup.AddSetting(L["DisplayName:System.Timing"], L["Description:System.Timing"]);
        timingSetting.AddDetail(
           await SettingDefinitionManager.GetAsync(TimingSettingNames.TimeZone),
           StringLocalizerFactory,
           await SettingManager.GetOrNullAsync(TimingSettingNames.TimeZone, providerName, providerKey),
           ValueType.Option,
           providerName)
            .AddOptions(timezones.Select(timezone => new OptionDto(timezone.Name, timezone.Value)))
            .RequiredPermission("SettingManagement.TimeZone");
        settingGroups.AddGroup(sysSettingGroup);

        #endregion

        #region 安全设置

        var securitySettingGroup = new SettingGroupDto(L["DisplayName:Security"], L["Description:Security"]);

        // 用户账户
        var accountSetting = securitySettingGroup.AddSetting(L["DisplayName:Security.Account"], L["Description:Security.Account"]);
        // 启用本地登录
        accountSetting.AddDetail(
            await SettingDefinitionManager.GetAsync(AccountSettingNames.EnableLocalLogin),
            StringLocalizerFactory,
            await SettingManager.GetOrNullAsync(AccountSettingNames.EnableLocalLogin, providerName, providerKey),
            ValueType.Boolean,
            providerName);
        accountSetting.AddDetail(
            await SettingDefinitionManager.GetAsync(AccountSettingNames.IsSelfRegistrationEnabled),
            StringLocalizerFactory,
            await SettingManager.GetOrNullAsync(AccountSettingNames.IsSelfRegistrationEnabled, providerName, providerKey),
            ValueType.Boolean,
            providerName);

        settingGroups.AddGroup(securitySettingGroup);

        #endregion

        #region 身份标识设置

        // 身份标识设置
        var identitySetting = new SettingGroupDto(L["DisplayName:Identity"], L["Description:Identity"]);

        #region 用户锁定

        var lockoutSetting = identitySetting.AddSetting(L["DisplayName:Identity.Lockout"], L["Description:Identity.Lockout"]);
        lockoutSetting.AddDetail(
            await SettingDefinitionManager.GetAsync(IdentitySettingNames.Lockout.AllowedForNewUsers),
            StringLocalizerFactory,
            await SettingManager.GetOrNullAsync(IdentitySettingNames.Lockout.AllowedForNewUsers, providerName, providerKey),
            ValueType.Boolean,
            providerName);
        lockoutSetting.AddDetail(
            await SettingDefinitionManager.GetAsync(IdentitySettingNames.Lockout.LockoutDuration),
            StringLocalizerFactory,
            await SettingManager.GetOrNullAsync(IdentitySettingNames.Lockout.LockoutDuration, providerName, providerKey),
            ValueType.Number,
            providerName);
        lockoutSetting.AddDetail(
            await SettingDefinitionManager.GetAsync(IdentitySettingNames.Lockout.MaxFailedAccessAttempts),
            StringLocalizerFactory,
            await SettingManager.GetOrNullAsync(IdentitySettingNames.Lockout.MaxFailedAccessAttempts, providerName, providerKey),
            ValueType.Number,
            providerName);

        #endregion

        #region 用户

        var userSetting = identitySetting.AddSetting(L["DisplayName:Identity.User"], L["Description:Identity.User"]);
        userSetting.AddDetail(
            await SettingDefinitionManager.GetAsync(IdentitySettingNames.User.IsEmailUpdateEnabled),
            StringLocalizerFactory,
            await SettingManager.GetOrNullAsync(IdentitySettingNames.User.IsEmailUpdateEnabled, providerName, providerKey),
            ValueType.Boolean,
            providerName);
        userSetting.AddDetail(
            await SettingDefinitionManager.GetAsync(IdentitySettingNames.User.IsUserNameUpdateEnabled),
            StringLocalizerFactory,
            await SettingManager.GetOrNullAsync(IdentitySettingNames.User.IsUserNameUpdateEnabled, providerName, providerKey),
            ValueType.Boolean,
            providerName);
        userSetting.AddDetail(
            await SettingDefinitionManager.GetAsync(LINGYUN.Abp.Identity.Settings.IdentitySettingNames.User.SmsNewUserRegister),
            StringLocalizerFactory,
            await SettingManager.GetOrNullAsync(LINGYUN.Abp.Identity.Settings.IdentitySettingNames.User.SmsNewUserRegister, providerName, providerKey),
            ValueType.String,
            providerName);
        userSetting.AddDetail(
            await SettingDefinitionManager.GetAsync(LINGYUN.Abp.Identity.Settings.IdentitySettingNames.User.SmsResetPassword),
            StringLocalizerFactory,
            await SettingManager.GetOrNullAsync(LINGYUN.Abp.Identity.Settings.IdentitySettingNames.User.SmsResetPassword, providerName, providerKey),
            ValueType.String,
            providerName);
        userSetting.AddDetail(
            await SettingDefinitionManager.GetAsync(LINGYUN.Abp.Identity.Settings.IdentitySettingNames.User.SmsUserSignin),
            StringLocalizerFactory,
            await SettingManager.GetOrNullAsync(LINGYUN.Abp.Identity.Settings.IdentitySettingNames.User.SmsUserSignin, providerName, providerKey),
            ValueType.String,
            providerName);
        userSetting.AddDetail(
            await SettingDefinitionManager.GetAsync(LINGYUN.Abp.Identity.Settings.IdentitySettingNames.User.SmsPhoneNumberConfirmed),
            StringLocalizerFactory,
            await SettingManager.GetOrNullAsync(LINGYUN.Abp.Identity.Settings.IdentitySettingNames.User.SmsPhoneNumberConfirmed, providerName, providerKey),
            ValueType.String,
            providerName);
        userSetting.AddDetail(
            await SettingDefinitionManager.GetAsync(LINGYUN.Abp.Identity.Settings.IdentitySettingNames.User.SmsRepetInterval),
            StringLocalizerFactory,
            await SettingManager.GetOrNullAsync(LINGYUN.Abp.Identity.Settings.IdentitySettingNames.User.SmsRepetInterval, providerName, providerKey),
            ValueType.Number,
            providerName);

        #endregion

        #region 登录

        var signinSetting = identitySetting.AddSetting(L["DisplayName:Identity.SignIn"], L["Description:Identity.SignIn"]);
        signinSetting.AddDetail(
            await SettingDefinitionManager.GetAsync(IdentitySettingNames.SignIn.EnablePhoneNumberConfirmation),
            StringLocalizerFactory,
            await SettingManager.GetOrNullAsync(IdentitySettingNames.SignIn.EnablePhoneNumberConfirmation, providerName, providerKey),
            ValueType.Boolean,
            providerName);
        signinSetting.AddDetail(
            await SettingDefinitionManager.GetAsync(IdentitySettingNames.SignIn.RequireConfirmedEmail),
            StringLocalizerFactory,
            await SettingManager.GetOrNullAsync(IdentitySettingNames.SignIn.RequireConfirmedEmail, providerName, providerKey),
            ValueType.Boolean,
            providerName);
        signinSetting.AddDetail(
            await SettingDefinitionManager.GetAsync(IdentitySettingNames.SignIn.RequireConfirmedPhoneNumber),
            StringLocalizerFactory,
            await SettingManager.GetOrNullAsync(IdentitySettingNames.SignIn.RequireConfirmedPhoneNumber, providerName, providerKey),
            ValueType.Boolean,
            providerName);

        #endregion

        #region 会话

        var sessionSetting = identitySetting.AddSetting(L["DisplayName:Identity.Session"], L["Description:Identity.Session"]);
        sessionSetting.AddDetail(
            await SettingDefinitionManager.GetAsync(LINGYUN.Abp.Identity.Settings.IdentitySettingNames.Session.ConcurrentLoginStrategy),
            StringLocalizerFactory,
            await SettingManager.GetOrNullAsync(LINGYUN.Abp.Identity.Settings.IdentitySettingNames.Session.ConcurrentLoginStrategy, providerName, providerKey),
            ValueType.Option,
            providerName)
            .AddOption(L["ConcurrentLoginStrategy:None"], ConcurrentLoginStrategy.None.ToString())
            .AddOption(L["ConcurrentLoginStrategy:LogoutFromSameTypeDevicesLimit"], ConcurrentLoginStrategy.LogoutFromSameTypeDevicesLimit.ToString())
            .AddOption(L["ConcurrentLoginStrategy:LogoutFromSameTypeDevices"], ConcurrentLoginStrategy.LogoutFromSameTypeDevices.ToString())
            .AddOption(L["ConcurrentLoginStrategy:LogoutFromAllDevices"], ConcurrentLoginStrategy.LogoutFromAllDevices.ToString());
        sessionSetting.AddDetail(
            await SettingDefinitionManager.GetAsync(LINGYUN.Abp.Identity.Settings.IdentitySettingNames.Session.LogoutFromSameTypeDevicesLimit),
            StringLocalizerFactory,
            await SettingManager.GetOrNullAsync(LINGYUN.Abp.Identity.Settings.IdentitySettingNames.Session.LogoutFromSameTypeDevicesLimit, providerName, providerKey),
            ValueType.Number,
            providerName);

        #endregion

        #region 密码

        var passwordSetting = identitySetting.AddSetting(L["DisplayName:Identity.Password"], L["Description:Identity.Password"]);
        passwordSetting.AddDetail(
            await SettingDefinitionManager.GetAsync(IdentitySettingNames.Password.RequireDigit),
            StringLocalizerFactory,
            await SettingManager.GetOrNullAsync(IdentitySettingNames.Password.RequireDigit, providerName, providerKey),
            ValueType.Boolean,
            providerName);
        passwordSetting.AddDetail(
            await SettingDefinitionManager.GetAsync(IdentitySettingNames.Password.RequiredLength),
            StringLocalizerFactory,
            await SettingManager.GetOrNullAsync(IdentitySettingNames.Password.RequiredLength, providerName, providerKey),
            ValueType.Number,
            providerName);
        passwordSetting.AddDetail(
            await SettingDefinitionManager.GetAsync(IdentitySettingNames.Password.RequiredUniqueChars),
            StringLocalizerFactory,
            await SettingManager.GetOrNullAsync(IdentitySettingNames.Password.RequiredUniqueChars, providerName, providerKey),
            ValueType.Number,
            providerName);
        passwordSetting.AddDetail(
            await SettingDefinitionManager.GetAsync(IdentitySettingNames.Password.RequireLowercase),
            StringLocalizerFactory,
            await SettingManager.GetOrNullAsync(IdentitySettingNames.Password.RequireLowercase, providerName, providerKey),
            ValueType.Boolean,
            providerName);
        passwordSetting.AddDetail(
            await SettingDefinitionManager.GetAsync(IdentitySettingNames.Password.RequireUppercase),
            StringLocalizerFactory,
            await SettingManager.GetOrNullAsync(IdentitySettingNames.Password.RequireUppercase, providerName, providerKey),
            ValueType.Boolean,
            providerName);
        passwordSetting.AddDetail(
            await SettingDefinitionManager.GetAsync(IdentitySettingNames.Password.RequireNonAlphanumeric),
            StringLocalizerFactory,
            await SettingManager.GetOrNullAsync(IdentitySettingNames.Password.RequireNonAlphanumeric, providerName, providerKey),
            ValueType.Boolean,
            providerName);
        passwordSetting.AddDetail(
            await SettingDefinitionManager.GetAsync(IdentitySettingNames.Password.ForceUsersToPeriodicallyChangePassword),
            StringLocalizerFactory,
            await SettingManager.GetOrNullAsync(IdentitySettingNames.Password.ForceUsersToPeriodicallyChangePassword, providerName, providerKey),
            ValueType.Boolean,
            providerName);
        passwordSetting.AddDetail(
            await SettingDefinitionManager.GetAsync(IdentitySettingNames.Password.PasswordChangePeriodDays),
            StringLocalizerFactory,
            await SettingManager.GetOrNullAsync(IdentitySettingNames.Password.PasswordChangePeriodDays, providerName, providerKey),
            ValueType.Number,
            providerName);

        #endregion

        #region 双因素

        // Removed See: https://github.com/abpframework/abp/pull/7719
        //var twoFactorSetting = identitySetting.AddSetting(L["DisplayName:Identity.TwoFactor"], L["Description:Identity.TwoFactor"]);
        //twoFactorSetting.AddDetail(
        //    SettingDefinitionManager.Get(IdentitySettingNames.TwoFactor.Behaviour),
        //    StringLocalizerFactory,
        //    await SettingManager.GetOrNullAsync(IdentitySettingNames.TwoFactor.Behaviour, providerName, providerKey),
        //    ValueType.Option)
        //    .AddOption(IdentityTwoFactorBehaviour.Optional.ToString(), IdentityTwoFactorBehaviour.Optional.ToString())
        //    .AddOption(IdentityTwoFactorBehaviour.Forced.ToString(), IdentityTwoFactorBehaviour.Forced.ToString())
        //    .AddOption(IdentityTwoFactorBehaviour.Disabled.ToString(), IdentityTwoFactorBehaviour.Disabled.ToString());
        //twoFactorSetting.AddDetail(
        //    SettingDefinitionManager.Get(IdentitySettingNames.TwoFactor.UsersCanChange),
        //    StringLocalizerFactory,
        //    await SettingManager.GetOrNullAsync(IdentitySettingNames.TwoFactor.UsersCanChange, providerName, providerKey),
        //    ValueType.Boolean);

        #endregion

        #region 组织机构

        var ouSetting = identitySetting.AddSetting(L["DisplayName:Identity.OrganizationUnit"], L["Description:Identity.OrganizationUnit"]);
        ouSetting.AddDetail(
            await SettingDefinitionManager.GetAsync(IdentitySettingNames.OrganizationUnit.MaxUserMembershipCount),
            StringLocalizerFactory,
            await SettingManager.GetOrNullAsync(IdentitySettingNames.OrganizationUnit.MaxUserMembershipCount, providerName, providerKey),
            ValueType.Number,
            providerName);

        settingGroups.AddGroup(identitySetting);

        #endregion

        #endregion

        #region 邮件设置

        if (!CurrentTenant.IsAvailable || 
            await FeatureChecker.IsEnabledAsync(true, [SettingManagementFeatures.Enable, SettingManagementFeatures.AllowChangingEmailSettings]))
        {
            var emailSettingGroup = new SettingGroupDto(L["DisplayName:Emailing"], L["Description:Emailing"]);

            var defaultMailSetting = emailSettingGroup.AddSetting(L["DisplayName:Emailing.Default"], L["Description:Emailing.Default"]);
            defaultMailSetting.AddDetail(
                await SettingDefinitionManager.GetAsync(EmailSettingNames.DefaultFromAddress),
                StringLocalizerFactory,
                await SettingManager.GetOrNullAsync(EmailSettingNames.DefaultFromAddress, providerName, providerKey),
                ValueType.String,
                providerName)
                .RequiredPermission("SettingManagement.Emailing");
            defaultMailSetting.AddDetail(
                await SettingDefinitionManager.GetAsync(EmailSettingNames.DefaultFromDisplayName),
                StringLocalizerFactory,
                await SettingManager.GetOrNullAsync(EmailSettingNames.DefaultFromDisplayName, providerName, providerKey),
                ValueType.String,
                providerName)
                .RequiredPermission("SettingManagement.Emailing");

            // 防止邮件设置泄露
            if (await AuthorizationService.IsGrantedAsync(AbpSettingManagementPermissions.Settings.Manager))
            {
                var smtpSetting = emailSettingGroup.AddSetting(L["DisplayName:Emailing.Smtp"], L["Description:Emailing.Smtp"]);
                smtpSetting.AddDetail(
                    await SettingDefinitionManager.GetAsync(EmailSettingNames.Smtp.EnableSsl),
                    StringLocalizerFactory,
                    await SettingManager.GetOrNullAsync(EmailSettingNames.Smtp.EnableSsl, providerName, providerKey),
                    ValueType.Boolean,
                    providerName)
                    .RequiredPermission("SettingManagement.Emailing");
                smtpSetting.AddDetail(
                    await SettingDefinitionManager.GetAsync(EmailSettingNames.Smtp.UseDefaultCredentials),
                    StringLocalizerFactory,
                    await SettingManager.GetOrNullAsync(EmailSettingNames.Smtp.UseDefaultCredentials, providerName, providerKey),
                    ValueType.Boolean,
                    providerName)
                    .RequiredPermission("SettingManagement.Emailing");
                smtpSetting.AddDetail(
                    await SettingDefinitionManager.GetAsync(EmailSettingNames.Smtp.Domain),
                    StringLocalizerFactory,
                    await SettingManager.GetOrNullAsync(EmailSettingNames.Smtp.Domain, providerName, providerKey),
                    ValueType.String,
                    providerName)
                    .RequiredPermission("SettingManagement.Emailing");
                smtpSetting.AddDetail(
                    await SettingDefinitionManager.GetAsync(EmailSettingNames.Smtp.Host),
                    StringLocalizerFactory,
                    await SettingManager.GetOrNullAsync(EmailSettingNames.Smtp.Host, providerName, providerKey),
                    ValueType.String,
                    providerName);
                smtpSetting.AddDetail(
                    await SettingDefinitionManager.GetAsync(EmailSettingNames.Smtp.Port),
                    StringLocalizerFactory,
                    await SettingManager.GetOrNullAsync(EmailSettingNames.Smtp.Port, providerName, providerKey),
                    ValueType.Number,
                    providerName)
                    .RequiredPermission("SettingManagement.Emailing");
                smtpSetting.AddDetail(
                    await SettingDefinitionManager.GetAsync(EmailSettingNames.Smtp.UserName),
                    StringLocalizerFactory,
                    await SettingManager.GetOrNullAsync(EmailSettingNames.Smtp.UserName, providerName, providerKey),
                    ValueType.String,
                    providerName)
                    .RequiredPermission("SettingManagement.Emailing");
                smtpSetting.AddDetail(
                    await SettingDefinitionManager.GetAsync(EmailSettingNames.Smtp.Password),
                    StringLocalizerFactory,
                    await SettingManager.GetOrNullAsync(EmailSettingNames.Smtp.Password, providerName, providerKey),
                    ValueType.String,
                    providerName)
                    .RequiredPermission("SettingManagement.Emailing");
                // 一个占位符,用于展现发送测试邮件
                smtpSetting.AddDetail(
                    new SettingDefinition(
                        name: "SendTestEmail",
                        displayName: LocalizableString.Create<AbpSettingManagementResource>("DisplayName:Emailing.SendTestEmail"),
                        description: LocalizableString.Create<AbpSettingManagementResource>("Description:Emailing.SendTestEmail")),
                    StringLocalizerFactory,
                    "",
                    ValueType.String,
                    providerName)
                    .WithSlot("send-test-email")
                    .RequiredPermission("SettingManagement.Emailing");
            }


            settingGroups.AddGroup(emailSettingGroup);
        }

        #endregion

        return settingGroups;
    }

    protected async virtual Task CheckFeatureAsync()
    {
        await FeatureChecker.CheckEnabledAsync(SettingManagementFeatures.Enable);
    }

    public async virtual Task SendTestEmailAsync(SendTestEmailInput input)
    {
        var emailSender = LazyServiceProvider.LazyGetRequiredService<IEmailSender>();

        await emailSender.SendAsync(
            input.EmailAddress,
            L["SendTestEmail:Subject"],
            L["SendTestEmail:Body"]);
    }
}
