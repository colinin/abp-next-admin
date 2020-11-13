using LINGYUN.Abp.WeChat.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;
using Volo.Abp.Identity;
using Volo.Abp.Settings;
using Volo.Abp.Sms;

namespace LINGYUN.Abp.Account
{
    /// <summary>
    /// 用户注册服务
    /// </summary>
    public class AccountAppService : ApplicationService, IAccountAppService
    {
        private IWeChatOpenIdFinder _weChatOpenIdFinder;
        protected IWeChatOpenIdFinder WeChatOpenIdFinder => LazyGetRequiredService(ref _weChatOpenIdFinder);
        protected ISmsSender SmsSender { get; }
        protected IdentityUserManager UserManager { get; }
        protected IdentityUserStore UserStore { get; }
        protected IIdentityUserRepository UserRepository { get; }
        protected IDistributedCache<AccountRegisterVerifyCacheItem> Cache { get; }
        protected PhoneNumberTokenProvider<IdentityUser> PhoneNumberTokenProvider { get; }
        public AccountAppService(
            ISmsSender smsSender,
            IdentityUserManager userManager,
            IdentityUserStore userStore,
            IIdentityUserRepository userRepository,
            IDistributedCache<AccountRegisterVerifyCacheItem> cache,
            PhoneNumberTokenProvider<IdentityUser> phoneNumberTokenProvider)
        {
            Cache = cache;
            SmsSender = smsSender;
            UserStore = userStore;
            UserManager = userManager;
            UserRepository = userRepository;
            PhoneNumberTokenProvider = phoneNumberTokenProvider;
            LocalizationResource = typeof(Localization.AccountResource);
        }

