using LINGYUN.Abp.Authentication.WeChat;
using LINGYUN.Abp.WeChat.Official;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Authentication.WeChat.Official
{
    /// <summary>
    /// 网页授权只有公众平台的实现
    /// </summary>
    public class WeChatOfficialOAuthHandler : OAuthHandler<WeChatOfficialOAuthOptions>
    {
        protected AbpWeChatOfficialOptionsFactory WeChatOfficialOptionsFactory { get; }
        public WeChatOfficialOAuthHandler(
            IOptionsMonitor<WeChatOfficialOAuthOptions> options,
            AbpWeChatOfficialOptionsFactory weChatOfficialOptionsFactory,
            ILoggerFactory logger, 
            UrlEncoder encoder) 
            : base(options, logger, encoder)
        {
            WeChatOfficialOptionsFactory = weChatOfficialOptionsFactory;
        }

        protected override async Task InitializeHandlerAsync()
        {
            var weChatOfficialOptions = await WeChatOfficialOptionsFactory.CreateAsync();

            // 用配置项重写
            Options.ClientId = weChatOfficialOptions.AppId;
            Options.ClientSecret = weChatOfficialOptions.AppSecret;
            Options.TimeProvider ??= TimeProvider.System;

            await base.InitializeHandlerAsync();
        }
        /// <summary>
        ///  第一步：构建用户授权地址
        /// </summary> 
        protected override string BuildChallengeUrl(AuthenticationProperties properties, string redirectUri)
        {
            var isWeChatBrewserRequest = IsWeChatBrowser();

            var scope = isWeChatBrewserRequest
                ? AbpAuthenticationWeChatConsts.UserInfoScope
                : AbpAuthenticationWeChatConsts.LoginScope;

            var endPoint = isWeChatBrewserRequest
                ? Options.AuthorizationEndpoint
                : AbpAuthenticationWeChatConsts.QrConnectEndpoint;

            redirectUri += $"?protected={Options.StateDataFormat.Protect(properties)}";

            var parameters = new Dictionary<string, string>
            {
                { "appid", Options.ClientId },
                { "redirect_uri", redirectUri },
                { "response_type", "code" },
                { "scope", scope },
                { "state", Guid.NewGuid().ToString("N") },
            };

            return $"{QueryHelpers.AddQueryString(endPoint, parameters)}#wechat_redirect";
        }

        /// <summary>
        /// 第二步：code换取access_token
        /// </summary> 
        protected async override Task<OAuthTokenResponse> ExchangeCodeAsync(OAuthCodeExchangeContext context)
        {
            var parameters = new Dictionary<string, string>()
            {
                { "appid", Options.ClientId },
                { "secret", Options.ClientSecret },
                { "code", context.Code },
                { "grant_type", "authorization_code" },
            };

            var address = QueryHelpers.AddQueryString(Options.TokenEndpoint, parameters);

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

        /// <summary>
        /// 第三步：构建用户票据
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="properties"></param>
        /// <param name="tokens"></param>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        protected async override Task<AuthenticationTicket> CreateTicketAsync(ClaimsIdentity identity, AuthenticationProperties properties, OAuthTokenResponse tokens)
        {
            var address = QueryHelpers.AddQueryString(Options.UserInformationEndpoint, new Dictionary<string, string>
            {
                ["access_token"] = tokens.AccessToken,
                ["openid"] = tokens.Response.GetRootString("openid")
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

            var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            if (!string.IsNullOrEmpty(payload.GetRootString("errcode")))
            {
                Logger.LogError("An error occurred while retrieving the user profile: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ response.StatusCode,
                                /* Headers: */ response.Headers.ToString(),
                                /* Body: */ await response.Content.ReadAsStringAsync());

                throw new HttpRequestException("An error occurred while retrieving user information.");
            }

            var context = new OAuthCreatingTicketContext(new ClaimsPrincipal(identity), properties, Context, Scheme, Options, Backchannel, tokens, payload.RootElement);
            context.RunClaimActions();

            await Events.CreatingTicket(context);

            return new AuthenticationTicket(context.Principal, context.Properties, Scheme.Name);
        }

        public override Task<bool> HandleRequestAsync()
        {
            return base.HandleRequestAsync();
        }

        protected async override Task<HandleRequestResult> HandleRemoteAuthenticateAsync()
        {
            var query = Request.Query;

            // TODO: 此处借用唯一的 CorrelationId, 将 properties生成的State缓存取出,进行解密
            var state = query["protected"];

            var properties = Options.StateDataFormat.Unprotect(state);

            if (properties == null)
            {
                return HandleRequestResult.Fail("The oauth state was missing or invalid.");
            }

            // OAuth2 10.12 CSRF
            if (!ValidateCorrelationId(properties))
            {
                return HandleRequestResult.Fail("Correlation failed.", properties);
            }

            var error = query["error"];
            if (!StringValues.IsNullOrEmpty(error))
            {
                // Note: access_denied errors are special protocol errors indicating the user didn't
                // approve the authorization demand requested by the remote authorization server.
                // Since it's a frequent scenario (that is not caused by incorrect configuration),
                // denied errors are handled differently using HandleAccessDeniedErrorAsync().
                // Visit https://tools.ietf.org/html/rfc6749#section-4.1.2.1 for more information.
                var errorDescription = query["error_description"];
                var errorUri = query["error_uri"];
                if (StringValues.Equals(error, "access_denied"))
                {
                    var result = await HandleAccessDeniedErrorAsync(properties);
                    if (!result.None)
                    {
                        return result;
                    }
                    var deniedEx = new Exception("Access was denied by the resource owner or by the remote server.");
                    deniedEx.Data["error"] = error.ToString();
                    deniedEx.Data["error_description"] = errorDescription.ToString();
                    deniedEx.Data["error_uri"] = errorUri.ToString();

                    return HandleRequestResult.Fail(deniedEx, properties);
                }

                var failureMessage = new StringBuilder();
                failureMessage.Append(error);
                if (!StringValues.IsNullOrEmpty(errorDescription))
                {
                    failureMessage.Append(";Description=").Append(errorDescription);
                }
                if (!StringValues.IsNullOrEmpty(errorUri))
                {
                    failureMessage.Append(";Uri=").Append(errorUri);
                }

                var ex = new Exception(failureMessage.ToString());
                ex.Data["error"] = error.ToString();
                ex.Data["error_description"] = errorDescription.ToString();
                ex.Data["error_uri"] = errorUri.ToString();

                return HandleRequestResult.Fail(ex, properties);
            }

            var code = query["code"];

            if (StringValues.IsNullOrEmpty(code))
            {
                return HandleRequestResult.Fail("Code was not found.", properties);
            }

            var codeExchangeContext = new OAuthCodeExchangeContext(properties, code, BuildRedirectUri(Options.CallbackPath));
            using var tokens = await ExchangeCodeAsync(codeExchangeContext);

            if (tokens.Error != null)
            {
                return HandleRequestResult.Fail(tokens.Error, properties);
            }

            if (string.IsNullOrEmpty(tokens.AccessToken))
            {
                return HandleRequestResult.Fail("Failed to retrieve access token.", properties);
            }

            var identity = new ClaimsIdentity(ClaimsIssuer);

            if (Options.SaveTokens)
            {
                var authTokens = new List<AuthenticationToken>();

                authTokens.Add(new AuthenticationToken { Name = "access_token", Value = tokens.AccessToken });
                if (!string.IsNullOrEmpty(tokens.RefreshToken))
                {
                    authTokens.Add(new AuthenticationToken { Name = "refresh_token", Value = tokens.RefreshToken });
                }

                if (!string.IsNullOrEmpty(tokens.TokenType))
                {
                    authTokens.Add(new AuthenticationToken { Name = "token_type", Value = tokens.TokenType });
                }

                if (!string.IsNullOrEmpty(tokens.ExpiresIn))
                {
                    int value;
                    if (int.TryParse(tokens.ExpiresIn, NumberStyles.Integer, CultureInfo.InvariantCulture, out value))
                    {
                        // https://www.w3.org/TR/xmlschema-2/#dateTime
                        // https://msdn.microsoft.com/en-us/library/az4se3k1(v=vs.110).aspx
                        var expiresAt = Options.TimeProvider.GetUtcNow() + TimeSpan.FromSeconds(value);
                        authTokens.Add(new AuthenticationToken
                        {
                            Name = "expires_at",
                            Value = expiresAt.ToString("o", CultureInfo.InvariantCulture)
                        });
                    }
                }

                properties.StoreTokens(authTokens);
            }

            var ticket = await CreateTicketAsync(identity, properties, tokens);
            if (ticket != null)
            {
                return HandleRequestResult.Success(ticket);
            }
            else
            {
                return HandleRequestResult.Fail("Failed to retrieve user information from remote server.", properties);
            }
        }

        protected override string FormatScope()
        {
            return string.Join(",", Options.Scope);
        }

        protected virtual bool IsWeChatBrowser()
        {
            var userAgent = Request.Headers[HeaderNames.UserAgent].ToString();

            return userAgent.Contains("micromessenger", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}