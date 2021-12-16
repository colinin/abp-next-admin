using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;
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

namespace LINGYUN.Abp.SettingManagement
{
    [Authorize(AbpSettingManagementPermissions.Settings.Default)]
    public class SettingAppService : ApplicationService, ISettingAppService
    {
        protected AbpLocalizationOptions LocalizationOptions { get; }

        protected IDistributedEventBus EventBus { get; }
        protected ISettingManager SettingManager { get; }
        protected ISettingDefinitionManager SettingDefinitionManager { get; }

        protected IDistributedCache<SettingCacheItem> Cache { get; }
        public SettingAppService(
            IDistributedEventBus eventBus,
            ISettingManager settingManager,
            IDistributedCache<SettingCacheItem> cache,
            IOptions<AbpLocalizationOptions> localizationOptions,
            ISettingDefinitionManager settingDefinitionManager)
        {
            Cache = cache;
            EventBus = eventBus;
            SettingManager = settingManager;
            SettingDefinitionManager = settingDefinitionManager;
            LocalizationOptions = localizationOptions.Value;
            LocalizationResource = typeof(AbpSettingManagementResource);
        }

        [Authorize(AbpSettingManagementPermissions.Settings.Manager)]
        public virtual async Task SetGlobalAsync(UpdateSettingsDto input)
        {
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
        public virtual async Task SetCurrentTenantAsync(UpdateSettingsDto input)
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
        public virtual async Task<SettingGroupResult> GetAllForCurrentTenantAsync()
        {
            return await GetAllForProviderAsync(TenantSettingValueProvider.ProviderName, CurrentTenant.GetId().ToString());
        }

        [AllowAnonymous]
        public virtual async Task<SettingGroupResult> GetAllForGlobalAsync()
        {
            return await GetAllForProviderAsync(GlobalSettingValueProvider.ProviderName, null);
        }

        protected virtual async Task<SettingGroupResult> GetAllForProviderAsync(string providerName, string providerKey)
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
                SettingDefinitionManager.Get(LocalizationSettingNames.DefaultLanguage),
                StringLocalizerFactory,
                await SettingManager.GetOrNullAsync(LocalizationSettingNames.DefaultLanguage, providerName, providerKey),
                ValueType.Option,
                providerName)
                ?.AddOptions(LocalizationOptions.Languages.Select(l => new OptionDto(l.DisplayName, l.CultureName)));
            // 时区
            var timingSetting = sysSettingGroup.AddSetting(L["DisplayName:System.Timing"], L["Description:System.Timing"]);
            timingSetting.AddDetail(
               SettingDefinitionManager.Get(TimingSettingNames.TimeZone),
               StringLocalizerFactory,
               await SettingManager.GetOrNullAsync(TimingSettingNames.TimeZone, providerName, providerKey),
               ValueType.String,
               providerName);
            settingGroups.AddGroup(sysSettingGroup);

            #endregion

            #region 安全设置

            var securitySettingGroup = new SettingGroupDto(L["DisplayName:Security"], L["Description:Security"]);

            // 用户账户
            var accountSetting = securitySettingGroup.AddSetting(L["DisplayName:Security.Account"], L["Description:Security.Account"]);
            // 启用本地登录
            accountSetting.AddDetail(
                SettingDefinitionManager.Get(AccountSettingNames.EnableLocalLogin),
                StringLocalizerFactory,
                await SettingManager.GetOrNullAsync(AccountSettingNames.EnableLocalLogin, providerName, providerKey),
                ValueType.Boolean,
                providerName);
            accountSetting.AddDetail(
                SettingDefinitionManager.Get(AccountSettingNames.IsSelfRegistrationEnabled),
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
                SettingDefinitionManager.Get(IdentitySettingNames.Lockout.AllowedForNewUsers),
                StringLocalizerFactory,
                await SettingManager.GetOrNullAsync(IdentitySettingNames.Lockout.AllowedForNewUsers, providerName, providerKey),
                ValueType.Boolean,
                providerName);
            lockoutSetting.AddDetail(
                SettingDefinitionManager.Get(IdentitySettingNames.Lockout.LockoutDuration),
                StringLocalizerFactory,
                await SettingManager.GetOrNullAsync(IdentitySettingNames.Lockout.LockoutDuration, providerName, providerKey),
                ValueType.Number,
                providerName);
            lockoutSetting.AddDetail(
                SettingDefinitionManager.Get(IdentitySettingNames.Lockout.MaxFailedAccessAttempts),
                StringLocalizerFactory,
                await SettingManager.GetOrNullAsync(IdentitySettingNames.Lockout.MaxFailedAccessAttempts, providerName, providerKey),
                ValueType.Number,
                providerName);

            #endregion

            #region 用户

