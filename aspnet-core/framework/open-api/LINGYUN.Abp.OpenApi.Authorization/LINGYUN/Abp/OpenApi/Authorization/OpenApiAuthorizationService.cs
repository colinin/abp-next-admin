using LINGYUN.Abp.Wrapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Volo.Abp;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.AspNetCore.WebClientInfo;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http;
using Volo.Abp.Json;

namespace LINGYUN.Abp.OpenApi.Authorization
{
    public class OpenApiAuthorizationService : IOpenApiAuthorizationService, ITransientDependency
    {
        private readonly IAppKeyStore _appKeyStore;
        private readonly AbpOpenApiOptions _openApiOptions;
        private readonly IWebClientInfoProvider _clientInfoProvider;
        private readonly INonceStore _nonceStore;
        private readonly IClientChecker _clientChecker;
        private readonly IIpAddressChecker _ipAddressChecker;
        private readonly AbpExceptionHandlingOptions _exceptionHandlingOptions;

        public OpenApiAuthorizationService(
            INonceStore nonceStore,
            IAppKeyStore appKeyStore,
            IClientChecker clientChecker,
            IIpAddressChecker ipAddressChecker,
            IWebClientInfoProvider clientInfoProvider,
            IOptionsMonitor<AbpOpenApiOptions> options,
            IOptions<AbpExceptionHandlingOptions> exceptionHandlingOptions)
        {
            _nonceStore = nonceStore;
            _appKeyStore = appKeyStore;
            _clientChecker = clientChecker;
            _ipAddressChecker = ipAddressChecker;
            _clientInfoProvider = clientInfoProvider;
            _openApiOptions = options.CurrentValue;
            _exceptionHandlingOptions = exceptionHandlingOptions.Value;
        }

        public async virtual Task<bool> AuthorizeAsync(HttpContext httpContext)
        {
            if (!_openApiOptions.IsEnabled)
            {
                return true;
            }

            if (!await ValidateClientIpAddress(httpContext))
            {
                return false;
            }

            if (!await ValidatAppDescriptor(httpContext))
            {
                return false;
            }

            return true;
        }

        protected async virtual Task<bool> ValidateClientIpAddress(HttpContext httpContext)
        {
            if (!string.IsNullOrWhiteSpace(_clientInfoProvider.ClientIpAddress) &&
                !await _ipAddressChecker.IsGrantAsync(_clientInfoProvider.ClientIpAddress, httpContext.RequestAborted))
            {
                var exception = new BusinessException(
                    AbpOpenApiConsts.InvalidAccessWithIpAddress,
                    $"Client IpAddress {_clientInfoProvider.ClientIpAddress} Not Allowed",
                    $"Client IpAddress {_clientInfoProvider.ClientIpAddress} Not Allowed");
                await Unauthorized(httpContext, exception);
                return false;
            }
            return true;
        }

