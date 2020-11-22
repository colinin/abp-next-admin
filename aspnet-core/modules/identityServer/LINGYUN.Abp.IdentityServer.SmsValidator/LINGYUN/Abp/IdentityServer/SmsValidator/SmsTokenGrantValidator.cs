using IdentityModel;
using IdentityServer4.Events;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using LINGYUN.Abp.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Volo.Abp.IdentityServer.Localization;
using Volo.Abp.Security.Claims;
using IdentityUser = Volo.Abp.Identity.IdentityUser;
using IdentityResource = Volo.Abp.Identity.Localization.IdentityResource;

namespace LINGYUN.Abp.IdentityServer.SmsValidator
{
    public class SmsTokenGrantValidator : IExtensionGrantValidator
    {
        protected ILogger<SmsTokenGrantValidator> Logger { get; }
        protected IEventService EventService { get; }
        protected IIdentityUserRepository UserRepository { get; }
        protected UserManager<IdentityUser> UserManager { get; }
        protected IStringLocalizer<IdentityResource> IdentityLocalizer { get; }
        protected IStringLocalizer<AbpIdentityServerResource> IdentityServerLocalizer { get; }

        public SmsTokenGrantValidator(
            IEventService eventService,
            UserManager<IdentityUser> userManager,
            IIdentityUserRepository userRepository,
            IStringLocalizer<IdentityResource> identityLocalizer,
            IStringLocalizer<AbpIdentityServerResource> identityServerLocalizer,
            ILogger<SmsTokenGrantValidator> logger)
        {
            Logger = logger;
            EventService = eventService;
            UserManager = userManager;
            UserRepository = userRepository;
            IdentityLocalizer = identityLocalizer;
            IdentityServerLocalizer = identityServerLocalizer;
        }

        public string GrantType => SmsValidatorConsts.SmsValidatorGrantTypeName;

        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            var raw = context.Request.Raw;
            var credential = raw.Get(OidcConstants.TokenRequest.GrantType);
            if (credential == null || !credential.Equals(GrantType))
            {
                Logger.LogInformation("Invalid grant type: not allowed");
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, IdentityServerLocalizer["InvalidGrant:GrantTypeInvalid"]);
                return;
            }
            var phoneNumber = raw.Get(SmsValidatorConsts.SmsValidatorParamName);
            var phoneToken = raw.Get(SmsValidatorConsts.SmsValidatorTokenName);
            if (phoneNumber.IsNullOrWhiteSpace() || phoneToken.IsNullOrWhiteSpace())
            {
                Logger.LogInformation("Invalid grant type: phone number or token code not found");
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, IdentityServerLocalizer["InvalidGrant:PhoneOrTokenCodeNotFound"]);
                return;
            }
            var currentUser = await UserRepository.FindByPhoneNumberAsync(phoneNumber);
            if(currentUser == null)
            {
                Logger.LogInformation("Invalid grant type: phone number not register");
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, IdentityServerLocalizer["InvalidGrant:PhoneNumberNotRegister"]);
                return;
            }

            if (await UserManager.IsLockedOutAsync(currentUser))
            {
                Logger.LogInformation("Authentication failed for username: {username}, reason: locked out", currentUser.UserName);
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, IdentityLocalizer["Volo.Abp.Identity:UserLockedOut"]);
                return;
            }
            
            var validResult = await UserManager.VerifyTwoFactorTokenAsync(currentUser, TokenOptions.DefaultPhoneProvider, phoneToken);
            if (!validResult)
            {
                Logger.LogWarning("Authentication failed for token: {0}, reason: invalid token", phoneToken);
                // 防尝试破解密码
                var identityResult = await UserManager.AccessFailedAsync(currentUser);
                if (identityResult.Succeeded)
                {
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, IdentityServerLocalizer["InvalidGrant:PhoneVerifyInvalid"]);
                    await EventService.RaiseAsync(new UserLoginFailureEvent(currentUser.UserName, $"invalid phone verify code {phoneToken}", false));
                }
                else
                {
                    Logger.LogInformation("Authentication failed for username: {username}, reason: access failed", currentUser.UserName);
                    var userAccessFailedError = identityResult.LocalizeErrors(IdentityLocalizer);
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, userAccessFailedError);
                    await EventService.RaiseAsync(new UserLoginFailureEvent(currentUser.UserName, userAccessFailedError, false));
                }
                return;
            }

            var sub = await UserManager.GetUserIdAsync(currentUser);

            var additionalClaims = new List<Claim>();
            if (currentUser.TenantId.HasValue)
            {
                additionalClaims.Add(new Claim(AbpClaimTypes.TenantId, currentUser.TenantId?.ToString()));
            }

            await EventService.RaiseAsync(new UserLoginSuccessEvent(currentUser.UserName, phoneNumber, null));
            context.Result = new GrantValidationResult(sub, OidcConstants.AuthenticationMethods.ConfirmationBySms, additionalClaims.ToArray());

            // 登录之后需要更新安全令牌
            (await UserManager.UpdateSecurityStampAsync(currentUser)).CheckErrors();
        }
    }
}
