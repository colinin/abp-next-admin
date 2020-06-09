using IdentityModel;
using IdentityServer4.Events;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Volo.Abp.Identity;
using Volo.Abp.IdentityServer.Localization;
using Volo.Abp.Security.Claims;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace LINGYUN.Abp.IdentityServer.WeChatValidator
{
    public class WeChatTokenGrantValidator : IExtensionGrantValidator
    {
        protected ILogger<WeChatTokenGrantValidator> Logger { get; }
        protected AbpWeChatValidatorOptions Options { get; }
        protected IHttpClientFactory HttpClientFactory{ get; }
        protected IEventService EventService { get; }
        protected IIdentityUserRepository UserRepository { get; }
        protected UserManager<IdentityUser> UserManager { get; }
        protected SignInManager<IdentityUser> SignInManager { get; }
        protected IStringLocalizer<AbpIdentityServerResource> Localizer { get; }
        protected PhoneNumberTokenProvider<IdentityUser> PhoneNumberTokenProvider { get; }


        public WeChatTokenGrantValidator(
            IEventService eventService,
            IHttpClientFactory httpClientFactory,
            UserManager<IdentityUser> userManager,
            IIdentityUserRepository userRepository,
            SignInManager<IdentityUser> signInManager,
            IStringLocalizer<AbpIdentityServerResource> stringLocalizer,
            PhoneNumberTokenProvider<IdentityUser> phoneNumberTokenProvider,
            IOptionsSnapshot<AbpWeChatValidatorOptions> options,
            ILogger<WeChatTokenGrantValidator> logger)
        {
            Logger = logger;
            Options = options.Value;

            EventService = eventService;
            UserManager = userManager;
            SignInManager = signInManager;
            Localizer = stringLocalizer;
            UserRepository = userRepository;
            HttpClientFactory = httpClientFactory;
            PhoneNumberTokenProvider = phoneNumberTokenProvider;
        }

        public string GrantType => WeChatValidatorConsts.WeChatValidatorGrantTypeName;

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
            var wechatCode = raw.Get(WeChatValidatorConsts.WeChatValidatorTokenName);
            if (wechatCode.IsNullOrWhiteSpace() || wechatCode.IsNullOrWhiteSpace())
            {
                Logger.LogWarning("Invalid grant type: wechat code not found");
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant,
                    Localizer["InvalidGrant:WeChatCodeNotFound"]);
                return;
            }
            var httpClient = HttpClientFactory.CreateClient(WeChatValidatorConsts.WeChatValidatorClientName);
            var httpRequest = new WeChatTokenRequest
            {
                Code = wechatCode,
                AppId = Options.AppId,
                Secret = Options.AppSecret,
                BaseUrl = httpClient.BaseAddress.AbsoluteUri
            };

            var wechatTokenResponse = await httpClient.RequestWeChatCodeTokenAsync(httpRequest);
            if (wechatTokenResponse.IsError)
            {
                Logger.LogWarning("Authentication failed for token: {0}, reason: invalid token", wechatCode);
                Logger.LogWarning("WeChat auth failed, error: {0}", wechatTokenResponse.ErrorMessage);
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant,
                    Localizer["InvalidGrant:WeChatTokenInvalid"]);
                return;
            }
            var currentUser = await UserManager.FindByNameAsync(wechatTokenResponse.OpenId);
            if(currentUser == null)
            {
                Logger.LogWarning("Invalid grant type: wechat openid: {0} not register", wechatTokenResponse.OpenId);
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant,
                    Localizer["InvalidGrant:WeChatNotRegister"]);
                return;
            }
            var sub = await UserManager.GetUserIdAsync(currentUser);

            var additionalClaims = new List<Claim>();
            if (currentUser.TenantId.HasValue)
            {
                additionalClaims.Add(new Claim(AbpClaimTypes.TenantId, currentUser.TenantId?.ToString()));
            }
            additionalClaims.Add(new Claim(WeChatValidatorConsts.ClaimTypes.OpenId, wechatTokenResponse.OpenId));

            await EventService.RaiseAsync(new UserLoginSuccessEvent(currentUser.UserName, wechatTokenResponse.OpenId, null));
            context.Result = new GrantValidationResult(sub, 
                WeChatValidatorConsts.AuthenticationMethods.BasedWeChatAuthentication, additionalClaims.ToArray());
        }
    }
}
