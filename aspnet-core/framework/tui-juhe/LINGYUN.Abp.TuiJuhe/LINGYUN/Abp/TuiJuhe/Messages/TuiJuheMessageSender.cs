using JetBrains.Annotations;
using LINGYUN.Abp.Features.LimitValidation;
using LINGYUN.Abp.TuiJuhe.Features;
using LINGYUN.Abp.TuiJuhe.Token;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;
using Volo.Abp.Json;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.TuiJuhe.Messages;

[RequiresFeature(TuiJuheFeatureNames.Enable)]
public class TuiJuheMessageSender : ITuiJuheMessageSender, ITransientDependency
{
    protected IJsonSerializer JsonSerializer { get; }
    protected ISettingProvider SettingProvider { get; }
    protected IHttpClientFactory HttpClientFactory { get; }
    protected ITuiJuheTokenProvider TokenProvider { get; }

    public TuiJuheMessageSender(
        IJsonSerializer jsonSerializer, 
        ISettingProvider settingProvider,
        IHttpClientFactory httpClientFactory,
        ITuiJuheTokenProvider tokenProvider)
    {
        JsonSerializer = jsonSerializer;
        SettingProvider = settingProvider;
        HttpClientFactory = httpClientFactory;
        TokenProvider = tokenProvider;
    }

    // 消息发送频率（请求限制）：单位小时内不超过50次
    [RequiresFeature(TuiJuheFeatureNames.Message.Enable)]
    [RequiresLimitFeature(
        TuiJuheFeatureNames.Message.SendLimit,
        TuiJuheFeatureNames.Message.SendLimitInterval,
        LimitPolicy.Hours)]
    public async virtual Task<TuiJuheResult<object, object>> SendAsync(
        [NotNull] string title,
        [NotNull] string content,
        [NotNull] string serviceId, 
        MessageContentType contentType = MessageContentType.Text, 
        CancellationToken cancellationToken = default)
    {
        var token = await TokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.GetTuiJuheClient();

        var resultContent = await client.SendMessageAsync(
            token,
            title,
            content,
            serviceId,
            GetDocType(contentType),
            cancellationToken);

        var response = JsonSerializer
            .Deserialize<TuiJuheResult<object, object>>(resultContent);

        return response;
    }

    private static string GetDocType(MessageContentType contentType)
    {
        return contentType switch
        {
            MessageContentType.Html => "html",
            MessageContentType.Text => "txt",
            MessageContentType.Json => "json",
            MessageContentType.Markdown => "markdown",
            _ => "html",
        };
    }
}