        protected async virtual Task<bool> ValidatAppDescriptor(HttpContext httpContext)
        {
            httpContext.Request.Headers.TryGetValue(AbpOpenApiConsts.HEADER_APP_KEY, out var appKey);
            httpContext.Request.Headers.TryGetValue(AbpOpenApiConsts.HEADER_SIGNATURE, out var sign);
            httpContext.Request.Headers.TryGetValue(AbpOpenApiConsts.HEADER_NONCE, out var nonce);
            httpContext.Request.Headers.TryGetValue(AbpOpenApiConsts.HEADER_TIMESTAMP, out var timeStampString);


            if (StringValues.IsNullOrEmpty(appKey))
            {
                var exception = new BusinessException(
                    AbpOpenApiConsts.InvalidAccessWithAppKeyNotFound,
                    $"{AbpOpenApiConsts.HEADER_APP_KEY} Not Found",
                    $"{AbpOpenApiConsts.HEADER_APP_KEY} Not Found");
                await Unauthorized(httpContext, exception);
                return false;
            }

            if (StringValues.IsNullOrEmpty(nonce))
            {
                var exception = new BusinessException(
                    AbpOpenApiConsts.InvalidAccessWithNonceNotFound,
                    $"{AbpOpenApiConsts.HEADER_NONCE} Not Found",
                    $"{AbpOpenApiConsts.HEADER_NONCE} Not Found");

                await Unauthorized(httpContext, exception);
                return false;
            }

            if (StringValues.IsNullOrEmpty(sign))
            {
                var exception = new BusinessException(
                    AbpOpenApiConsts.InvalidAccessWithSignNotFound,
                    $"{AbpOpenApiConsts.HEADER_SIGNATURE} Not Found",
                    $"{AbpOpenApiConsts.HEADER_SIGNATURE} Not Found");

                await Unauthorized(httpContext, exception);
                return false;
            }

            if (StringValues.IsNullOrEmpty(timeStampString))
            {
                var exception = new BusinessException(
                    AbpOpenApiConsts.InvalidAccessWithTimestampNotFound,
                    $"{AbpOpenApiConsts.HEADER_TIMESTAMP} Not Found",
                    $"{AbpOpenApiConsts.HEADER_TIMESTAMP} Not Found");

                await Unauthorized(httpContext, exception);
                return false;
            }

            if (!long.TryParse(timeStampString.ToString(), out long timeStamp))
            {
                var exception = new BusinessException(
                    AbpOpenApiConsts.InvalidAccessWithTimestamp,
                    "invalid timestamp",
                    "invalid timestamp");

                await Unauthorized(httpContext, exception);
                return false;
            }

            if (!await _nonceStore.TrySetAsync(nonce.ToString(), httpContext.RequestAborted))
            {
                var exception = new BusinessException(
                    AbpOpenApiConsts.InvalidAccessWithNonceRepeated,
                    $"Request {nonce} has repeated",
                    $"Request {nonce} has repeated");
                await Unauthorized(httpContext, exception);
                return false;
            }

            if (!await _clientChecker.IsGrantAsync(appKey.ToString(), httpContext.RequestAborted))
            {
                var exception = new BusinessException(
                    AbpOpenApiConsts.InvalidAccessWithClientId,
                    $"Client Id {appKey} Not Allowed",
                    $"Client Id {appKey} Not Allowed");
                await Unauthorized(httpContext, exception);
                return false;
            }

            var appDescriptor = await _appKeyStore.FindAsync(appKey.ToString(), httpContext.RequestAborted);
            if (appDescriptor == null)
            {
                var exception = new BusinessException(
                    AbpOpenApiConsts.InvalidAccessWithAppKey,
                    "invalid appKey",
                    "invalid appKey")
                    .WithData("AppKey", appKey.ToString());

                await Unauthorized(httpContext, exception);
                return false;
            }

            var queryDictionary = new Dictionary<string, string>();
            var queryStringCollection = httpContext.Request.Query;
            foreach (var queryString in queryStringCollection)
            {
                queryDictionary.Add(queryString.Key, queryString.Value.ToString());
            }
            queryDictionary.TryAdd("appKey", appDescriptor.AppKey);
            queryDictionary.TryAdd("appSecret", appDescriptor.AppSecret);
            queryDictionary.TryAdd("nonce", nonce.ToString());
            queryDictionary.TryAdd("t", timeStampString.ToString());

            var requiredSign = CalculationSignature(httpContext.Request.Path.Value, queryDictionary);
            if (!string.Equals(requiredSign, sign.ToString()))
            {
                var exception = new BusinessException(
                    AbpOpenApiConsts.InvalidAccessWithSign,
                    "invalid signature",
                    "invalid signature");

                await Unauthorized(httpContext, exception);
                return false;
            }

            if (appDescriptor.SignLifetime.HasValue && appDescriptor.SignLifetime > 0)
            {
                var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                if ((now - timeStamp) / 1000 > appDescriptor.SignLifetime.Value)
                {
                    var exception = new BusinessException(
                      AbpOpenApiConsts.InvalidAccessWithTimestamp,
                      "session timed out or expired",
                      "session timed out or expired");

                    await Unauthorized(httpContext, exception);
                    return false;
                }
            }

            return true;
        }

        protected async virtual Task Unauthorized(HttpContext context, Exception exception)
        {
            var errorInfoConverter = context.RequestServices.GetRequiredService<IExceptionToErrorInfoConverter>();
            var errorInfo = errorInfoConverter.Convert(exception, options =>
            {
                options.SendExceptionsDetailsToClients = _exceptionHandlingOptions.SendExceptionsDetailsToClients;
                options.SendStackTraceToClients = _exceptionHandlingOptions.SendStackTraceToClients;
            });

            if (context.Request.CanAccept(MimeTypes.Application.Json) ||
                context.Request.IsAjax())
            {
                var wrapOptions = context.RequestServices.GetRequiredService<IOptions<AbpWrapperOptions>>().Value;
                if (wrapOptions.IsEnabled)
                {
                    var jsonSerializer = context.RequestServices.GetRequiredService<IJsonSerializer>();
                    var exceptionWrapHandlerFactory = context.RequestServices.GetRequiredService<IExceptionWrapHandlerFactory>();
                    var exceptionWrapContext = new ExceptionWrapContext(
                        exception,
                        errorInfo,
                        context.RequestServices);
                    exceptionWrapHandlerFactory.CreateFor(exceptionWrapContext).Wrap(exceptionWrapContext);

                    var wrapResult = new WrapResult(
                        exceptionWrapContext.ErrorInfo.Code,
                        exceptionWrapContext.ErrorInfo.Message,
                        exceptionWrapContext.ErrorInfo.Details);

                    context.Response.Clear();
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    context.Response.Headers.Append(AbpHttpWrapConsts.AbpWrapResult, "true");
                    context.Response.Headers.Append("Content-Type", "application/json");

                    await context.Response.WriteAsync(jsonSerializer.Serialize(wrapResult));
                    return;
                }
            }

            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            await context.Response.WriteAsync(errorInfo.Message);
        }

        private static string CalculationSignature(string url, IDictionary<string, string> queryDictionary)
        {
            var queryString = BuildQuery(queryDictionary);
            // %20 替换 +
            var encodeUrl = UrlEncode(string.Concat(url, "?", queryString)).Replace("+", "%20");

            return encodeUrl.ToMd5();
        }

        private static string BuildQuery(IDictionary<string, string> queryStringDictionary)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var queryString in queryStringDictionary.OrderBy(q => q.Key))
            {
                sb.Append(queryString.Key)
                  .Append('=')
                  .Append(queryString.Value)
                  .Append('&');
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        private static string UrlEncode(string str)
        {
            return HttpUtility.UrlEncode(str, Encoding.UTF8).ToUpper();
        }
    }
}
