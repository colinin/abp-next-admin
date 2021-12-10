using LINGYUN.Abp.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Account.Localization;
using Volo.Abp.Caching;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.Account
{
    [Authorize]
    public class MyProfileAppService : AccountApplicationServiceBase, IMyProfileAppService
    {
        protected IDistributedCache<SmsSecurityTokenCacheItem> SecurityTokenCache { get; }
        protected IUserSecurityCodeSender SecurityCodeSender { get; }
        protected Identity.IIdentityUserRepository UserRepository { get; }

        public MyProfileAppService(
            Identity.IIdentityUserRepository userRepository,
            IUserSecurityCodeSender securityCodeSender,
            IDistributedCache<SmsSecurityTokenCacheItem> securityTokenCache)
        {
            UserRepository = userRepository;
            SecurityCodeSender = securityCodeSender;
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
            var securityTokenCacheKey = SmsSecurityTokenCacheItem.CalculateCacheKey(input.NewPhoneNumber, "SmsChangePhoneNumber");
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
            await SecurityCodeSender.SendPhoneConfirmedCodeAsync(input.NewPhoneNumber, token, template);

            securityTokenCacheItem = new SmsSecurityTokenCacheItem(token, user.ConcurrencyStamp);
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

            var securityTokenCacheKey = SmsSecurityTokenCacheItem.CalculateCacheKey(input.NewPhoneNumber, "SmsChangePhoneNumber");
            await SecurityTokenCache.RemoveAsync(securityTokenCacheKey);
        }
    }
}
