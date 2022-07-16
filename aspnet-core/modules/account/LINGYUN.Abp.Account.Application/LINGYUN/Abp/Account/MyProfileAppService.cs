using LINGYUN.Abp.Account.Emailing;
using LINGYUN.Abp.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Account.Localization;
using Volo.Abp.Caching;
using Volo.Abp.Identity;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.Account
{
    [Authorize]
    public class MyProfileAppService : AccountApplicationServiceBase, IMyProfileAppService
    {
        protected IDistributedCache<SecurityTokenCacheItem> SecurityTokenCache { get; }
        protected IAccountSmsSecurityCodeSender SecurityCodeSender { get; }
        protected Identity.IIdentityUserRepository UserRepository { get; }
        protected IdentitySecurityLogManager IdentitySecurityLogManager { get; }

        public MyProfileAppService(
            Identity.IIdentityUserRepository userRepository,
            IAccountSmsSecurityCodeSender securityCodeSender,
            IdentitySecurityLogManager identitySecurityLogManager,
            IDistributedCache<SecurityTokenCacheItem> securityTokenCache)
        {
            UserRepository = userRepository;
            SecurityCodeSender = securityCodeSender;
            IdentitySecurityLogManager = identitySecurityLogManager;
            SecurityTokenCache = securityTokenCache;

            LocalizationResource = typeof(AccountResource);
        }

        public virtual async Task<TwoFactorEnabledDto> GetTwoFactorEnabledAsync()
        {
            var user = await GetCurrentUserAsync();

            return new TwoFactorEnabledDto
            {
                Enabled = await UserManager.GetTwoFactorEnabledAsync(user),
            };
        }

        public virtual async Task ChangeTwoFactorEnabledAsync(TwoFactorEnabledDto input)
        {
            // Removed See: https://github.com/abpframework/abp/pull/7719
            //if (!await SettingProvider.IsTrueAsync(IdentitySettingNames.TwoFactor.UsersCanChange))
            //{
            //    throw new BusinessException(Volo.Abp.Identity.IdentityErrorCodes.CanNotChangeTwoFactor);
            //}
            // TODO: Abp官方移除了双因素的设置,不排除以后会增加,如果在用户接口中启用了双因素认证,可能造成登录失败!
            var user = await GetCurrentUserAsync();

            (await UserManager.SetTwoFactorEnabledWithAccountConfirmedAsync(user, input.Enabled)).CheckErrors();

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        public virtual async Task SendChangePhoneNumberCodeAsync(SendChangePhoneNumberCodeInput input)
        {
            var securityTokenCacheKey = SecurityTokenCacheItem.CalculateSmsCacheKey(input.NewPhoneNumber, "SmsChangePhoneNumber");
            var securityTokenCacheItem = await SecurityTokenCache.GetAsync(securityTokenCacheKey);
            var interval = await SettingProvider.GetAsync(Identity.Settings.IdentitySettingNames.User.SmsRepetInterval, 1);
            if (securityTokenCacheItem != null)
            {
                throw new UserFriendlyException(L["SendRepeatPhoneVerifyCode", interval]);
            }

            // 是否已有用户使用手机号绑定
            if (await UserRepository.IsPhoneNumberConfirmedAsync(input.NewPhoneNumber))
            {
                throw new BusinessException(Identity.IdentityErrorCodes.DuplicatePhoneNumber);
            }
            var user = await GetCurrentUserAsync();

            var template = await SettingProvider.GetOrNullAsync(Identity.Settings.IdentitySettingNames.User.SmsPhoneNumberConfirmed);
            var token = await UserManager.GenerateChangePhoneNumberTokenAsync(user, input.NewPhoneNumber);
            // 发送验证码
            await SecurityCodeSender.SendSmsCodeAsync(input.NewPhoneNumber, token, template);

            securityTokenCacheItem = new SecurityTokenCacheItem(token, user.ConcurrencyStamp);
            await SecurityTokenCache
                .SetAsync(securityTokenCacheKey, securityTokenCacheItem,
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(interval)
                    });
        }

        public virtual async Task ChangePhoneNumberAsync(ChangePhoneNumberInput input)
        {
            // 是否已有用户使用手机号绑定
            if (await UserRepository.IsPhoneNumberConfirmedAsync(input.NewPhoneNumber))
            {
                throw new BusinessException(Identity.IdentityErrorCodes.DuplicatePhoneNumber);
            }
            var user = await GetCurrentUserAsync();
            // 更换手机号
            (await UserManager.ChangePhoneNumberAsync(user, input.NewPhoneNumber, input.Code)).CheckErrors();

            await CurrentUnitOfWork.SaveChangesAsync();

            var securityTokenCacheKey = SecurityTokenCacheItem.CalculateSmsCacheKey(input.NewPhoneNumber, "SmsChangePhoneNumber");
            await SecurityTokenCache.RemoveAsync(securityTokenCacheKey);
        }

        public async virtual Task SendEmailConfirmLinkAsync(SendEmailConfirmCodeDto input)
        {
            var user = await UserManager.FindByEmailAsync(input.Email);

            if (user == null)
            {
                throw new UserFriendlyException(L["Volo.Account:InvalidEmailAddress", input.Email]);
            }

            if (user.EmailConfirmed)
            {
                throw new BusinessException(Identity.IdentityErrorCodes.DuplicateConfirmEmailAddress);
            }

            var token = await UserManager.GenerateEmailConfirmationTokenAsync(user);
            var sender = LazyServiceProvider.LazyGetRequiredService<IAccountEmailConfirmSender>();

            await sender.SendEmailConfirmLinkAsync(
                user,
                token,
                input.AppName,
                input.ReturnUrl,
                input.ReturnUrlHash);
        }

        public async virtual Task ConfirmEmailAsync(ConfirmEmailInput input)
        {
            await IdentityOptions.SetAsync();

            var user = await UserManager.GetByIdAsync(input.UserId);

            (await UserManager.ConfirmEmailAsync(user, input.ConfirmToken)).CheckErrors();

            await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext
            {
                Identity = IdentitySecurityLogIdentityConsts.Identity,
                Action = "ConfirmEmail"
            });
        }
    }
}
