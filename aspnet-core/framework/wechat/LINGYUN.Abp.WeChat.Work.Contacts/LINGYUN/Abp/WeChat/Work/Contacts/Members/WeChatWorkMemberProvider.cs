using LINGYUN.Abp.WeChat.Work.Contacts.Features;
using LINGYUN.Abp.WeChat.Work.Contacts.Members.Models;
using LINGYUN.Abp.WeChat.Work.Contacts.Members.Request;
using LINGYUN.Abp.WeChat.Work.Contacts.Members.Response;
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

namespace LINGYUN.Abp.WeChat.Work.Contacts.Members;

[RequiresFeature(WeChatWorkContactsFeatureNames.Enable)]
public class WeChatWorkMemberProvider : IWeChatWorkMemberProvider, ISingletonDependency
{
    protected IJsonSerializer JsonSerializer { get; }
    protected IHttpClientFactory HttpClientFactory { get; }
    protected IWeChatWorkTokenProvider WeChatWorkTokenProvider { get; }
    protected IWeChatWorkContactTokenProvider WeChatWorkContactTokenProvider { get; }

    public WeChatWorkMemberProvider(
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

    public async virtual Task<WeChatWorkGetMemberResponse> GetMemberAsync(
        string userId,
        CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(userId, nameof(userId), 64, 1);

        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        return await client.GetMemberAsync(token.AccessToken, userId, cancellationToken);
    }

    public async virtual Task<WeChatWorkCreateMemberResponse> CreateMemberAsync(
        WeChatWorkCreateMemberRequest request,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        var token = await WeChatWorkContactTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        return await client.CreateMemberAsync(token.AccessToken, request, cancellationToken);
    }

    public async virtual Task<WeChatWorkResponse> UpdateMemberAsync(
        WeChatWorkUpdateMemberRequest request,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        var token = await WeChatWorkContactTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        return await client.UpdateMemberAsync(token.AccessToken, request, cancellationToken);
    }

    public async virtual Task<WeChatWorkResponse> DeleteMemberAsync(
        string userId,
        CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(userId, nameof(userId), 64, 1);

        var token = await WeChatWorkContactTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        return await client.DeleteMemberAsync(token.AccessToken, userId, cancellationToken);
    }

    public async virtual Task<WeChatWorkResponse> BulkDeleteMemberAsync(
        WeChatWorkBulkDeleteMemberRequest request,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        var token = await WeChatWorkContactTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        return await client.BulkDeleteMemberAsync(token.AccessToken, request, cancellationToken);
    }

    public async virtual Task<WeChatWorkGetSimpleMemberListResponse> GetSimpleMemberListAsync(
        int departmentId,
        CancellationToken cancellationToken = default)
    {
        Check.NotDefaultOrNull<int>(departmentId, nameof(departmentId));

        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        return await client.GetSimpleMemberListAsync(token.AccessToken, departmentId, cancellationToken);
    }

    public async virtual Task<WeChatWorkGetMemberListResponse> GetMemberListAsync(
        int departmentId,
        CancellationToken cancellationToken = default)
    {
        Check.NotDefaultOrNull<int>(departmentId, nameof(departmentId));

        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        return await client.GetMemberListAsync(token.AccessToken, departmentId, cancellationToken);
    }

    public async virtual Task<WeChatWorkConvertToOpenIdResponse> ConvertToOpenIdAsync(
        WeChatWorkConvertToOpenIdRequest request,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        return await client.ConvertToOpenIdAsync(token.AccessToken, request, cancellationToken);
    }

    public async virtual Task<WeChatWorkConvertToUserIdResponse> ConvertToUserIdAsync(
        WeChatWorkConvertToUserIdRequest request,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        return await client.ConvertToUserIdAsync(token.AccessToken, request, cancellationToken);
    }

    public async virtual Task<WeChatWorkResponse> AuthSuccessAsync(
        string userId,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(userId, nameof(userId));

        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        return await client.AuthSuccessAsync(token.AccessToken, userId, cancellationToken);
    }

    public async virtual Task<WeChatWorkBulkInviteMemberResponse> BulkInviteMemberAsync(
        WeChatWorkBulkInviteMemberRequest request,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        return await client.BulkInviteMemberAsync(token.AccessToken, request, cancellationToken);
    }

    public async virtual Task<WeChatWorkGetJoinQrCodeResponse> GetJoinQrCodeAsync(
        QrCodeSizeType sizeType,
        CancellationToken cancellationToken = default)
    {
        var token = await WeChatWorkContactTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        return await client.GetJoinQrCodeAsync(token.AccessToken, sizeType, cancellationToken);
    }

    public async virtual Task<WeChatWorkGetUserIdResponse> GetUserIdByMobileAsync(
        WeChatWorkGetUserIdByMobileRequest request,
        CancellationToken cancellationToken = default)
    {
        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        return await client.GetUserIdByMobileAsync(token.AccessToken, request, cancellationToken);
    }

    public async virtual Task<WeChatWorkGetUserIdResponse> GetUserIdByEmailAsync(
        WeChatWorkGetUserIdByEmailRequest request,
        CancellationToken cancellationToken = default)
    {
        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        return await client.GetUserIdByEmailAsync(token.AccessToken, request, cancellationToken);
    }

    public async virtual Task<WeChatWorkGetUserIdListResponse> GetUserIdListAsync(
        WeChatWorkGetUserIdListRequest request,
        CancellationToken cancellationToken = default)
    {
        var token = await WeChatWorkContactTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        return await client.GetUserIdListAsync(token.AccessToken, request, cancellationToken);
    }
}
