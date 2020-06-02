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
        protected ISmsSender SmsSender { get; }
        protected IdentityUserManager UserManager { get; }
        protected IIdentityUserRepository UserRepository { get; }
        protected IDistributedCache<AccountRegisterVerifyCacheItem> Cache { get; }
        protected PhoneNumberTokenProvider<IdentityUser> PhoneNumberTokenProvider { get; }
        public AccountAppService(
            ISmsSender smsSender,
            IdentityUserManager userManager,
            IIdentityUserRepository userRepository,
            IDistributedCache<AccountRegisterVerifyCacheItem> cache,
            PhoneNumberTokenProvider<IdentityUser> phoneNumberTokenProvider)
        {
            Cache = cache;
            SmsSender = smsSender;
            UserManager = userManager;
            UserRepository = userRepository;
            PhoneNumberTokenProvider = phoneNumberTokenProvider;
            LocalizationResource = typeof(Localization.AccountResource);
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
        public virtual async Task<IdentityUserDto> RegisterAsync(RegisterVerifyDto input)
        {
            var phoneVerifyCacheKey = NormalizeCacheKey(input.PhoneNumber);

            var phoneVerifyCacheItem = await Cache.GetAsync(phoneVerifyCacheKey);
            if(phoneVerifyCacheItem == null || !phoneVerifyCacheItem.VerifyCode.Equals(input.VerifyCode))
            {
                throw new UserFriendlyException(L["PhoneVerifyCodeInvalid"]);
            }

            await CheckSelfRegistrationAsync();

            var userEmail = input.EmailAddress ?? input.PhoneNumber + "@abp.io";
            var userName = input.UserName ?? input.PhoneNumber;
            var user = new IdentityUser(GuidGenerator.Create(), userName, userEmail, CurrentTenant.Id)
            {
                Name = input.Name ?? input.PhoneNumber
            };
            (await UserManager.CreateAsync(user, input.Password)).CheckErrors();

            (await UserManager.SetPhoneNumberAsync(user, input.PhoneNumber)).CheckErrors();
            (await UserManager.SetEmailAsync(user, userEmail)).CheckErrors();
            (await UserManager.AddDefaultRolesAsync(user)).CheckErrors();

            await Cache.RemoveAsync(phoneVerifyCacheKey);

            return ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);
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
            if (input.VerifyType == PhoneNumberVerifyType.Register)
            {
                var phoneVerifyCode = new Random().Next(100000, 999999);
                verifyCacheItem.VerifyCode = phoneVerifyCode.ToString();
                var templateCode = await SettingProvider.GetOrNullAsync(AccountSettingNames.SmsRegisterTemplateCode);
                await SendPhoneVerifyMessageAsync(templateCode, input.PhoneNumber, phoneVerifyCode.ToString());
            }
            else
            {
                var phoneVerifyCode = await SendSigninVerifyCodeAsync(input.PhoneNumber);
                verifyCacheItem.VerifyCode = phoneVerifyCode;
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
            var user = await UserRepository.FindByPhoneNumberAsync(phoneNumber);
            if (user == null)
            {
                throw new UserFriendlyException(L["PhoneNumberNotRegisterd"]);
            }
            // 获取登录验证码模板号
            var templateCode = await SettingProvider.GetOrNullAsync(AccountSettingNames.SmsSigninTemplateCode);
            // 生成手机验证码
            var phoneVerifyCode = await PhoneNumberTokenProvider.GenerateAsync("phone_verify", UserManager, user);
            // 发送短信验证码
            await SendPhoneVerifyMessageAsync(templateCode, user.PhoneNumber, phoneVerifyCode);

            return phoneVerifyCode;
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
