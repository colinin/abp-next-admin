using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;

namespace LINGYUN.Abp.Sonatype.Nexus.Assets;
public class NexusAssetManager : INexusAssetManager, ISingletonDependency
{
    protected IJsonSerializer JsonSerializer { get; }
    protected IHttpClientFactory HttpClientFactory { get; }
    protected AbpSonatypeNexusOptions Options { get; }

    public NexusAssetManager(
        IJsonSerializer jsonSerializer,
        IHttpClientFactory httpClientFactory,
        IOptions<AbpSonatypeNexusOptions> options)
    {
        Options = options.Value;
        JsonSerializer = jsonSerializer;
        HttpClientFactory = httpClientFactory;
    }

    public async virtual Task DeleteAsync([NotNull] string id, CancellationToken cancellationToken = default)
    {
        var client = HttpClientFactory.CreateClient(SonatypeNexusConsts.ApiClient);
        var username = Options.UserName;
        var password = Options.Password;
        var authBase64Data = Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + password));

        var requestMessage = new HttpRequestMessage(HttpMethod.Delete, $"/service/rest/v1/assets/{id}");
        requestMessage.Headers.Add("Authorization", $"Basic {authBase64Data}");

        var response = await client.SendAsync(requestMessage, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            throw new AbpException(response.ReasonPhrase);
        }
    }

    public async virtual Task<NexusAsset> GetAsync([NotNull] string id, CancellationToken cancellationToken = default)
    {
        var client = HttpClientFactory.CreateClient(SonatypeNexusConsts.ApiClient);

        var response = await client.GetAsync($"/service/rest/v1/assets/{id}", cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            throw new AbpException(response.ReasonPhrase);
        }

        var responseContent = await response.Content.ReadAsStringAsync();
        var nexusAsset = JsonSerializer.Deserialize<NexusAsset>(responseContent);

        return nexusAsset;
    }

    public async virtual Task<Stream> GetContentOrNullAsync([NotNull] NexusAsset asset, CancellationToken cancellationToken = default)
    {
        if (asset == null || asset.DownloadUrl.IsNullOrWhiteSpace())
        {
            return null;
        }

        var client = HttpClientFactory.CreateClient(SonatypeNexusConsts.ApiClient);

        return await client.GetStreamAsync(asset.DownloadUrl);
    }

    public async virtual Task<NexusAssetListResult> ListAsync([NotNull] string repository, string continuationToken = null, CancellationToken cancellationToken = default)
    {
        var client = HttpClientFactory.CreateClient(SonatypeNexusConsts.ApiClient);

        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/service/rest/v1/assets");
        urlBuilder.AppendFormat("?repository={0}", repository);
        if (!continuationToken.IsNullOrWhiteSpace())
        {
            urlBuilder.AppendFormat("&continuationToken={0}", continuationToken);
        }

        var response = await client.GetAsync(urlBuilder.ToString(), cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            throw new AbpException(response.ReasonPhrase);
        }

        var responseContent = await response.Content.ReadAsStringAsync();
        var nexusAssetListResult = JsonSerializer.Deserialize<NexusAssetListResult>(responseContent);

        return nexusAssetListResult;
    }
}
