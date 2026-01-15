using LINGYUN.Abp.WeChat.Work.Contacts.Departments.Request;
using LINGYUN.Abp.WeChat.Work.Contacts.Departments.Response;
using LINGYUN.Abp.WeChat.Work.Contacts.Features;
using LINGYUN.Abp.WeChat.Work.Contacts.Token;
using LINGYUN.Abp.WeChat.Work.Token;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;
using Volo.Abp.Json;

namespace LINGYUN.Abp.WeChat.Work.Contacts.Departments;

[RequiresFeature(WeChatWorkContactsFeatureNames.Enable)]
public class WeChatWorkDepartmentProvider : IWeChatWorkDepartmentProvider, ISingletonDependency
{
    protected IJsonSerializer JsonSerializer { get; }
    protected IHttpClientFactory HttpClientFactory { get; }
    protected IWeChatWorkTokenProvider WeChatWorkTokenProvider { get; }
    protected IWeChatWorkContactTokenProvider WeChatWorkContactTokenProvider { get; }

    public WeChatWorkDepartmentProvider(
        IJsonSerializer jsonSerializer,
        IHttpClientFactory httpClientFactory,
        IWeChatWorkTokenProvider weChatWorkTokenProvider,
        IWeChatWorkContactTokenProvider weChatWorkContactTokenProvider)
    {
        JsonSerializer = jsonSerializer;
        HttpClientFactory = httpClientFactory;
        WeChatWorkTokenProvider = weChatWorkTokenProvider;
        WeChatWorkContactTokenProvider = weChatWorkContactTokenProvider;
    }

    public async virtual Task<WeChatWorkGetDepartmentListResponse> GetDepartmentListAsync(
        WeChatWorkGetDepartmentListRequest request,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        return await client.GetDepartmentListAsync(token.AccessToken, request, cancellationToken);
    }

    public async virtual Task<WeChatWorkGetSubDepartmentListResponse> GetSubDepartmentListAsync(
        WeChatWorkGetSubDepartmentListRequest request,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        return await client.GetSubDepartmentListAsync(token.AccessToken, request, cancellationToken);
    }

    public async virtual Task<WeChatWorkGetDepartmentResponse> GetDepartmentAsync(
        WeChatWorkGetDepartmentRequest request,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        return await client.GetDepartmentAsync(token.AccessToken, request, cancellationToken);
    }

    public async virtual Task<WeChatWorkCreateDepartmentResponse> CreateDepartmentAsync(
        WeChatWorkCreateDepartmentRequest request,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        var token = await WeChatWorkContactTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        return await client.CreateDepartmentAsync(token.AccessToken, request, cancellationToken);
    }

    public async virtual Task<WeChatWorkResponse> UpdateDepartmentAsync(
        WeChatWorkUpdateDepartmentRequest request,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        var token = await WeChatWorkContactTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        return await client.UpdateDepartmentAsync(token.AccessToken, request, cancellationToken);
    }

    public async virtual Task<WeChatWorkResponse> DeleteDepartmentAsync(
        WeChatWorkDeleteDepartmentRequest request,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        var token = await WeChatWorkContactTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        return await client.DeleteDepartmentAsync(token.AccessToken, request, cancellationToken);
    }
}
