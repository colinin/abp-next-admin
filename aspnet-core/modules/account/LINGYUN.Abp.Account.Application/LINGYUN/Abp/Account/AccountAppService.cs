using LINGYUN.Abp.Identity;
using LINGYUN.Abp.Identity.Security;
using LINGYUN.Abp.Identity.Settings;
using LINGYUN.Abp.WeChat;
using LINGYUN.Abp.WeChat.MiniProgram;
using LINGYUN.Abp.WeChat.OpenId;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.Identity;
using Volo.Abp.Settings;
using Volo.Abp.Validation;
using IIdentityUserRepository = LINGYUN.Abp.Identity.IIdentityUserRepository;

namespace LINGYUN.Abp.Account
{
    public class AccountAppService : AccountApplicationServiceBase, IAccountAppService
    {
        protected ITotpService TotpService { get; }
        protected IIdentityUserRepository UserRepository { get; }
        protected IUserSecurityCodeSender SecurityCodeSender { get; }
        protected IWeChatOpenIdFinder WeChatOpenIdFinder { get; }
        protected AbpWeChatMiniProgramOptionsFactory MiniProgramOptionsFactory { get; }
        protected IDistributedCache<SmsSecurityTokenCacheItem> SecurityTokenCache { get; }

        public AccountAppService(
            ITotpService totpService,
            IWeChatOpenIdFinder weChatOpenIdFinder,
            IIdentityUserRepository userRepository,
            IUserSecurityCodeSender securityCodeSender,
            IDistributedCache<SmsSecurityTokenCacheItem> securityTokenCache,
            AbpWeChatMiniProgramOptionsFactory miniProgramOptionsFactory)
        {
            TotpService = totpService;
            UserRepository = userRepository;
            WeChatOpenIdFinder = weChatOpenIdFinder;
            SecurityCodeSender = securityCodeSender;
            SecurityTokenCache = securityTokenCache;
            MiniProgramOptionsFactory = miniProgramOptionsFactory;
        }

        public virtual async Task RegisterAsync(WeChatRegisterDto input)
        {
            ThowIfInvalidEmailAddress(input.EmailAddress);

            await CheckSelfRegistrationAsync();
            await IdentityOptions.SetAsync();

            var options = await MiniProgramOptionsFactory.CreateAsync();

            var wehchatOpenId = await WeChatOpenIdFinder.FindAsync(input.Code, options.AppId, options.AppSecret);

            var user = await UserManager.FindByLoginAsync(AbpWeChatMiniProgramConsts.ProviderName, wehchatOpenId.OpenId);
            if (user != null)
            {
                // 应该要抛出微信号已注册异常,而不是直接返回注册用户数据,否则造成用户信息泄露
                throw new UserFriendlyException(L["DuplicateWeChat"]);
            }
            var userName = input.UserName;
            if (userName.IsNullOrWhiteSpace())
            {
                userName = "wxid-" + wehchatOpenId.OpenId.ToMd5().ToLower();
            }
            
            var userEmail = input.EmailAddress;//如果邮件地址不验证,随意写入一个
            if (userEmail.IsNullOrWhiteSpace())
            {
                userEmail = $"{userName}@{CurrentTenant.Name ?? "default"}.io";
            }

            user = new IdentityUser(GuidGenerator.Create(), userName, userEmail, CurrentTenant.Id);
            (await UserManager.CreateAsync(user, input.Password)).CheckErrors();

            (await UserManager.AddDefaultRolesAsync(user)).CheckErrors();

            var userLogin = new UserLoginInfo(AbpWeChatMiniProgramConsts.ProviderName, wehchatOpenId.OpenId, AbpWeChatGlobalConsts.DisplayName);
            (await UserManager.AddLoginAsync(user, userLogin)).CheckErrors();

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        public virtual async Task SendPhoneRegisterCodeAsync(SendPhoneRegisterCodeDto input)
        {
            await CheckSelfRegistrationAsync();
            await CheckNewUserPhoneNumberNotBeUsedAsync(input.PhoneNumber);

            var securityTokenCacheKey = SmsSecurityTokenCacheItem.CalculateCacheKey(input.PhoneNumber, "SmsVerifyCode");
            var securityTokenCacheItem = await SecurityTokenCache.GetAsync(securityTokenCacheKey);
            var interval = await SettingProvider.GetAsync(IdentitySettingNames.User.SmsRepetInterval, 1);

            if (securityTokenCacheItem != null)
            {
                throw new UserFriendlyException(L["SendRepeatSmsVerifyCode", interval]);
            }

            var template = await SettingProvider.GetOrNullAsync(IdentitySettingNames.User.SmsNewUserRegister);

            // 安全令牌
            var securityToken = GuidGenerator.Create().ToString("N");

            var code = TotpService.GenerateCode(Encoding.Unicode.GetBytes(securityToken), securityTokenCacheKey);
            securityTokenCacheItem = new SmsSecurityTokenCacheItem(code.ToString(), securityToken);

            await SecurityCodeSender.SendPhoneConfirmedCodeAsync(
                input.PhoneNumber, securityTokenCacheItem.Token, template);

            await SecurityTokenCache
                .SetAsync(securityTokenCacheKey, securityTokenCacheItem,
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(interval)
                    });
        }

