using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Content;
using Volo.Abp.Http;
using Volo.Abp.Http.Client;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.BackgroundTasks.Jobs;

public abstract class HttpRequestJobBase
{
    // 可选, 请求时指定区域性
    public const string PropertyCulture = "culture";

    protected ICurrentTenant CurrentTenant { get; set; }

    protected virtual void InitJob(JobRunnableContext context)
    {
        CurrentTenant = context.GetRequiredService<ICurrentTenant>();
    }

    protected virtual async Task<T> RequestAsync<T>(
       JobRunnableContext context,
       HttpMethod httpMethod,
       string requestUrl,
       object data = null,
       string contentType = MimeTypes.Application.Json,
       IReadOnlyDictionary<string, string> headers = null,
       string clientName = null)
    {
        var response = await RequestAsync(
            context, 
            httpMethod, 
            requestUrl, 
            data,
            contentType,
            headers,
            clientName);
        var responseContent = response.Content;

        if (typeof(T) == typeof(IRemoteStreamContent) ||
            typeof(T) == typeof(RemoteStreamContent))
        {
            return (T)(object)new RemoteStreamContent(
                await responseContent.ReadAsStreamAsync(),
                responseContent.Headers?.ContentDisposition?.FileNameStar ??
                RemoveQuotes(responseContent.Headers?.ContentDisposition?.FileName).ToString(),
                responseContent.Headers?.ContentType?.ToString(),
                responseContent.Headers?.ContentLength);
        }

        var stringContent = await responseContent.ReadAsStringAsync();

        if (string.IsNullOrWhiteSpace(stringContent))
        {
            return default;
        }

        if (typeof(T) == typeof(string))
        {
            return (T)(object)stringContent;
        }

        return Deserialize<T>(stringContent);
    }

    protected virtual async Task<HttpResponseMessage> RequestAsync(
        JobRunnableContext context,
        HttpMethod httpMethod,
        string requestUrl,
        object data = null,
        string contentType = MimeTypes.Application.Json,
        IReadOnlyDictionary<string, string> headers = null,
        string clientName = null)
    {
        context.TryGetString(PropertyCulture, out var culture);
        using (CultureHelper.Use(culture ?? "en"))
        {
            var request = BuildRequestMessage(httpMethod, requestUrl, data, contentType, headers);
            var clientFactory = context.GetRequiredService<IHttpClientFactory>();

            var client = clientName.IsNullOrWhiteSpace()
                ? clientFactory.CreateClient(BackgroundTasksConsts.DefaultHttpClient)
                : clientFactory.CreateClient(clientName);

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                await ThrowExceptionForResponseAsync(response);
            }

            return response;
        }
    }

    protected virtual HttpRequestMessage BuildRequestMessage(
        HttpMethod httpMethod,
        string requestUrl,
        object data = null,
        string contentType = MimeTypes.Application.Json,
        IReadOnlyDictionary<string, string> headers = null)
    {
        var httpRequestMesasge = new HttpRequestMessage(httpMethod, requestUrl);
        if (data != null)
        {
            // TODO: 需要支持表单类型

            // application/json 支持
            httpRequestMesasge.Content = new StringContent(
                Serialize(data),
                Encoding.UTF8,
                contentType ?? MimeTypes.Application.Json);
        }

        AddHeaders(httpRequestMesasge, headers);

        return httpRequestMesasge;
    }

    protected virtual void AddHeaders(
        HttpRequestMessage requestMessage,
        IReadOnlyDictionary<string, string> headers = null)
    {
        if (CurrentTenant?.Id.HasValue == true)
        {
            requestMessage.Headers.Add(TenantResolverConsts.DefaultTenantKey, CurrentTenant.Id.ToString());
        }

        if (headers != null)
        {
            foreach (var header in headers)
            {
                requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
        }
        // 不包装请求结果
        requestMessage.Headers.TryAddWithoutValidation("_AbpDontWrapResult", "true");
        requestMessage.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue(CultureInfo.CurrentCulture.Name));
    }

    protected virtual async Task ThrowExceptionForResponseAsync(HttpResponseMessage response)
    {
        if (response.Headers.Contains(AbpHttpConsts.AbpErrorFormat))
        {
            RemoteServiceErrorResponse errorResponse;
            try
            {
                errorResponse = Deserialize<RemoteServiceErrorResponse>(
                    await response.Content.ReadAsStringAsync()
                );
            }
            catch (Exception ex)
            {
                throw new AbpRemoteCallException(
                    new RemoteServiceErrorInfo
                    {
                        Message = response.ReasonPhrase,
                        Code = response.StatusCode.ToString()
                    },
                    ex
                )
                {
                    HttpStatusCode = (int)response.StatusCode
                };
            }

            throw new AbpRemoteCallException(errorResponse.Error)
            {
                HttpStatusCode = (int)response.StatusCode
            };
        }
        else
        {
            throw new AbpRemoteCallException(
                new RemoteServiceErrorInfo
                {
                    Message = response.ReasonPhrase,
                    Code = response.StatusCode.ToString()
                }
            )
            {
                HttpStatusCode = (int)response.StatusCode
            };
        }
    }

    protected virtual T Deserialize<T>(string value)
    {
        return JsonConvert.DeserializeObject<T>(value);
    }

    protected virtual string Serialize(object value)
    {
        return JsonConvert.SerializeObject(value);
    }

    protected virtual StringSegment RemoveQuotes(StringSegment input)
    {
        if (!StringSegment.IsNullOrEmpty(input) && input.Length >= 2 && input[0] == '"' && input[input.Length - 1] == '"')
        {
            input = input.Subsegment(1, input.Length - 2);
        }

        return input;
    }
}
