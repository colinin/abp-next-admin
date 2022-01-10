using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Http;
using Volo.Abp.Json;

namespace LINGYUN.Abp.BackgroundTasks.Jobs;

public class HttpRequestJob : IJobRunnable
{
    public const string PropertyUrl = "url";
    public const string PropertyMethod = "method";
    public const string PropertyData = "data";
    public const string PropertyContentType = "contentType";
    public const string PropertyHeaders = "headers";
    public const string PropertyToken = "token";

    public virtual async Task ExecuteAsync(JobRunnableContext context)
    {
        var clientFactory = context.GetRequiredService<IHttpClientFactory>();

        var client = clientFactory.CreateClient();
        var requestMessage = BuildRequestMessage(context);

        var response = await client.SendAsync(
               requestMessage,
               HttpCompletionOption.ResponseHeadersRead);

        var stringContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode && stringContent.IsNullOrWhiteSpace())
        {
            context.SetResult($"HttpStatusCode: {(int)response.StatusCode}, Reason: {response.ReasonPhrase}");
            return;
        }
        context.SetResult(stringContent);
    }

    protected virtual HttpRequestMessage BuildRequestMessage(JobRunnableContext context)
    {
        var url = context.GetString(PropertyUrl);
        var method = context.GetString(PropertyMethod);
        context.TryGetJobData(PropertyData, out var data);
        context.TryGetJobData(PropertyContentType, out var contentType);

        var jsonSerializer = context.GetRequiredService<IJsonSerializer>();

        var httpRequestMesasge = new HttpRequestMessage(new HttpMethod(method), url);
        if (data != null)
        {
            // TODO: 需要支持表单类型

            // application/json 支持
            httpRequestMesasge.Content = new StringContent(
                jsonSerializer.Serialize(data),
                Encoding.UTF8,
                contentType?.ToString() ?? MimeTypes.Application.Json);
        }
        if (context.TryGetJobData(PropertyHeaders, out var headers))
        {
            var headersDic = new Dictionary<string, object>();
            if (headers is string headerString)
            {
                try
                {
                    headersDic = jsonSerializer.Deserialize<Dictionary<string, object>>(headerString);
                }
                catch { }
            }
            foreach (var header in headersDic)
            {
                httpRequestMesasge.Headers.Add(header.Key, header.Value.ToString());
            }
        }
        // TODO: 和 headers 一起?
        if (context.TryGetString(PropertyToken, out var accessToken))
        {
            httpRequestMesasge.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        return httpRequestMesasge;
    }
}
