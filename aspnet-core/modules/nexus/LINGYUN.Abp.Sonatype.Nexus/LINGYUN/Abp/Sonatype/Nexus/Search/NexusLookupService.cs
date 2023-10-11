using LINGYUN.Abp.Sonatype.Nexus.Assets;
using LINGYUN.Abp.Sonatype.Nexus.Components;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;

namespace LINGYUN.Abp.Sonatype.Nexus.Search;
public class NexusLookupService : INexusLookupService, ISingletonDependency
{
    protected IJsonSerializer JsonSerializer { get; }
    protected IHttpClientFactory HttpClientFactory { get; }
    protected AbpSonatypeNexusOptions Options { get; }

    public NexusLookupService(
        IJsonSerializer jsonSerializer,
        IHttpClientFactory httpClientFactory,
        IOptions<AbpSonatypeNexusOptions> options)
    {
        Options = options.Value;
        JsonSerializer = jsonSerializer;
        HttpClientFactory = httpClientFactory;
    }

    public async virtual Task<NexusAssetListResult> ListAssetAsync(NexusSearchArgs args, CancellationToken cancellationToken = default)
    {
        var client = HttpClientFactory.CreateClient(SonatypeNexusConsts.ApiClient);

        var urlBuilder = new StringBuilder();
        urlBuilder.AppendFormat("/service/rest/v1/search/assets?format={0}", args.Format);
        urlBuilder.AppendFormat("&repository={0}", args.Repository);
        urlBuilder.AppendFormat("&group={0}", args.Group);
        urlBuilder.AppendFormat("&name={0}", args.Name);
        if(!args.Keyword.IsNullOrWhiteSpace())
        {
            urlBuilder.AppendFormat("&q={0}", args.Keyword);
        }
        if (!args.Version.IsNullOrWhiteSpace())
        {
            urlBuilder.AppendFormat("&version={0}", args.Version);
        }
        if (args.Timeout.HasValue)
        {
            urlBuilder.AppendFormat("&timeout={0}", args.Timeout.Value);
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

    public async virtual Task<NexusComponentListResult> ListComponentAsync(NexusSearchArgs args, CancellationToken cancellationToken = default)
    {
        var client = HttpClientFactory.CreateClient(SonatypeNexusConsts.ApiClient);

        var urlBuilder = new StringBuilder();
        urlBuilder.AppendFormat("/service/rest/v1/search?format={0}", args.Format);
        urlBuilder.AppendFormat("&repository={0}", args.Repository);
        urlBuilder.AppendFormat("&group={0}", args.Group);
        urlBuilder.AppendFormat("&name={0}", args.Name);
        if (!args.Keyword.IsNullOrWhiteSpace())
        {
            urlBuilder.AppendFormat("&q={0}", args.Keyword);
        }
        if (!args.Version.IsNullOrWhiteSpace())
        {
            urlBuilder.AppendFormat("&version={0}", args.Version);
        }
        if (args.Timeout.HasValue)
        {
            urlBuilder.AppendFormat("&timeout={0}", args.Timeout.Value);
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
