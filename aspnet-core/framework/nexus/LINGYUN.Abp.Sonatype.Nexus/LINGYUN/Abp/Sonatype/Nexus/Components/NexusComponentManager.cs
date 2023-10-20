using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;

namespace LINGYUN.Abp.Sonatype.Nexus.Components;
public class NexusComponentManager : INexusComponentManager, ISingletonDependency
{
    protected IJsonSerializer JsonSerializer { get; }
    protected IHttpClientFactory HttpClientFactory { get; }
    protected AbpSonatypeNexusOptions Options { get; }

    public NexusComponentManager(
        IJsonSerializer jsonSerializer,
        IHttpClientFactory httpClientFactory,
        IOptions<AbpSonatypeNexusOptions> options)
    {
        Options = options.Value;
        JsonSerializer = jsonSerializer;
        HttpClientFactory = httpClientFactory;
    }

    public async virtual Task UploadAsync([NotNull] NexusComponentUploadArgs args, CancellationToken cancellationToken = default)
    {
        var client = HttpClientFactory.CreateClient(SonatypeNexusConsts.ApiClient);

        var username = Options.UserName;
        var password = Options.Password;
        var authBase64Data = Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + password));

        using var formDataContent = args.BuildContent();
        var requestMessage = new HttpRequestMessage(
               HttpMethod.Post,
               $"/service/rest/v1/components?repository={args.Repository}")
        {
            Content = formDataContent,
        };
        requestMessage.Headers.Add("Authorization", $"Basic {authBase64Data}");

        var response = await client.SendAsync(requestMessage, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            throw new AbpException(response.ReasonPhrase);
        }
    }

    public async virtual Task<bool> DeleteAsync([NotNull] string id, CancellationToken cancellationToken = default)
    {
        var client = HttpClientFactory.CreateClient(SonatypeNexusConsts.ApiClient);

        var username = Options.UserName;
        var password = Options.Password;
        var authBase64Data = Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + password));

        var requestMessage = new HttpRequestMessage(HttpMethod.Delete, $"/service/rest/v1/components/{id}");
        requestMessage.Headers.Add("Authorization", $"Basic {authBase64Data}");

        var response = await client.SendAsync(requestMessage, cancellationToken);
        return response.IsSuccessStatusCode;
    }

    public async virtual Task<NexusComponent> GetAsync([NotNull] string id, CancellationToken cancellationToken = default)
    {
        var client = HttpClientFactory.CreateClient(SonatypeNexusConsts.ApiClient);

        var response = await client.GetAsync($"/service/rest/v1/components/{id}", cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            throw new AbpException(response.ReasonPhrase);
        }

        var responseContent = await response.Content.ReadAsStringAsync();
        var nexusComponent = JsonSerializer.Deserialize<NexusComponent>(responseContent);

        return nexusComponent;
    }

    public async virtual Task<NexusComponentListResult> ListAsync([NotNull] string repository, string continuationToken = null, CancellationToken cancellationToken = default)
    {
        var client = HttpClientFactory.CreateClient(SonatypeNexusConsts.ApiClient);

        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/service/rest/v1/components");
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
        var nexusComponentListResult = JsonSerializer.Deserialize<NexusComponentListResult>(responseContent);

        return nexusComponentListResult;
    }
}