            var userSetting = identitySetting.AddSetting(L["DisplayName:Identity.User"], L["Description:Identity.User"]);
            userSetting.AddDetail(
                SettingDefinitionManager.Get(IdentitySettingNames.User.IsEmailUpdateEnabled),
                StringLocalizerFactory,
                await SettingManager.GetOrNullAsync(IdentitySettingNames.User.IsEmailUpdateEnabled, providerName, providerKey),
                ValueType.Boolean,
                providerName);
            userSetting.AddDetail(
                SettingDefinitionManager.Get(IdentitySettingNames.User.IsUserNameUpdateEnabled),
                StringLocalizerFactory,
                await SettingManager.GetOrNullAsync(IdentitySettingNames.User.IsUserNameUpdateEnabled, providerName, providerKey),
                ValueType.Boolean,
                providerName);
            userSetting.AddDetail(
                SettingDefinitionManager.Get(LINGYUN.Abp.Identity.Settings.IdentitySettingNames.User.SmsNewUserRegister),
                StringLocalizerFactory,
                await SettingManager.GetOrNullAsync(LINGYUN.Abp.Identity.Settings.IdentitySettingNames.User.SmsNewUserRegister, providerName, providerKey),
                ValueType.String,
                providerName);
            userSetting.AddDetail(
                SettingDefinitionManager.Get(LINGYUN.Abp.Identity.Settings.IdentitySettingNames.User.SmsResetPassword),
                StringLocalizerFactory,
                await SettingManager.GetOrNullAsync(LINGYUN.Abp.Identity.Settings.IdentitySettingNames.User.SmsResetPassword, providerName, providerKey),
                ValueType.String,
                providerName);
            userSetting.AddDetail(
                SettingDefinitionManager.Get(LINGYUN.Abp.Identity.Settings.IdentitySettingNames.User.SmsUserSignin),
                StringLocalizerFactory,
                await SettingManager.GetOrNullAsync(LINGYUN.Abp.Identity.Settings.IdentitySettingNames.User.SmsUserSignin, providerName, providerKey),
                ValueType.String,
                providerName);
            userSetting.AddDetail(
                SettingDefinitionManager.Get(LINGYUN.Abp.Identity.Settings.IdentitySettingNames.User.SmsPhoneNumberConfirmed),
                StringLocalizerFactory,
                await SettingManager.GetOrNullAsync(LINGYUN.Abp.Identity.Settings.IdentitySettingNames.User.SmsPhoneNumberConfirmed, providerName, providerKey),
                ValueType.String,
                providerName);
            userSetting.AddDetail(
                SettingDefinitionManager.Get(LINGYUN.Abp.Identity.Settings.IdentitySettingNames.User.SmsRepetInterval),
                StringLocalizerFactory,
                await SettingManager.GetOrNullAsync(LINGYUN.Abp.Identity.Settings.IdentitySettingNames.User.SmsRepetInterval, providerName, providerKey),
                ValueType.Number,
                providerName);

            #endregion

            #region 登录

            var signinSetting = identitySetting.AddSetting(L["DisplayName:Identity.SignIn"], L["Description:Identity.SignIn"]);
            signinSetting.AddDetail(
                SettingDefinitionManager.Get(IdentitySettingNames.SignIn.EnablePhoneNumberConfirmation),
                StringLocalizerFactory,
                await SettingManager.GetOrNullAsync(IdentitySettingNames.SignIn.EnablePhoneNumberConfirmation, providerName, providerKey),
                ValueType.Boolean,
                providerName);
            signinSetting.AddDetail(
                SettingDefinitionManager.Get(IdentitySettingNames.SignIn.RequireConfirmedEmail),
                StringLocalizerFactory,
                await SettingManager.GetOrNullAsync(IdentitySettingNames.SignIn.RequireConfirmedEmail, providerName, providerKey),
                ValueType.Boolean,
                providerName);
            signinSetting.AddDetail(
                SettingDefinitionManager.Get(IdentitySettingNames.SignIn.RequireConfirmedPhoneNumber),
                StringLocalizerFactory,
                await SettingManager.GetOrNullAsync(IdentitySettingNames.SignIn.RequireConfirmedPhoneNumber, providerName, providerKey),
                ValueType.Boolean,
                providerName);

            #endregion

            #region 密码