        public virtual async Task RegisterAsync(PhoneRegisterDto input)
        {
            await CheckSelfRegistrationAsync();
            await IdentityOptions.SetAsync();
            await CheckNewUserPhoneNumberNotBeUsedAsync(input.PhoneNumber);

            var securityTokenCacheKey = SmsSecurityTokenCacheItem.CalculateCacheKey(input.PhoneNumber, "SmsVerifyCode");
            var securityTokenCacheItem = await SecurityTokenCache.GetAsync(securityTokenCacheKey);
            if (securityTokenCacheItem == null)
            {
                // 验证码过期
                throw new UserFriendlyException(L["InvalidSmsVerifyCode"]);
            }

            // 验证码是否有效
            if (input.Code.Equals(securityTokenCacheItem.Token) && int.TryParse(input.Code, out int token))
            {
                var securityToken = Encoding.Unicode.GetBytes(securityTokenCacheItem.SecurityToken);
                // 校验totp验证码
                if (TotpService.ValidateCode(securityToken, token, securityTokenCacheKey))
                {
                    var userEmail = input.EmailAddress ?? $"{input.PhoneNumber}@{CurrentTenant.Name ?? "default"}.io";//如果邮件地址不验证,随意写入一个
                    var userName = input.UserName ?? input.PhoneNumber;
                    var user = new IdentityUser(GuidGenerator.Create(), userName, userEmail, CurrentTenant.Id)
                    {
                        Name = input.Name ?? input.PhoneNumber
                    };

                    await UserStore.SetPhoneNumberAsync(user, input.PhoneNumber);
                    await UserStore.SetPhoneNumberConfirmedAsync(user, true);

                    (await UserManager.CreateAsync(user, input.Password)).CheckErrors();

                    (await UserManager.AddDefaultRolesAsync(user)).CheckErrors();

                    await SecurityTokenCache.RemoveAsync(securityTokenCacheKey);

                    await CurrentUnitOfWork.SaveChangesAsync();

                    return;
                }
            }
            // 验证码无效
            throw new UserFriendlyException(L["InvalidSmsVerifyCode"]);
        }

