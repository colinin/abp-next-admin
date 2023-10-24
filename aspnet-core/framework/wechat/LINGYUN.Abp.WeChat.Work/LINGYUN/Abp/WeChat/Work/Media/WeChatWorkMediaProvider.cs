using LINGYUN.Abp.WeChat.Work.Media.Models;
using LINGYUN.Abp.WeChat.Work.Token;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Content;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.WeChat.Work.Media;
public class WeChatWorkMediaProvider : IWeChatWorkMediaProvider, ISingletonDependency
{
    protected IHttpClientFactory HttpClientFactory { get; }
    protected IWeChatWorkTokenProvider WeChatWorkTokenProvider { get; }

    public WeChatWorkMediaProvider(
        IHttpClientFactory httpClientFactory,
        IWeChatWorkTokenProvider weChatWorkTokenProvider)
    {
        HttpClientFactory = httpClientFactory;
        WeChatWorkTokenProvider = weChatWorkTokenProvider;
    }

    public async virtual Task<IRemoteStreamContent> GetAsync(
        string agentId,
        string mediaId,
        CancellationToken cancellationToken = default)
    {
        var token = await WeChatWorkTokenProvider.GetTokenAsync(agentId, cancellationToken);
        var client = HttpClientFactory.CreateClient(AbpWeChatWorkGlobalConsts.ApiClient);

        using var response = await client.GetMediaAsync(
            token.AccessToken,
            mediaId,
            cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var errorResponse = await response.DeserializeObjectAsync<WeChatWorkResponse>();
            errorResponse.ThrowIfNotSuccess();
        }

        var mediaStream = await response.Content.ReadAsStreamAsync();
        string fileName = null;
        string contentType = null;

        if (response.Headers.TryGetValues("Content-Disposition", out var contentDispositions))
        {
            var contentDisposition = contentDispositions.FirstOrDefault();
            if (!contentDisposition.IsNullOrWhiteSpace())
            {
                var startIndex = contentDisposition.IndexOf("filename=", StringComparison.OrdinalIgnoreCase);
                if (startIndex >= 0)
                {
                    startIndex += "filename=".Length;
                    var endIndex = contentDisposition.IndexOf(";", startIndex);
                    if (endIndex < 0)
                    {
                        endIndex = contentDisposition.Length;
                    }
                    fileName = contentDisposition.Substring(startIndex, endIndex - startIndex).Trim('\"');
                }
            }
        }

        if (response.Headers.TryGetValues("Content-Type", out var contentTypes))
        {
            contentType = contentTypes.FirstOrDefault();
        }

        return new RemoteStreamContent(
            mediaStream,
            fileName: fileName,
            contentType: contentType);
    }

    public async virtual Task<WeChatWorkMediaResponse> UploadAsync(
        string agentId,
        string type, 
        IRemoteStreamContent media, 
        CancellationToken cancellationToken = default)
    {
        var token = await WeChatWorkTokenProvider.GetTokenAsync(agentId, cancellationToken);
        var client = HttpClientFactory.CreateClient(AbpWeChatWorkGlobalConsts.ApiClient);

        var request = new WeChatWorkMediaRequest(
            token.AccessToken,
            media);

        using var response = await client.UploadMediaAsync(type, request, cancellationToken);
        var mediaRespose = await response.DeserializeObjectAsync<WeChatWorkMediaResponse>();
        mediaRespose.ThrowIfNotSuccess();

        return mediaRespose;
    }

    public async virtual Task<WeChatWorkImageResponse> UploadImageAsync(
        string agentId,
        IRemoteStreamContent image,
        CancellationToken cancellationToken = default)
    {
        var token = await WeChatWorkTokenProvider.GetTokenAsync(agentId, cancellationToken);
        var client = HttpClientFactory.CreateClient(AbpWeChatWorkGlobalConsts.ApiClient);
        var request = new WeChatWorkMediaRequest(
            token.AccessToken,
            image);

        using var response = await client.UploadImageAsync(request, cancellationToken);
        var mediaRespose = await response.DeserializeObjectAsync<WeChatWorkImageResponse>();
        mediaRespose.ThrowIfNotSuccess();

        return mediaRespose;
    }
}
