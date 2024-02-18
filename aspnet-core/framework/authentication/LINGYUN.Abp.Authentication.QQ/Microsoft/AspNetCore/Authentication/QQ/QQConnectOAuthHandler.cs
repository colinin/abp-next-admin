using LINGYUN.Abp.Authentication.QQ;
using LINGYUN.Abp.Tencent.QQ;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Authentication.QQ
{
    /// <summary>
    /// QQ互联实现
    /// </summary>
    public class QQConnectOAuthHandler : OAuthHandler<QQConnectOAuthOptions>
    {
        protected AbpTencentQQOptionsFactory TencentQQOptionsFactory { get; }
        public QQConnectOAuthHandler(
            IOptionsMonitor<QQConnectOAuthOptions> options,
            AbpTencentQQOptionsFactory tencentQQOptionsFactory,
            ILoggerFactory logger, 
            UrlEncoder encoder) 
            : base(options, logger, encoder)
        {
            TencentQQOptionsFactory = tencentQQOptionsFactory;
        }

        protected override async Task InitializeHandlerAsync()
        {
            var options = await TencentQQOptionsFactory.CreateAsync();

            // 用配置项重写
            Options.ClientId = options.AppId;
            Options.ClientSecret = options.AppKey;
            Options.IsMobile = options.IsMobile;
            Options.TimeProvider ??= TimeProvider.System;

            await base.InitializeHandlerAsync();
        }

        /// <summary>
        ///  构建用户授权地址
        /// </summary> 
        protected override string BuildChallengeUrl(AuthenticationProperties properties, string redirectUri)
        {
            var challengeUrl = base.BuildChallengeUrl(properties, redirectUri);
            if (Options.IsMobile)
            {
                challengeUrl += "&display=mobile";
            }
            return challengeUrl;

        }

        /// <summary>
        /// code换取access_token
        /// </summary> 
        protected override async Task<OAuthTokenResponse> ExchangeCodeAsync(OAuthCodeExchangeContext context)
        {
            var address = QueryHelpers.AddQueryString(Options.TokenEndpoint, new Dictionary<string, string>()
            {
                { "client_id", Options.ClientId },
                { "redirect_uri", context.RedirectUri },
                { "client_secret", Options.ClientSecret},
                { "code", context.Code},
                { "grant_type","authorization_code"}
            });

            var response = await Backchannel.GetAsync(address);
            if (!response.IsSuccessStatusCode)
            {
                Logger.LogError("An error occurred while retrieving an access token: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ response.StatusCode,
                                /* Headers: */ response.Headers.ToString(),
                                /* Body: */ await response.Content.ReadAsStringAsync());

                return OAuthTokenResponse.Failed(new Exception("An error occurred while retrieving an access token."));
            }

            var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            if (!string.IsNullOrEmpty(payload.GetRootString("errcode")))
            {
                Logger.LogError("An error occurred while retrieving an access token: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ response.StatusCode,
                                /* Headers: */ response.Headers.ToString(),
                                /* Body: */ await response.Content.ReadAsStringAsync());

                return OAuthTokenResponse.Failed(new Exception("An error occurred while retrieving an access token."));
            }
            return OAuthTokenResponse.Success(payload);
        }

        protected override async Task<AuthenticationTicket> CreateTicketAsync(ClaimsIdentity identity, AuthenticationProperties properties, OAuthTokenResponse tokens)
        {
            var openIdEndpoint = Options.OpenIdEndpoint + "?access_token=" + tokens.AccessToken + "&fmt=json";
            var openIdResponse = await Backchannel.GetAsync(openIdEndpoint, Context.RequestAborted);
            openIdResponse.EnsureSuccessStatusCode();

            var openIdPayload = JsonDocument.Parse(await openIdResponse.Content.ReadAsStringAsync());
            var openId = openIdPayload.GetRootString("openid");

            identity.AddClaim(new Claim(AbpQQClaimTypes.OpenId, openId, ClaimValueTypes.String, Options.ClaimsIssuer));

            var address = QueryHelpers.AddQueryString(Options.UserInformationEndpoint, new Dictionary<string, string>
            {
                {"oauth_consumer_key", Options.ClientId},
                {"access_token", tokens.AccessToken},
                {"openid", openId}
            });

            var response = await Backchannel.GetAsync(address);
            if (!response.IsSuccessStatusCode)
            {
                Logger.LogError("An error occurred while retrieving the user profile: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ response.StatusCode,
                                /* Headers: */ response.Headers.ToString(),
                                /* Body: */ await response.Content.ReadAsStringAsync());

                throw new HttpRequestException("An error occurred while retrieving user information.");
            }

            var userInfoPayload = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            var errorCode = userInfoPayload.GetRootString("ret");
            if (!"0".Equals(errorCode))
            {
                // See: https://wiki.connect.qq.com/%e5%85%ac%e5%85%b1%e8%bf%94%e5%9b%9e%e7%a0%81%e8%af%b4%e6%98%8e
                Logger.LogError("An error occurred while retrieving the user profile: the remote server " +
                                "returned code {Code} response with message: {Message}.",
                                errorCode,
                                userInfoPayload.GetRootString("msg"));

                throw new HttpRequestException("An error occurred while retrieving user information.");
            }

            var nickName = userInfoPayload.GetRootString("nickname");
            if (!nickName.IsNullOrWhiteSpace())
            {
                identity.AddClaim(new Claim(AbpQQClaimTypes.NickName, nickName, ClaimValueTypes.String, Options.ClaimsIssuer));
            }
            var gender = userInfoPayload.GetRootString("gender");
            if (!gender.IsNullOrWhiteSpace())
            {
                identity.AddClaim(new Claim(AbpQQClaimTypes.Gender, gender, ClaimValueTypes.String, Options.ClaimsIssuer));
            }
            var avatarUrl = userInfoPayload.GetRootString("figureurl_qq_1");
            if (!avatarUrl.IsNullOrWhiteSpace())
            {
                identity.AddClaim(new Claim(AbpQQClaimTypes.AvatarUrl, avatarUrl, ClaimValueTypes.String, Options.ClaimsIssuer));
            }

            var context = new OAuthCreatingTicketContext(
                new ClaimsPrincipal(identity), 
                properties,
                Context,
                Scheme, 
                Options,
                Backchannel, 
                tokens,
                userInfoPayload.RootElement);

            context.RunClaimActions();

            await Events.CreatingTicket(context);

            return new AuthenticationTicket(context.Principal, context.Properties, Scheme.Name);
        }
    }
}