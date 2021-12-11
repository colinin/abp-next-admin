using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.Caching;
using Volo.Abp.Emailing;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Features;
using Volo.Abp.Identity.Settings;
using Volo.Abp.Localization;
using Volo.Abp.SettingManagement;
using Volo.Abp.SettingManagement.Localization;
using Volo.Abp.Settings;
using Volo.Abp.Timing;
using Volo.Abp.Users;

namespace LINGYUN.Abp.SettingManagement
{
    [Authorize]
    public class UserSettingAppService : ApplicationService, IUserSettingAppService
    {
        protected AbpLocalizationOptions LocalizationOptions { get; }

        protected IDistributedEventBus EventBus { get; }
        protected ISettingManager SettingManager { get; }
        protected ISettingDefinitionManager SettingDefinitionManager { get; }

        protected IDistributedCache<SettingCacheItem> Cache { get; }
        public UserSettingAppService(
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

        public virtual async Task SetCurrentUserAsync(UpdateSettingsDto input)
        {
            // 增加特性检查
            await CheckFeatureAsync();

            foreach (var setting in input.Settings)
            {
                await SettingManager.SetForCurrentUserAsync(setting.Name, setting.Value);
            }

            CurrentUnitOfWork.OnCompleted(async () =>
            {
                // 发送刷新用户缓存事件
                await EventBus.PublishAsync(new CurrentApplicationConfigurationCacheResetEventData());
            });

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        public virtual async Task<SettingGroupResult> GetAllForCurrentUserAsync()
        {
            return await GetAllForProviderAsync(UserSettingValueProvider.ProviderName, CurrentUser.GetId().ToString());
        }

        protected virtual async Task<SettingGroupResult> GetAllForProviderAsync(string providerName, string providerKey)
        {
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

            #region 身份标识设置

            // 身份标识设置
            var identitySetting = new SettingGroupDto(L["DisplayName:Identity"], L["Description:Identity"]);


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

            settingGroups.AddGroup(identitySetting);

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
