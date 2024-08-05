using LINGYUN.Abp.Account.Emailing;
using LINGYUN.Abp.Identity;
using LINGYUN.Abp.Identity.Security;
using LINGYUN.Abp.Identity.Session;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Account.Localization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Caching;
using Volo.Abp.Data;
using Volo.Abp.Identity;
using Volo.Abp.Settings;
using Volo.Abp.Users;
using IIdentitySessionRepository = LINGYUN.Abp.Identity.IIdentitySessionRepository;

namespace LINGYUN.Abp.Account;

[Authorize]
public class MyProfileAppService : AccountApplicationServiceBase, IMyProfileAppService
{
    protected IDistributedCache<SecurityTokenCacheItem> SecurityTokenCache { get; }
    protected IAccountSmsSecurityCodeSender SecurityCodeSender { get; }
    protected Identity.IIdentityUserRepository UserRepository { get; }
    protected IdentitySecurityLogManager IdentitySecurityLogManager { get; }
    protected IAuthenticatorUriGenerator AuthenticatorUriGenerator => LazyServiceProvider.LazyGetRequiredService<IAuthenticatorUriGenerator>();

    protected IIdentitySessionManager IdentitySessionManager => LazyServiceProvider.LazyGetRequiredService<IIdentitySessionManager>();
    protected IIdentitySessionRepository IdentitySessionRepository => LazyServiceProvider.LazyGetRequiredService<IIdentitySessionRepository>();

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

    public async virtual Task<PagedResultDto<IdentitySessionDto>> GetSessionsAsync(GetMySessionsInput input)
    {
        var user = await GetCurrentUserAsync();
        var totalCount = await IdentitySessionRepository.GetCountAsync(
            user.Id, input.Device, input.ClientId);
        var identitySessions = await IdentitySessionRepository.GetListAsync(
            input.Sorting, input.MaxResultCount, input.SkipCount,
            user.Id, input.Device, input.ClientId);

        return new PagedResultDto<IdentitySessionDto>(totalCount,
            identitySessions.Select(session => new IdentitySessionDto
            {
                Id = session.Id,
                SessionId = session.SessionId,
                SignedIn = session.SignedIn,
                ClientId = session.ClientId,
                Device = session.Device,
                DeviceInfo = session.DeviceInfo,
                IpAddresses = session.IpAddresses,
                LastAccessed = session.LastAccessed,
            }).ToList());
    }

    public async virtual Task RevokeSessionAsync(string sessionId)
    {
        await IdentitySessionManager.RevokeSessionAsync(sessionId);
    }

    public async virtual Task<TwoFactorEnabledDto> GetTwoFactorEnabledAsync()
    {
        var user = await GetCurrentUserAsync();

        return new TwoFactorEnabledDto
        {
            Enabled = await UserManager.GetTwoFactorEnabledAsync(user),
        };
    }

    public async virtual Task ChangeTwoFactorEnabledAsync(TwoFactorEnabledDto input)
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

    public async virtual Task SendChangePhoneNumberCodeAsync(SendChangePhoneNumberCodeInput input)
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

    public async virtual Task ChangePhoneNumberAsync(ChangePhoneNumberInput input)
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

        var user = await UserManager.GetByIdAsync(CurrentUser.GetId());

        (await UserManager.ConfirmEmailAsync(user, input.ConfirmToken)).CheckErrors();

        await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext
        {
            Identity = IdentitySecurityLogIdentityConsts.Identity,
            Action = "ConfirmEmail"
        });
    }

    public async virtual Task<AuthenticatorDto> GetAuthenticator()
    {
        await IdentityOptions.SetAsync();

        var user = await UserManager.GetByIdAsync(CurrentUser.GetId());

        var hasAuthenticatorEnabled = user.GetProperty(UserManager.Options.Tokens.AuthenticatorTokenProvider, false);
        if (hasAuthenticatorEnabled)
        {
            return new AuthenticatorDto
            {
                IsAuthenticated = true,
            };
        }

        var userEmail = await UserManager.GetEmailAsync(user);
        var unformattedKey = await UserManager.GetAuthenticatorKeyAsync(user);
        if (string.IsNullOrEmpty(unformattedKey))
        {
            await UserManager.ResetAuthenticatorKeyAsync(user);
            unformattedKey = await UserManager.GetAuthenticatorKeyAsync(user);
        }

        var authenticatorUri = AuthenticatorUriGenerator.Generate(userEmail, unformattedKey);

        return new AuthenticatorDto
        {
            SharedKey = FormatKey(unformattedKey),
            AuthenticatorUri = authenticatorUri,
        };
    }

    public async virtual Task<AuthenticatorRecoveryCodeDto> VerifyAuthenticatorCode(VerifyAuthenticatorCodeInput input)
    {
        await IdentityOptions.SetAsync();

        var user = await UserManager.GetByIdAsync(CurrentUser.GetId());

        var tokenValid = await UserManager.VerifyTwoFactorTokenAsync(user, 
            UserManager.Options.Tokens.AuthenticatorTokenProvider, input.AuthenticatorCode);
        if (!tokenValid)
        {
            throw new BusinessException(Identity.IdentityErrorCodes.AuthenticatorTokenInValid);
        }

        var recoveryCodes = await UserManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);

        user.SetProperty(UserManager.Options.Tokens.AuthenticatorTokenProvider, true);

        await UserStore.ReplaceCodesAsync(user, recoveryCodes);

        (await UserManager.UpdateAsync(user)).CheckErrors();

        await CurrentUnitOfWork.SaveChangesAsync();

        return new AuthenticatorRecoveryCodeDto
        {
            RecoveryCodes = recoveryCodes.ToList(),
        };
    }

    public async virtual Task ResetAuthenticator()
    {
        await IdentityOptions.SetAsync();

        var user = await UserManager.GetByIdAsync(CurrentUser.GetId());

        user.RemoveProperty(UserManager.Options.Tokens.AuthenticatorTokenProvider);

        await UserManager.ResetAuthenticatorKeyAsync(user);
        await UserStore.ReplaceCodesAsync(user, new string[0]);

        var validTwoFactorProviders = await UserManager.GetValidTwoFactorProvidersAsync(user);
        if (!validTwoFactorProviders
            .Where(provider => provider != UserManager.Options.Tokens.AuthenticatorTokenProvider)
            .Any())
        {
            // 如果用户没有任何双因素认证提供程序,则禁用二次认证,否则将造成无法登录
            await UserManager.SetTwoFactorEnabledAsync(user, false);
        }

        (await UserManager.UpdateAsync(user)).CheckErrors();

        await CurrentUnitOfWork.SaveChangesAsync();
    }

    private static string FormatKey(string unformattedKey)
    {
        var result = new StringBuilder();
        var currentPosition = 0;
        while (currentPosition + 4 < unformattedKey.Length)
        {
            result.Append(unformattedKey.AsSpan(currentPosition, 4)).Append(' ');
            currentPosition += 4;
        }
        if (currentPosition < unformattedKey.Length)
        {
            result.Append(unformattedKey.AsSpan(currentPosition));
        }

        return result.ToString().ToLowerInvariant();
    }
}
