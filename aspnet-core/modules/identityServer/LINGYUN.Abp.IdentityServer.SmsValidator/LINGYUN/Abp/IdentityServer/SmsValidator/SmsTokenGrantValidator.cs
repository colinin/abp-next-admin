using IdentityModel;
using IdentityServer4.Events;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Volo.Abp.Identity;
using Volo.Abp.IdentityServer;
using Volo.Abp.IdentityServer.Localization;
using Volo.Abp.Security.Claims;

using IdentityResource = Volo.Abp.Identity.Localization.IdentityResource;
using IdentityUser = Volo.Abp.Identity.IdentityUser;
using IIdentityUserRepository = LINGYUN.Abp.Identity.IIdentityUserRepository;

namespace LINGYUN.Abp.IdentityServer.SmsValidator
{
    public class SmsTokenGrantValidator : IExtensionGrantValidator
    {
        protected ILogger<SmsTokenGrantValidator> Logger { get; }
        protected IEventService EventService { get; }
        protected IIdentityUserRepository UserRepository { get; }
        protected UserManager<IdentityUser> UserManager { get; }
        protected IdentitySecurityLogManager IdentitySecurityLogManager { get; }
        protected IStringLocalizer<IdentityResource> IdentityLocalizer { get; }
        protected IStringLocalizer<AbpIdentityServerResource> IdentityServerLocalizer { get; }

        public SmsTokenGrantValidator(
            IEventService eventService,
            UserManager<IdentityUser> userManager,
            IIdentityUserRepository userRepository,
            IdentitySecurityLogManager identitySecurityLogManager,
            IStringLocalizer<IdentityResource> identityLocalizer,
            IStringLocalizer<AbpIdentityServerResource> identityServerLocalizer,
            ILogger<SmsTokenGrantValidator> logger)
        {
            Logger = logger;
            EventService = eventService;
            UserManager = userManager;
            UserRepository = userRepository;
            IdentitySecurityLogManager = identitySecurityLogManager;
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

                await SaveSecurityLogAsync(context, currentUser, IdentityServerSecurityLogActionConsts.LoginLockedout);

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

                await SaveSecurityLogAsync(context, currentUser, SmsValidatorConsts.SecurityCodeFailed);

                return;
            }

            await EventService.RaiseAsync(new UserLoginSuccessEvent(currentUser.UserName, phoneNumber, null));

            // 登录之后需要更新安全令牌
            (await UserManager.UpdateSecurityStampAsync(currentUser)).CheckErrors();

            await SetSuccessResultAsync(context, currentUser);
        }

        protected virtual async Task SetSuccessResultAsync(ExtensionGrantValidationContext context, IdentityUser user)
        {
            var sub = await UserManager.GetUserIdAsync(user);

            Logger.LogInformation("Credentials validated for username: {username}", user.UserName);

            var additionalClaims = new List<Claim>();

            await AddCustomClaimsAsync(additionalClaims, user, context);

            context.Result = new GrantValidationResult(
                sub,
                OidcConstants.AuthenticationMethods.ConfirmationBySms,
                additionalClaims.ToArray()
            );

            await SaveSecurityLogAsync(
                context,
                user,
                IdentityServerSecurityLogActionConsts.LoginSucceeded);
        }

        protected virtual async Task SaveSecurityLogAsync(
            ExtensionGrantValidationContext context,
            IdentityUser user,
            string action)
        {
            var logContext = new IdentitySecurityLogContext
            {
                Identity = IdentityServerSecurityLogIdentityConsts.IdentityServer,
                Action = action,
                UserName = user.UserName,
                ClientId = await FindClientIdAsync(context)
            };
            logContext.WithProperty("GrantType", GrantType);

            await IdentitySecurityLogManager.SaveAsync(logContext);
        }

        protected virtual Task<string> FindClientIdAsync(ExtensionGrantValidationContext context)
        {
            return Task.FromResult(context.Request?.Client?.ClientId);
        }

        protected virtual Task AddCustomClaimsAsync(
            List<Claim> customClaims,
            IdentityUser user,
            ExtensionGrantValidationContext context)
        {
            if (user.TenantId.HasValue)
            {
                customClaims.Add(
                    new Claim(
                        AbpClaimTypes.TenantId,
                        user.TenantId?.ToString()
                    )
                );
            }

            return Task.CompletedTask;
        }
    }
}