        public virtual async Task<IdentityUserDto> RegisterAsync(WeChatRegisterDto input)
        {
            await CheckSelfRegistrationAsync();

            var wehchatOpenId = await WeChatOpenIdFinder.FindAsync(input.Code);

            var user = await UserManager.FindByLoginAsync(AbpWeChatAuthorizationConsts.ProviderKey, wehchatOpenId.OpenId);
            if (user != null)
            {
                // 应该要抛出微信号已注册异常,而不是直接返回注册用户数据,否则造成用户信息泄露
                throw new UserFriendlyException(L["DuplicateWeChat"]);
            }
            var userName = input.UserName ?? "wx-" + wehchatOpenId.OpenId;
            var userEmail = input.EmailAddress ?? $"{userName}@{CurrentTenant.Name ?? "default"}.io";//如果邮件地址不验证,随意写入一个

            user = new IdentityUser(GuidGenerator.Create(), userName, userEmail, CurrentTenant.Id)
            {
                Name = input.Name ?? userName
            };
            (await UserManager.CreateAsync(user, input.Password)).CheckErrors();

            (await UserManager.AddDefaultRolesAsync(user)).CheckErrors();

            var userLogin = new UserLoginInfo(AbpWeChatAuthorizationConsts.ProviderKey, wehchatOpenId.OpenId, AbpWeChatAuthorizationConsts.DisplayName);
            (await UserManager.AddLoginAsync(user, userLogin)).CheckErrors();

            return ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);
        }
        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <remarks>
        /// 用户通过VerifyPhoneNumber接口发送到手机的验证码,传递注册信息注册用户
        ///     如果没有此手机号的缓存记录或验证码不匹配,抛出验证码无效的异常
        ///     用户注册成功,清除缓存的验证码记录
        /// </remarks>
        public virtual async Task<IdentityUserDto> RegisterAsync(PhoneNumberRegisterDto input)
        {
            var phoneVerifyCacheKey = NormalizeCacheKey(input.PhoneNumber);

            var phoneVerifyCacheItem = await Cache.GetAsync(phoneVerifyCacheKey);
            if(phoneVerifyCacheItem == null || !phoneVerifyCacheItem.VerifyCode.Equals(input.VerifyCode))
            {
                throw new UserFriendlyException(L["PhoneVerifyCodeInvalid"]);
            }

            await CheckSelfRegistrationAsync();

            // 需要用户输入邮箱?
            //if (UserManager.Options.User.RequireUniqueEmail)
            //{
            //    if (input.EmailAddress.IsNullOrWhiteSpace())
            //    {
            //        throw new UserFriendlyException(L["RequiredEmailAddress"]);
            //    }
            //}

            var userEmail = input.EmailAddress ?? $"{input.PhoneNumber}@{CurrentTenant.Name ?? "default"}.io";//如果邮件地址不验证,随意写入一个
            var userName = input.UserName ?? input.PhoneNumber;
            var user = new IdentityUser(GuidGenerator.Create(), userName, userEmail, CurrentTenant.Id)
            {
                Name = input.Name ?? input.PhoneNumber
            };
            // 写入手机号要在创建用户之前,因为有一个自定义的手机号验证
            await UserStore.SetPhoneNumberAsync(user, input.PhoneNumber);
            await UserStore.SetPhoneNumberConfirmedAsync(user, true);

            (await UserManager.CreateAsync(user, input.Password)).CheckErrors();

            (await UserManager.AddDefaultRolesAsync(user)).CheckErrors();

            await Cache.RemoveAsync(phoneVerifyCacheKey);

            return ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);
        }

        // TODO: 是否有必要移动到ProfileService
        /// <summary>
        /// 重置用户密码
        /// </summary>
        /// <param name="passwordReset"></param>
        /// <returns></returns>
        public virtual async Task ResetPasswordAsync(PasswordResetDto passwordReset)
        {
            // 本来可以不需要的，令牌算法有一个有效期
            // 不过这里采用令牌强制过期策略,避免一个令牌多次使用
            var phoneVerifyCacheKey = NormalizeCacheKey(passwordReset.PhoneNumber);

            var phoneVerifyCacheItem = await Cache.GetAsync(phoneVerifyCacheKey);
            if (phoneVerifyCacheItem == null || !phoneVerifyCacheItem.VerifyCode.Equals(passwordReset.VerifyCode))
            {
                throw new UserFriendlyException(L["PhoneVerifyCodeInvalid"]);
            }

            var userId = await GetUserIdByPhoneNumberAsync(passwordReset.PhoneNumber);

            var user = await UserManager.GetByIdAsync(userId);

            (await UserManager.ResetPasswordAsync(user, phoneVerifyCacheItem.VerifyToken, passwordReset.NewPassword)).CheckErrors();


            await Cache.RemoveAsync(phoneVerifyCacheKey);
        }
        /// <summary>
        /// 验证手机号码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <remarks>
        /// 用户传递手机号码及认证类型
        /// 1、如果认证类型为注册：
        ///     先查询是否存在此手机号的缓存验证码信息,如果存在,抛出不能重复发送验证码异常
        ///     随机生成6位纯数字验证码,通过短信接口服务发送到用户手机,并缓存验证码,设定一个有效时长
        ///     
        /// 2、如果认证类型为登录：
        ///     先查询是否存在此手机号的缓存验证码信息,如果存在,抛出不能重复发送验证码异常
        ///     通过手机号查询用户信息,如果用户不存在,抛出手机号未注册异常
        ///     调用PhoneNumberTokenProvider接口生成6位手机验证码,用途为 phone_verify
        ///     发送手机验证码到用户手机,并缓存验证码,设定一个有效时长
        ///     
        ///     用户调用 IdentityServer4/connect/token 登录系统（需要引用LINGYUN.Abp.IdentityServer.SmsValidator模块）
        ///         参数1:grant_type=phone_verify 
        ///         参数2:phone_number=手机号码
        ///         参数3:phone_verify_code=手机验证码
        ///         参数4:client_id=客户端标识
        ///         参数5:client_secret=客户端密钥
        /// </remarks>
        public virtual async Task VerifyPhoneNumberAsync(VerifyDto input)
        {
            // TODO: 借用TOTP算法生成6位动态验证码

            var verifyCodeExpiration = await SettingProvider.GetAsync<int>(AccountSettingNames.PhoneVerifyCodeExpiration);
            var phoneVerifyCacheKey = NormalizeCacheKey(input.PhoneNumber);
            var verifyCacheItem = await Cache.GetAsync(phoneVerifyCacheKey);
            if (verifyCacheItem != null)
            {
                throw new UserFriendlyException(L["PhoneVerifyCodeNotRepeatSend", verifyCodeExpiration]);
            }
            verifyCacheItem = new AccountRegisterVerifyCacheItem
            {
                PhoneNumber = input.PhoneNumber,
            };
            switch (input.VerifyType)
            {
                case PhoneNumberVerifyType.Register:
                    var phoneVerifyCode = new Random().Next(100000, 999999);
                    verifyCacheItem.VerifyCode = phoneVerifyCode.ToString();
                    var templateCode = await SettingProvider.GetOrDefaultAsync(AccountSettingNames.SmsRegisterTemplateCode, ServiceProvider);
                    await SendPhoneVerifyMessageAsync(templateCode, input.PhoneNumber, phoneVerifyCode.ToString());
                    break;
                case PhoneNumberVerifyType.Signin:
                    var phoneSigninCode = await SendSigninVerifyCodeAsync(input.PhoneNumber);
                    verifyCacheItem.VerifyCode = phoneSigninCode;
                    break;
                case PhoneNumberVerifyType.ResetPassword:
                    var resetPasswordCode = new Random().Next(100000, 999999);
                    verifyCacheItem.VerifyCode = resetPasswordCode.ToString();
                    var resetPasswordToken = await SendResetPasswordVerifyCodeAsync(input.PhoneNumber, verifyCacheItem.VerifyCode);
                    verifyCacheItem.VerifyToken = resetPasswordToken;
                    break;
            }
            
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(verifyCodeExpiration)
            };
            await Cache.SetAsync(phoneVerifyCacheKey, verifyCacheItem, cacheOptions);
        }

        /// <summary>
        /// 发送登录验证码
        /// </summary>
        /// <param name="phoneNumber">手机号</param>
        /// <returns>返回登录验证码</returns>
        protected virtual async Task<string> SendSigninVerifyCodeAsync(string phoneNumber)
        {
            // 查找用户信息
            var user = await GetUserByPhoneNumberAsync(phoneNumber);
            // 获取登录验证码模板号
            var templateCode = await SettingProvider.GetOrDefaultAsync(AccountSettingNames.SmsSigninTemplateCode, ServiceProvider);
            // 生成手机验证码
            var phoneVerifyCode = await PhoneNumberTokenProvider.GenerateAsync("phone_verify", UserManager, user);
            // 发送短信验证码
            await SendPhoneVerifyMessageAsync(templateCode, user.PhoneNumber, phoneVerifyCode);

            return phoneVerifyCode;
        }
         
        protected virtual async Task<string> SendResetPasswordVerifyCodeAsync(string phoneNumber, string phoneVerifyCode)
        {
            // 查找用户信息
            var user = await GetUserByPhoneNumberAsync(phoneNumber);
            // 获取登录验证码模板号
            var templateCode = await SettingProvider.GetOrDefaultAsync(AccountSettingNames.SmsResetPasswordTemplateCode, ServiceProvider);
            // 生成重置密码验证码
            var phoneVerifyToken = await UserManager.GeneratePasswordResetTokenAsync(user);
            // 发送短信验证码
            await SendPhoneVerifyMessageAsync(templateCode, user.PhoneNumber, phoneVerifyCode);

            return phoneVerifyToken;
        }

        protected virtual async Task<IdentityUser> GetUserByPhoneNumberAsync(string phoneNumber)
        {
            // 查找用户信息
            var user = await UserRepository.FindByPhoneNumberAsync(phoneNumber);
            if (user == null)
            {
                throw new UserFriendlyException(L["PhoneNumberNotRegisterd"]);
            }
            return user;
        }

        protected virtual async Task<Guid> GetUserIdByPhoneNumberAsync(string phoneNumber)
        {
            // 查找用户信息
            var userId = await UserRepository.GetIdByPhoneNumberAsync(phoneNumber);
            if (!userId.HasValue)
            {
                throw new UserFriendlyException(L["PhoneNumberNotRegisterd"]);
            }
            return userId.Value;
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
        /// <summary>
        /// 发送短信验证码
        /// </summary>
        /// <param name="templateCode"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="verifyCode"></param>
        /// <returns></returns>
        protected virtual async Task SendPhoneVerifyMessageAsync(string templateCode, string phoneNumber, string verifyCode)
        {
            var sendMessage = new SmsMessage(phoneNumber, "SendSmsMessage");
            sendMessage.Properties.Add("code", verifyCode);
            sendMessage.Properties.Add("TemplateCode", templateCode);
            await SmsSender.SendAsync(sendMessage);
        }
        /// <summary>
        /// 格式化缓存主键
        /// </summary>
        /// <param name="phoneNumber">手机号码</param>
        /// <returns></returns>
        protected string NormalizeCacheKey(string phoneNumber)
        {
            return $"ACCOUNT-PHONE:{phoneNumber}";
        }
    }
}
