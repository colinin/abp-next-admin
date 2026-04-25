using LINGYUN.Abp.Account.Security;
using LINGYUN.Abp.Identity;
using LINGYUN.Abp.Identity.Settings;
using LINGYUN.Abp.WeChat;
using LINGYUN.Abp.WeChat.MiniProgram;
using LINGYUN.Abp.WeChat.OpenId;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Caching;
using Volo.Abp.Clients;
using Volo.Abp.Identity;
using Volo.Abp.Settings;
using Volo.Abp.Validation;
using IIdentityUserRepository = LINGYUN.Abp.Identity.IIdentityUserRepository;

namespace LINGYUN.Abp.Account;

public class AccountAppService : AccountApplicationServiceBase, IAccountAppService
{
    protected IIdentityUserRepository UserRepository { get; }
    protected IAccountSmsSecurityCodeSender SecurityCodeSender { get; }
    protected IWeChatOpenIdFinder WeChatOpenIdFinder { get; }
    protected IdentitySecurityLogManager IdentitySecurityLogManager { get; }
    protected AbpWeChatMiniProgramOptionsFactory MiniProgramOptionsFactory { get; }
    protected IDistributedCache<SecurityTokenCacheItem> SecurityTokenCache { get; }

    public AccountAppService(
        IWeChatOpenIdFinder weChatOpenIdFinder,
        IIdentityUserRepository userRepository,
        IAccountSmsSecurityCodeSender securityCodeSender,
        IDistributedCache<SecurityTokenCacheItem> securityTokenCache,
        AbpWeChatMiniProgramOptionsFactory miniProgramOptionsFactory,
        IdentitySecurityLogManager identitySecurityLogManager)
    {
        UserRepository = userRepository;
        WeChatOpenIdFinder = weChatOpenIdFinder;
        SecurityCodeSender = securityCodeSender;
        SecurityTokenCache = securityTokenCache;
        MiniProgramOptionsFactory = miniProgramOptionsFactory;
        IdentitySecurityLogManager = identitySecurityLogManager;
    }

    public async virtual Task RegisterAsync(WeChatRegisterDto input)
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

        await IdentitySecurityLogManager.SaveAsync(
            new IdentitySecurityLogContext
            {
                Action = "WeChatRegister",
                ClientId = await FindClientIdAsync(),
                Identity = "Account",
                UserName = user.UserName
            });