        public virtual async Task SendPhoneResetPasswordCodeAsync(SendPhoneResetPasswordCodeDto input)
        {
            /*
             * 注解: 微软的重置密码方法通过 UserManager.GeneratePasswordResetTokenAsync 接口生成密码重置Token
             *       而这个Token设计的意义就是用户通过链接来重置密码,所以不适合短信验证
             *       某些企业是把链接生成一个短链发送短信的,不过这种方式不是很推荐,因为现在是真没几个人敢随便点短信链接的
             *  
             *  此处设计方式为:
             *  
             *  step1: 例行检查是否重复发送,这一点是很有必要的
             *  step2: 通过已确认的手机号来查询用户,如果用户未确认手机号,那就不能发送,这一点也是很有必要的
             *  step3(重点): 通过 UserManager.GenerateTwoFactorTokenAsync 接口来生成二次认证码,这就相当于伪验证码,只是用于确认用户传递的验证码是否通过
             *               比起自己生成随机数,这个验证码利用了TOTP算法,有时间限制的
             *  step4(重点): 用户传递验证码后,通过 UserManager.VerifyTwoFactorTokenAsync 接口来校验验证码
             *               验证通过后,再利用 UserManager.GeneratePasswordResetTokenAsync 接口来生成真正的用于重置密码的Token
            */

            var securityTokenCacheKey = SmsSecurityTokenCacheItem.CalculateCacheKey(input.PhoneNumber, "SmsVerifyCode");
            var securityTokenCacheItem = await SecurityTokenCache.GetAsync(securityTokenCacheKey);
            var interval = await SettingProvider.GetAsync(IdentitySettingNames.User.SmsRepetInterval, 1);
            // 传递 isConfirmed 用户必须是已确认过手机号的
            var user = await GetUserByPhoneNumberAsync(input.PhoneNumber, isConfirmed: true);
            // 能查询到缓存就是重复发送
            if (securityTokenCacheItem != null)
            {
                throw new UserFriendlyException(L["SendRepeatSmsVerifyCode", interval]);
            }

            var template = await SettingProvider.GetOrNullAsync(IdentitySettingNames.User.SmsResetPassword);
            // 生成二次认证码
            var code = await UserManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultPhoneProvider);
            // 发送短信验证码
            await SecurityCodeSender.SendPhoneConfirmedCodeAsync(input.PhoneNumber, code, template);
            // 缓存这个手机号的记录,防重复
            securityTokenCacheItem = new SmsSecurityTokenCacheItem(code, user.SecurityStamp);
            await SecurityTokenCache
                .SetAsync(securityTokenCacheKey, securityTokenCacheItem,
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(interval)
                    });
        }

        public virtual async Task ResetPasswordAsync(PhoneResetPasswordDto input)
        {
            var securityTokenCacheKey = SmsSecurityTokenCacheItem.CalculateCacheKey(input.PhoneNumber, "SmsVerifyCode");
            var securityTokenCacheItem = await SecurityTokenCache.GetAsync(securityTokenCacheKey);
            if (securityTokenCacheItem == null)
            {
                throw new UserFriendlyException(L["InvalidSmsVerifyCode"]);
            }
            await IdentityOptions.SetAsync();
            // 传递 isConfirmed 用户必须是已确认过手机号的
            var user = await GetUserByPhoneNumberAsync(input.PhoneNumber, isConfirmed: true);
            // 验证二次认证码
            if (!await UserManager.VerifyTwoFactorTokenAsync(user, TokenOptions.DefaultPhoneProvider, input.Code))
            {
                // 验证码无效
                throw new UserFriendlyException(L["InvalidSmsVerifyCode"]);
            }
            // 生成真正的重置密码Token
            var resetPwdToken = await UserManager.GeneratePasswordResetTokenAsync(user);
            // 重置密码
            (await UserManager.ResetPasswordAsync(user, resetPwdToken, input.NewPassword)).CheckErrors();
            // 移除缓存项
            await SecurityTokenCache.RemoveAsync(securityTokenCacheKey);

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        public virtual async Task SendPhoneSigninCodeAsync(SendPhoneSigninCodeDto input)
        {
            var securityTokenCacheKey = SmsSecurityTokenCacheItem.CalculateCacheKey(input.PhoneNumber, "SmsVerifyCode");
            var securityTokenCacheItem = await SecurityTokenCache.GetAsync(securityTokenCacheKey);
            var interval = await SettingProvider.GetAsync(IdentitySettingNames.User.SmsRepetInterval, 1);
            if (securityTokenCacheItem != null)
            {
                throw new UserFriendlyException(L["SendRepeatSmsVerifyCode", interval]);
            }
            // 传递 isConfirmed 验证过的用户才允许通过手机登录
            var user = await GetUserByPhoneNumberAsync(input.PhoneNumber, isConfirmed: true);
            var code = await UserManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultPhoneProvider);
            var template = await SettingProvider.GetOrNullAsync(IdentitySettingNames.User.SmsUserSignin);

            // 发送登录验证码短信
            await SecurityCodeSender.SendPhoneConfirmedCodeAsync(input.PhoneNumber, code, template);
            // 缓存登录验证码状态,防止同一手机号重复发送
            securityTokenCacheItem = new SmsSecurityTokenCacheItem(code, user.SecurityStamp);
            await SecurityTokenCache
                .SetAsync(securityTokenCacheKey, securityTokenCacheItem,
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(interval)
                    });
        }

        protected virtual async Task<IdentityUser> GetUserByPhoneNumberAsync(string phoneNumber, bool isConfirmed = true)
        {
            var user = await UserRepository.FindByPhoneNumberAsync(phoneNumber, isConfirmed, true);
            if (user == null)
            {
                throw new UserFriendlyException(L["PhoneNumberNotRegisterd"]);
            }
            return user;
        }

        /// <summary>
        /// 检查是否允许用户注册
        /// </summary>
        /// <returns></returns>
        protected virtual async Task CheckSelfRegistrationAsync()
        {
            if (!await SettingProvider.IsTrueAsync(Volo.Abp.Account.Settings.AccountSettingNames.IsSelfRegistrationEnabled))
            {
                throw new UserFriendlyException(L["SelfRegistrationDisabledMessage"]);
            }
        }

        protected virtual async Task CheckNewUserPhoneNumberNotBeUsedAsync(string phoneNumber)
        {
            if (await UserRepository.IsPhoneNumberUedAsync(phoneNumber))
            {
                throw new UserFriendlyException(L["DuplicatePhoneNumber"]);
            }
        }

        private void ThowIfInvalidEmailAddress(string inputEmail)
        {
            if (!inputEmail.IsNullOrWhiteSpace() &&
                !ValidationHelper.IsValidEmailAddress(inputEmail))
            {
                throw new AbpValidationException(
                    new ValidationResult[]
                    {
                        new ValidationResult(L["The {0} field is not a valid e-mail address.", L["DisplayName:EmailAddress"]], new string[]{ "EmailAddress" })
                    });
            }
        }
    }
}
