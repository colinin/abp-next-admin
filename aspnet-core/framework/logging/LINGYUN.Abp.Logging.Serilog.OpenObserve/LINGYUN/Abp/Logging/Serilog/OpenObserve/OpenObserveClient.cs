using LINGYUN.Abp.Logging.Serilog.OpenObserve.Models;
using LINGYUN.Abp.Logging.Serilog.OpenObserve.Models.Search;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Json;

namespace LINGYUN.Abp.Logging.Serilog.OpenObserve;

public partial class OpenObserveClient
{
    protected HttpClient HttpClient { get; }
    protected IJsonSerializer JsonSerializer { get; }
    protected AbpLoggingSerilogOpenObserveOptions Options { get; }
    public OpenObserveClient(
        HttpClient httpClient,
        IJsonSerializer jsonSerializer,
        IOptions<AbpLoggingSerilogOpenObserveOptions> options)
    {
        HttpClient = httpClient;
        Options = options.Value;
        JsonSerializer = jsonSerializer;
    }

    public async virtual Task<SearchResponse<THint>> SearchAsync<THint>(
        SearchQuery query,
        SearchType? searchType = null,
        int? timeout = OpenObserveConstants.ZO_QUERY_TIMEOUT,
        SearchAgentOptions? agentOptions = null, 
        CancellationToken cancellationToken = default)
    {
        var httpResponse = await HttpClient.PostAsJsonAsync(
            $"/api/{Options.Organization}/_search",
            new SearchRequest(query, searchType, timeout, agentOptions),
            cancellationToken);

        if (httpResponse.IsSuccessStatusCode)
        {
            var responseContent = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
            return JsonSerializer.Deserialize<SearchResponse<THint>>(responseContent);
        }

        var statusCode = (int)httpResponse.StatusCode;
        if (statusCode >= 400 && statusCode < 500)
        {
            var responseContent = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
            var errorResponse = JsonSerializer.Deserialize<OpenObserveErrorResponse>(responseContent);

            throw new OpenObserveSearchException(errorResponse);
        }

        throw new OpenObserveRequestException(httpResponse);
    }
}
