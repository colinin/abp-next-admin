using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;

namespace LINGYUN.Abp.Sonatype.Nexus.Services.CoreUI;

public class CoreUiServiceProxy : ICoreUiServiceProxy, ISingletonDependency
{
    private static int _idCurrent = 1;
    protected IJsonSerializer JsonSerializer { get; }
    protected IHttpClientFactory HttpClientFactory { get; }
    protected AbpSonatypeNexusOptions Options { get; }

    public CoreUiServiceProxy(
        IJsonSerializer jsonSerializer,
        IHttpClientFactory httpClientFactory,
        IOptions<AbpSonatypeNexusOptions> options)
    {
        Options = options.Value;
        JsonSerializer = jsonSerializer;
        HttpClientFactory = httpClientFactory;
    }

    public async virtual Task<CoreUIResponse<TResult>> SearchAsync<TData, TResult>(
        CoreUIRequest<TData> request, 
        CancellationToken cancellationToken = default)
    {
        Interlocked.Increment(ref _idCurrent);
        request.Tid = _idCurrent;
        var client = HttpClientFactory.CreateClient(SonatypeNexusConsts.ApiClient);

        using var requestContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/service/extdirect")
        {
            Content = requestContent,
        };

        var response = await client.SendAsync(requestMessage, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            throw new AbpException(response.ReasonPhrase);
        }

        var responseContent = await response.Content.ReadAsStringAsync();
        var coreUiResponse = JsonSerializer.Deserialize<CoreUIResponse<TResult>>(responseContent);

        return coreUiResponse;
    }
}
