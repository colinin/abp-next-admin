using LINGYUN.Abp.WeChat.Work.OA.Features;
using LINGYUN.Abp.WeChat.Work.OA.MeetingRooms.Request;
using LINGYUN.Abp.WeChat.Work.OA.MeetingRooms.Response;
using LINGYUN.Abp.WeChat.Work.Token;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;

namespace LINGYUN.Abp.WeChat.Work.OA.MeetingRooms;

[RequiresFeature(WeChatWorkOAFeatureNames.Enable)]
public class WeChatWorkMeetingRoomProvider : IWeChatWorkMeetingRoomProvider, ISingletonDependency
{
    protected IHttpClientFactory HttpClientFactory { get; }
    protected IWeChatWorkTokenProvider WeChatWorkTokenProvider { get; }

    public WeChatWorkMeetingRoomProvider(
        IHttpClientFactory httpClientFactory,
        IWeChatWorkTokenProvider weChatWorkTokenProvider)
    {
        HttpClientFactory = httpClientFactory;
        WeChatWorkTokenProvider = weChatWorkTokenProvider;
    }

    public async virtual Task<WeChatWorkCreateMeetingRoomResponse> CreateMeetingRoomAsync(
        WeChatWorkCreateMeetingRoomRequest request,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        return await client.CreateMeetingRoomAsync(token.AccessToken, request, cancellationToken);
    }

    public async virtual Task<WeChatWorkGetMeetingRoomListResponse> GetMeetingRoomListAsync(
        WeChatWorkGetMeetingRoomListRequest request,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        return await client.GetMeetingRoomListAsync(token.AccessToken, request, cancellationToken);
    }

    public async virtual Task<WeChatWorkResponse> UpdateMeetingRoomAsync(
        WeChatWorkUpdateMeetingRoomRequest request,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        return await client.UpdateMeetingRoomAsync(token.AccessToken, request, cancellationToken);
    }

    public async virtual Task<WeChatWorkResponse> DeleteMeetingRoomAsync(
        WeChatWorkDeleteMeetingRoomRequest request,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        return await client.DeleteMeetingRoomAsync(token.AccessToken, request, cancellationToken);
    }

    public async virtual Task<WeChatWorkGetMeetingRoomBookingListResponse> GetMeetingRoomBookingListAsync(
        WeChatWorkGetMeetingRoomBookingListRequest request,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        return await client.GetMeetingRoomBookingListAsync(token.AccessToken, request, cancellationToken);
    }

    public async virtual Task<WeChatWorkBookMeetingRoomResponse> BookMeetingRoomAsync(
        WeChatWorkBookMeetingRoomRequest request,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        return await client.BookMeetingRoomAsync(token.AccessToken, request, cancellationToken);
    }

    public async virtual Task<WeChatWorkBookMeetingRoomByScheduleResponse> BookMeetingRoomByScheduleAsync(
        WeChatWorkBookMeetingRoomByScheduleRequest request,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        return await client.BookMeetingRoomByScheduleAsync(token.AccessToken, request, cancellationToken);
    }

    public async virtual Task<WeChatWorkBookMeetingRoomByMeetingResponse> BookMeetingRoomByMeetingAsync(
        WeChatWorkBookMeetingRoomByMeetingRequest request,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        return await client.BookMeetingRoomByMeetingAsync(token.AccessToken, request, cancellationToken);
    }

    public async virtual Task<WeChatWorkResponse> CancelBookMeetingRoomAsync(
        WeChatWorkCancelBookMeetingRoomRequest request,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        return await client.CancelBookMeetingRoomAsync(token.AccessToken, request, cancellationToken);
    }

    public async virtual Task<WeChatWorkGetMeetingRoomBookResponse> GetMeetingRoomBookAsync(
        WeChatWorkGetMeetingRoomBookRequest request,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(request, nameof(request));

        var token = await WeChatWorkTokenProvider.GetTokenAsync(cancellationToken);
        var client = HttpClientFactory.CreateWeChatWorkApiClient();

        return await client.GetMeetingRoomBookAsync(token.AccessToken, request, cancellationToken);
    }
}
