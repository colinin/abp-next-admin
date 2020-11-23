using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Settings;
using Volo.Abp.Settings;
using Volo.Abp.Users;

namespace LINGYUN.Abp.Identity
{
    [Authorize]
    public class MyProfileAppService : IdentityAppServiceBase, IMyProfileAppService
    {
        protected IDistributedCache<SmsSecurityTokenCacheItem> SecurityTokenCache { get; }
        protected IUserSecurityCodeSender SecurityCodeSender { get; }
        protected IdentityUserManager UserManager { get; }
        protected IIdentityUserRepository UserRepository { get; }

        public MyProfileAppService(
            IdentityUserManager userManager,
            IIdentityUserRepository userRepository,
            IUserSecurityCodeSender securityCodeSender,
            IDistributedCache<SmsSecurityTokenCacheItem> securityTokenCache)
        {
            UserManager = userManager;
            UserRepository = userRepository;
            SecurityCodeSender = securityCodeSender;
            SecurityTokenCache = securityTokenCache;
        }

        public virtual async Task ChangeTwoFactorEnabledAsync(ChangeTwoFactorEnabledDto input)
        {
            if (!await SettingProvider.IsTrueAsync(IdentitySettingNames.TwoFactor.UsersCanChange))
            {
                throw new BusinessException(Volo.Abp.Identity.IdentityErrorCodes.CanNotChangeTwoFactor);
            }

            var user = await UserManager.GetByIdAsync(CurrentUser.GetId());

            (await UserManager.SetTwoFactorEnabledWithAccountConfirmedAsync(user, input.Enabled)).CheckErrors();

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        public virtual async Task SendChangePhoneNumberCodeAsync(SendChangePhoneNumberCodeDto input)
        {
            var securityTokenCacheKey = SmsSecurityTokenCacheItem.CalculateCacheKey(input.NewPhoneNumber, "SmsChangePhoneNumber");
            var securityTokenCacheItem = await SecurityTokenCache.GetAsync(securityTokenCacheKey);
            var interval = await SettingProvider.GetAsync(Settings.IdentitySettingNames.User.SmsRepetInterval, 1);
            if (securityTokenCacheItem != null)
            {
                throw new UserFriendlyException(L["SendRepeatPhoneVerifyCode", interval]);
            }

            // 是否已有用户使用手机号绑定
            if (await UserRepository.IsPhoneNumberConfirmedAsync(input.NewPhoneNumber))
            {
                throw new BusinessException(IdentityErrorCodes.DuplicatePhoneNumber);
            }
            var user = await UserManager.GetByIdAsync(CurrentUser.GetId());
            var template = await SettingProvider.GetOrNullAsync(Settings.IdentitySettingNames.User.SmsPhoneNumberConfirmed);
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

        public virtual async Task ChangePhoneNumberAsync(ChangePhoneNumberDto input)
        {
            // 是否已有用户使用手机号绑定
            if (await UserRepository.IsPhoneNumberConfirmedAsync(input.NewPhoneNumber))
            {
                throw new BusinessException(IdentityErrorCodes.DuplicatePhoneNumber);
            }
            //TODO: 可以查询缓存用 securityTokenCacheItem.SecurityToken 与 user.SecurityStamp 作对比
            var user = await UserManager.GetByIdAsync(CurrentUser.GetId());
            // 更换手机号
            (await UserManager.ChangePhoneNumberAsync(user, input.NewPhoneNumber, input.Code)).CheckErrors();

            await CurrentUnitOfWork.SaveChangesAsync();

            var securityTokenCacheKey = SmsSecurityTokenCacheItem.CalculateCacheKey(input.NewPhoneNumber, "SmsChangePhoneNumber");
            await SecurityTokenCache.RemoveAsync(securityTokenCacheKey);
        }
    }
}