        await CurrentUnitOfWork.SaveChangesAsync();
    }

    public async virtual Task SendPhoneRegisterCodeAsync(SendPhoneRegisterCodeDto input)
    {
        await CheckSelfRegistrationAsync();
        await CheckNewUserPhoneNumberNotBeUsedAsync(input.PhoneNumber);

        var securityTokenCacheKey = SecurityTokenCacheItem.CalculateSmsCacheKey(
            input.PhoneNumber, 
            UserTwoFactorTokenProviderConsts.PhoneNumberRegisterPurpose);
        var securityTokenCacheItem = await SecurityTokenCache.GetAsync(securityTokenCacheKey);
        var interval = await SettingProvider.GetAsync(IdentitySettingNames.User.SmsRepetInterval, 1);

        if (securityTokenCacheItem != null)
        {
            throw new UserFriendlyException(L["SendRepeatSmsVerifyCode", interval]);
        }

        var template = await SettingProvider.GetOrNullAsync(IdentitySettingNames.User.SmsNewUserRegister);

        var tempNewUser = new IdentityUser(
            GuidGenerator.Create(),
            "TempUser",
            "TempUser@abp.io",
            CurrentTenant.Id);
        tempNewUser.SetPhoneNumberRegisterUser();

        (await UserManager.UpdateSecurityStampAsync(tempNewUser)).CheckErrors();

        var code = await UserManager.GenerateUserTokenAsync(
            tempNewUser, 
            UserTwoFactorTokenProviderConsts.PhoneNumberRegisterTokenProvider, 
            UserTwoFactorTokenProviderConsts.PhoneNumberRegisterPurpose);

        securityTokenCacheItem = new SecurityTokenCacheItem(code, tempNewUser.Id, await UserManager.GetSecurityStampAsync(tempNewUser));

        await SecurityCodeSender.SendAsync(
            input.PhoneNumber, securityTokenCacheItem.Token, template);

        await SecurityTokenCache
            .SetAsync(securityTokenCacheKey, securityTokenCacheItem,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(interval)
                });
    }

    public async virtual Task RegisterAsync(PhoneRegisterDto input)
    {
        await CheckSelfRegistrationAsync();
        await IdentityOptions.SetAsync();
        await CheckNewUserPhoneNumberNotBeUsedAsync(input.PhoneNumber);

        var securityTokenCacheKey = SecurityTokenCacheItem.CalculateSmsCacheKey(
            input.PhoneNumber,
            UserTwoFactorTokenProviderConsts.PhoneNumberRegisterPurpose);
        var securityTokenCacheItem = await SecurityTokenCache.GetAsync(securityTokenCacheKey);
        if (securityTokenCacheItem == null)
        {
            // 验证码过期
            throw new UserFriendlyException(L["InvalidVerifyCode"]);
        }

        // 验证码是否有效
        if (input.Code.Equals(securityTokenCacheItem.Token))
        {
            var tempNewUser = new IdentityUser(
                securityTokenCacheItem.UserId,
                "TempUser",
                "TempUser@abp.io",
                CurrentTenant.Id);
            tempNewUser.SetPhoneNumberRegisterUser();
            await UserStore.SetSecurityStampAsync(tempNewUser, securityTokenCacheItem.SecurityToken);

            if (await UserManager.VerifyUserTokenAsync(
                tempNewUser, 
                UserTwoFactorTokenProviderConsts.PhoneNumberRegisterTokenProvider, 
                UserTwoFactorTokenProviderConsts.PhoneNumberRegisterPurpose,
                input.Code))
            {
                var userEmail = input.EmailAddress ?? $"{input.PhoneNumber}@{CurrentTenant.Name ?? "default"}.io";//如果邮件地址不验证,随意写入一个
                var userName = input.UserName ?? input.PhoneNumber;
                var user = new IdentityUser(GuidGenerator.Create(), userName, userEmail, CurrentTenant.Id)
                {
                    Name = input.Name ?? input.PhoneNumber
                };

                (await UserManager.SetPhoneNumberAsync(user, input.PhoneNumber)).CheckErrors();
                (await UserManager.CreateAsync(user, input.Password)).CheckErrors();

                await UserStore.SetPhoneNumberConfirmedAsync(user, true);

                (await UserManager.AddDefaultRolesAsync(user)).CheckErrors();

                await SecurityTokenCache.RemoveAsync(securityTokenCacheKey);

                await IdentitySecurityLogManager.SaveAsync(
                    new IdentitySecurityLogContext
                    {
                        Action = "PhoneNumberRegister",
                        ClientId = await FindClientIdAsync(),
                        Identity = "Account",
                        UserName = user.UserName
                    });

                await CurrentUnitOfWork.SaveChangesAsync();

                return;
            }
        }
        // 验证码无效
        throw new UserFriendlyException(L["InvalidVerifyCode"]);
    }

    public async virtual Task SendPhoneResetPasswordCodeAsync(SendPhoneResetPasswordCodeDto input)
    {
        // 传递 isConfirmed 用户必须是已确认过手机号的
        var user = await GetUserByPhoneNumberAsync(input.PhoneNumber, isConfirmed: true);
        // 外部认证用户不允许修改密码
        if (user.IsExternal)
        {
            throw new BusinessException(code: Volo.Abp.Identity.IdentityErrorCodes.ExternalUserPasswordChange);
        }

        var securityTokenCacheKey = SecurityTokenCacheItem.CalculateSmsCacheKey(
            input.PhoneNumber,
            UserTwoFactorTokenProviderConsts.PhoneResetPasswordPurpose);
        var securityTokenCacheItem = await SecurityTokenCache.GetAsync(securityTokenCacheKey);
        var interval = await SettingProvider.GetAsync(IdentitySettingNames.User.SmsRepetInterval, 1);
        // 能查询到缓存就是重复发送
        if (securityTokenCacheItem != null)
        {
            throw new UserFriendlyException(L["SendRepeatSmsVerifyCode", interval]);
        }

        var template = await SettingProvider.GetOrNullAsync(IdentitySettingNames.User.SmsResetPassword);
        // 生成二次认证码
        var code = await UserManager.GenerateUserTokenAsync(
            user, 
            TokenOptions.DefaultPhoneProvider,
            UserTwoFactorTokenProviderConsts.PhoneResetPasswordPurpose);
        // 发送短信验证码
        await SecurityCodeSender.SendAsync(input.PhoneNumber, code, template);
        // 缓存这个手机号的记录,防重复
        securityTokenCacheItem = new SecurityTokenCacheItem(code, user.Id, user.SecurityStamp);
        await SecurityTokenCache
            .SetAsync(securityTokenCacheKey, securityTokenCacheItem,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(interval)
                });
    }

    public async virtual Task ResetPasswordAsync(PhoneResetPasswordDto input)
    {
        var securityTokenCacheKey = SecurityTokenCacheItem.CalculateSmsCacheKey(
            input.PhoneNumber,
            UserTwoFactorTokenProviderConsts.PhoneResetPasswordPurpose);
        var securityTokenCacheItem = await SecurityTokenCache.GetAsync(securityTokenCacheKey);
        if (securityTokenCacheItem == null)
        {
            throw new UserFriendlyException(L["InvalidVerifyCode"]);
        }
        await IdentityOptions.SetAsync();
        // 传递 isConfirmed 用户必须是已确认过手机号的
        var user = await GetUserByPhoneNumberAsync(input.PhoneNumber, isConfirmed: true);
        // 外部认证用户不允许修改密码
        if (user.IsExternal)
        {
            throw new BusinessException(code: Volo.Abp.Identity.IdentityErrorCodes.ExternalUserPasswordChange);
        }
        // 验证二次认证码
        if (!await UserManager.VerifyUserTokenAsync(
            user, 
            TokenOptions.DefaultPhoneProvider,
            UserTwoFactorTokenProviderConsts.PhoneResetPasswordPurpose, 
            input.Code))
        {
            // 验证码无效
            throw new UserFriendlyException(L["InvalidVerifyCode"]);
        }
        // 生成真正的重置密码Token
        var resetPwdToken = await UserManager.GeneratePasswordResetTokenAsync(user);
        // 重置密码
        (await UserManager.ResetPasswordAsync(user, resetPwdToken, input.NewPassword)).CheckErrors();
        // 移除缓存项
        await SecurityTokenCache.RemoveAsync(securityTokenCacheKey);

        await IdentitySecurityLogManager.SaveAsync(
            new IdentitySecurityLogContext
            {
                Action = "ResetPassword",
                ClientId = await FindClientIdAsync(),
                Identity = "Account",
                UserName = user.UserName
            });

        await CurrentUnitOfWork.SaveChangesAsync();
    }

    public async virtual Task SendPhoneSigninCodeAsync(SendPhoneSigninCodeDto input)
    {
        var securityTokenCacheKey = SecurityTokenCacheItem.CalculateSmsCacheKey(input.PhoneNumber, "PhoneNumberSignin");
        var securityTokenCacheItem = await SecurityTokenCache.GetAsync(securityTokenCacheKey);
        var interval = await SettingProvider.GetAsync(IdentitySettingNames.User.SmsRepetInterval, 1);
        if (securityTokenCacheItem != null)
        {
            throw new UserFriendlyException(L["SendRepeatSmsVerifyCode", interval]);
        }
        // 传递 isConfirmed 验证过的用户才允许通过手机登录
        var user = await GetUserByPhoneNumberAsync(input.PhoneNumber, isConfirmed: true);
        // 登录验证手机号需要使用二次自带的认证
        var code = await UserManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultPhoneProvider);
        var template = await SettingProvider.GetOrNullAsync(IdentitySettingNames.User.SmsUserSignin);

        // 发送登录验证码短信
        await SecurityCodeSender.SendAsync(input.PhoneNumber, code, template);
        // 缓存登录验证码状态,防止同一手机号重复发送
        securityTokenCacheItem = new SecurityTokenCacheItem(code, user.Id, user.SecurityStamp);
        await SecurityTokenCache
            .SetAsync(securityTokenCacheKey, securityTokenCacheItem,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(interval)
                });
    }

    public async virtual Task SendEmailSigninCodeAsync(SendEmailSigninCodeDto input)
    {
        var sender = LazyServiceProvider.LazyGetRequiredService<IAccountEmailSecurityCodeSender>();

        var user = await UserManager.FindByEmailAsync(input.EmailAddress);

        if (user == null)
        {
            throw new UserFriendlyException(L["UserNotRegisterd"]);
        }
        if (!user.EmailConfirmed)
        {
            throw new UserFriendlyException(L["UserEmailNotConfirmed"]);
        }

        // 登录验证邮件安全码需要使用二次自带的认证
        var code = await UserManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider);

        await sender.SendLoginCodeAsync(code, user.UserName, user.Email);
    }

    public async virtual Task<ListResultDto<NameValue>> GetTwoFactorProvidersAsync(GetTwoFactorProvidersInput input)
    {
        var user = await UserManager.GetByIdAsync(input.UserId);

        var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(user);
        return new ListResultDto<NameValue>(
            userFactors.Select(key => new NameValue(L[$"TwoFactor:{key}"].Value, key)).ToList());
    }

    protected async virtual Task<IdentityUser> GetUserByPhoneNumberAsync(string phoneNumber, bool isConfirmed = true)
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
    protected async virtual Task CheckSelfRegistrationAsync()
    {
        if (!await SettingProvider.IsTrueAsync(Volo.Abp.Account.Settings.AccountSettingNames.IsSelfRegistrationEnabled))
        {
            throw new UserFriendlyException(L["SelfRegistrationDisabledMessage"]);
        }
    }

    protected async virtual Task CheckNewUserPhoneNumberNotBeUsedAsync(string phoneNumber)
    {
        if (await UserRepository.IsPhoneNumberUedAsync(phoneNumber))
        {
            throw new UserFriendlyException(L["DuplicatePhoneNumber"]);
        }
    }

    protected virtual Task<string> FindClientIdAsync()
    {
        var client = LazyServiceProvider.LazyGetRequiredService<ICurrentClient>();

        return Task.FromResult(client.Id);
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
