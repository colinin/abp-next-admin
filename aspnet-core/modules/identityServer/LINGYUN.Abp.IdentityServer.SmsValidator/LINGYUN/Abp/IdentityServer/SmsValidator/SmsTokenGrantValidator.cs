using IdentityModel;
using IdentityServer4.Events;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using LINGYUN.Abp.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Volo.Abp.IdentityServer.Localization;
using Volo.Abp.Security.Claims;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace LINGYUN.Abp.IdentityServer.SmsValidator
{
    public class SmsTokenGrantValidator : IExtensionGrantValidator
    {
        protected ILogger<SmsTokenGrantValidator> Logger { get; }
        protected IEventService EventService { get; }
        protected IIdentityUserRepository UserRepository { get; }
        protected UserManager<IdentityUser> UserManager { get; }
        protected SignInManager<IdentityUser> SignInManager { get; }
        protected IStringLocalizer<AbpIdentityServerResource> Localizer { get; }
        protected PhoneNumberTokenProvider<IdentityUser> PhoneNumberTokenProvider { get; }


        public SmsTokenGrantValidator(
            IEventService eventService,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IIdentityUserRepository userRepository,
            IStringLocalizer<AbpIdentityServerResource> stringLocalizer,
            PhoneNumberTokenProvider<IdentityUser> phoneNumberTokenProvider,
            ILogger<SmsTokenGrantValidator> logger)
        {
            Logger = logger;
            EventService = eventService;
            UserManager = userManager;
            SignInManager = signInManager;
            Localizer = stringLocalizer;
            UserRepository = userRepository;
            PhoneNumberTokenProvider = phoneNumberTokenProvider;
        }

        public string GrantType => SmsValidatorConsts.SmsValidatorGrantTypeName;

        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            var raw = context.Request.Raw;
            var credential = raw.Get(OidcConstants.TokenRequest.GrantType);
            if (credential == null || !credential.Equals(GrantType))
            {
                Logger.LogWarning("Invalid grant type: not allowed");
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant,
                    Localizer["InvalidGrant:GrantTypeInvalid"]);
                return;
            }
            var phoneNumber = raw.Get(SmsValidatorConsts.SmsValidatorParamName);
            var phoneToken = raw.Get(SmsValidatorConsts.SmsValidatorTokenName);
            if (phoneNumber.IsNullOrWhiteSpace() || phoneToken.IsNullOrWhiteSpace())
            {
                Logger.LogWarning("Invalid grant type: phone number or token code not found");
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant,
                    Localizer["InvalidGrant:PhoneOrTokenCodeNotFound"]);
                return;
            }
            var currentUser = await UserRepository.FindByPhoneNumberAsync(phoneNumber);
            if(currentUser == null)
            {
                Logger.LogWarning("Invalid grant type: phone number not register");
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant,
                    Localizer["InvalidGrant:PhoneNumberNotRegister"]);
                return;
            }
            var validResult = await PhoneNumberTokenProvider.ValidateAsync(SmsValidatorConsts.SmsValidatorPurpose, phoneToken, UserManager, currentUser);
            if (!validResult)
            {
                Logger.LogWarning("Authentication failed for token: {0}, reason: invalid token", phoneToken);
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant,
                    Localizer["InvalidGrant:PhoneVerifyInvalid"]);
                await EventService.RaiseAsync(new UserLoginFailureEvent(currentUser.UserName, $"invalid phone verify code {phoneToken}", false));
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
        }
    }
}
