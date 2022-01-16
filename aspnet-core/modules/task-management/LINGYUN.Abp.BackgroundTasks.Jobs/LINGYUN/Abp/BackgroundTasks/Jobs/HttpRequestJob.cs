using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Net.Http;
using System.Threading.Tasks;

namespace LINGYUN.Abp.BackgroundTasks.Jobs;

public class HttpRequestJob : HttpRequestJobBase, IJobRunnable
{
    public const string PropertyUrl = "url";
    public const string PropertyMethod = "method";
    public const string PropertyData = "data";
    public const string PropertyContentType = "contentType";
    public const string PropertyHeaders = "headers";
    public const string PropertyToken = "token";

    public virtual async Task ExecuteAsync(JobRunnableContext context)
    {
        InitJob(context);

        var url = context.GetString(PropertyUrl);
        var method = context.GetString(PropertyMethod);
        context.TryGetJobData(PropertyData, out var data);
        context.TryGetString(PropertyContentType, out var contentType);

        var headers = new Dictionary<string, string>();
        if (context.TryGetJobData(PropertyHeaders, out var headerInput))
        {
            if (headerInput is string headerString)
            {
                try
                {
                    headers = Deserialize<Dictionary<string, string>>(headerString);
                }
                catch { }
            }
        }

        var response = await RequestAsync(
            context,
            new HttpMethod(method),
            url,
            data,
            contentType,
            headers.ToImmutableDictionary());

        var stringContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode && stringContent.IsNullOrWhiteSpace())
        {
            context.SetResult($"HttpStatusCode: {(int)response.StatusCode}, Reason: {response.ReasonPhrase}");
            return;
        }
        context.SetResult(stringContent);
    }
}