            var passwordSetting = identitySetting.AddSetting(L["DisplayName:Identity.Password"], L["Description:Identity.Password"]);
            passwordSetting.AddDetail(
                SettingDefinitionManager.Get(IdentitySettingNames.Password.RequireDigit),
                StringLocalizerFactory,
                await SettingManager.GetOrNullAsync(IdentitySettingNames.Password.RequireDigit, providerName, providerKey),
                ValueType.Boolean,
                providerName);
            passwordSetting.AddDetail(
                SettingDefinitionManager.Get(IdentitySettingNames.Password.RequiredLength),
                StringLocalizerFactory,
                await SettingManager.GetOrNullAsync(IdentitySettingNames.Password.RequiredLength, providerName, providerKey),
                ValueType.Number,
                providerName);
            passwordSetting.AddDetail(
                SettingDefinitionManager.Get(IdentitySettingNames.Password.RequiredUniqueChars),
                StringLocalizerFactory,
                await SettingManager.GetOrNullAsync(IdentitySettingNames.Password.RequiredUniqueChars, providerName, providerKey),
                ValueType.Number,
                providerName);
            passwordSetting.AddDetail(
                SettingDefinitionManager.Get(IdentitySettingNames.Password.RequireLowercase),
                StringLocalizerFactory,
                await SettingManager.GetOrNullAsync(IdentitySettingNames.Password.RequireLowercase, providerName, providerKey),
                ValueType.Boolean,
                providerName);
            passwordSetting.AddDetail(
                SettingDefinitionManager.Get(IdentitySettingNames.Password.RequireUppercase),
                StringLocalizerFactory,
                await SettingManager.GetOrNullAsync(IdentitySettingNames.Password.RequireUppercase, providerName, providerKey),
                ValueType.Boolean,
                providerName);
            passwordSetting.AddDetail(
                SettingDefinitionManager.Get(IdentitySettingNames.Password.RequireNonAlphanumeric),
                StringLocalizerFactory,
                await SettingManager.GetOrNullAsync(IdentitySettingNames.Password.RequireNonAlphanumeric, providerName, providerKey),
                ValueType.Boolean,
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
                SettingDefinitionManager.Get(IdentitySettingNames.OrganizationUnit.MaxUserMembershipCount),
                StringLocalizerFactory,
                await SettingManager.GetOrNullAsync(IdentitySettingNames.OrganizationUnit.MaxUserMembershipCount, providerName, providerKey),
                ValueType.Number,
                providerName);

            settingGroups.AddGroup(identitySetting);

            #endregion

            #endregion

            #region 邮件设置

            var emailSettingGroup = new SettingGroupDto(L["DisplayName:Emailing"], L["Description:Emailing"]);
            var defaultMailSetting = emailSettingGroup.AddSetting(L["DisplayName:Emailing.Default"], L["Description:Emailing.Default"]);
            defaultMailSetting.AddDetail(
                SettingDefinitionManager.Get(EmailSettingNames.DefaultFromAddress),
                StringLocalizerFactory,
                await SettingManager.GetOrNullAsync(EmailSettingNames.DefaultFromAddress, providerName, providerKey),
                ValueType.String,
                providerName);
            defaultMailSetting.AddDetail(
                SettingDefinitionManager.Get(EmailSettingNames.DefaultFromDisplayName),
                StringLocalizerFactory,
                await SettingManager.GetOrNullAsync(EmailSettingNames.DefaultFromDisplayName, providerName, providerKey),
                ValueType.String,
                providerName);

            var smtpSetting = emailSettingGroup.AddSetting(L["DisplayName:Emailing.Smtp"], L["Description:Emailing.Smtp"]);
            smtpSetting.AddDetail(
                SettingDefinitionManager.Get(EmailSettingNames.Smtp.EnableSsl),
                StringLocalizerFactory,
                await SettingManager.GetOrNullAsync(EmailSettingNames.Smtp.EnableSsl, providerName, providerKey),
                ValueType.Boolean,
                providerName);
            smtpSetting.AddDetail(
                SettingDefinitionManager.Get(EmailSettingNames.Smtp.UseDefaultCredentials),
                StringLocalizerFactory,
                await SettingManager.GetOrNullAsync(EmailSettingNames.Smtp.UseDefaultCredentials, providerName, providerKey),
                ValueType.Boolean,
                providerName);
            smtpSetting.AddDetail(
                SettingDefinitionManager.Get(EmailSettingNames.Smtp.Domain),
                StringLocalizerFactory,
                await SettingManager.GetOrNullAsync(EmailSettingNames.Smtp.Domain, providerName, providerKey),
                ValueType.String,
                providerName);
            smtpSetting.AddDetail(
                SettingDefinitionManager.Get(EmailSettingNames.Smtp.Host),
                StringLocalizerFactory,
                await SettingManager.GetOrNullAsync(EmailSettingNames.Smtp.Host, providerName, providerKey),
                ValueType.String,
                providerName);
            smtpSetting.AddDetail(
                SettingDefinitionManager.Get(EmailSettingNames.Smtp.Port),
                StringLocalizerFactory,
                await SettingManager.GetOrNullAsync(EmailSettingNames.Smtp.Port, providerName, providerKey),
                ValueType.Number,
                providerName);
            smtpSetting.AddDetail(
                SettingDefinitionManager.Get(EmailSettingNames.Smtp.UserName),
                StringLocalizerFactory,
                await SettingManager.GetOrNullAsync(EmailSettingNames.Smtp.UserName, providerName, providerKey),
                ValueType.String,
                providerName);
            smtpSetting.AddDetail(
                SettingDefinitionManager.Get(EmailSettingNames.Smtp.Password),
                StringLocalizerFactory,
                await SettingManager.GetOrNullAsync(EmailSettingNames.Smtp.Password, providerName, providerKey),
                ValueType.String,
                providerName);

            settingGroups.AddGroup(emailSettingGroup);

            #endregion

            return settingGroups;
        }

        protected virtual async Task CheckFeatureAsync()
        {
            await FeatureChecker.CheckEnabledAsync(SettingManagementFeatures.Enable);
        }
    }
}
