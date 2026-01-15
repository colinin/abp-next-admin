using LINGYUN.Abp.WeChat.Work;
using LINGYUN.Abp.WeChat.Work.OA.MeetingRooms.Request;
using LINGYUN.Abp.WeChat.Work.OA.MeetingRooms.Response;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;

internal static partial class HttpClientWeChatWorkRequestExtensions
{
    public async static Task<WeChatWorkCreateMeetingRoomResponse> CreateMeetingRoomAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkCreateMeetingRoomRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/oa/meetingroom/add");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        using var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson())
        };

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkCreateMeetingRoomResponse>();
    }

    public async static Task<WeChatWorkGetMeetingRoomListResponse> GetMeetingRoomListAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkGetMeetingRoomListRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/oa/meetingroom/list");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        using var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson())
        };

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkGetMeetingRoomListResponse>();
    }

    public async static Task<WeChatWorkResponse> UpdateMeetingRoomAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkUpdateMeetingRoomRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/oa/meetingroom/edit");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        using var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson())
        };

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkResponse>();
    }

    public async static Task<WeChatWorkResponse> DeleteMeetingRoomAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkDeleteMeetingRoomRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/oa/meetingroom/del");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        using var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson())
        };

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkResponse>();
    }

    public async static Task<WeChatWorkGetMeetingRoomBookingListResponse> GetMeetingRoomBookingListAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkGetMeetingRoomBookingListRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/oa/meetingroom/get_booking_info");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        using var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson())
        };

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkGetMeetingRoomBookingListResponse>();
    }

    public async static Task<WeChatWorkBookMeetingRoomResponse> BookMeetingRoomAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkBookMeetingRoomRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/oa/meetingroom/book");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        using var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson())
        };

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkBookMeetingRoomResponse>();
    }

    public async static Task<WeChatWorkBookMeetingRoomByScheduleResponse> BookMeetingRoomByScheduleAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkBookMeetingRoomByScheduleRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/oa/meetingroom/book_by_schedule");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        using var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson())
        };

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkBookMeetingRoomByScheduleResponse>();
    }

    public async static Task<WeChatWorkBookMeetingRoomByMeetingResponse> BookMeetingRoomByMeetingAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkBookMeetingRoomByMeetingRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/oa/meetingroom/book_by_meeting");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        using var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson())
        };

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkBookMeetingRoomByMeetingResponse>();
    }

    public async static Task<WeChatWorkResponse> CancelBookMeetingRoomAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkCancelBookMeetingRoomRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/oa/meetingroom/cancel_book");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        using var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson())
        };

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkResponse>();
    }

    public async static Task<WeChatWorkGetMeetingRoomBookResponse> GetMeetingRoomBookAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkGetMeetingRoomBookRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/oa/meetingroom/bookinfo/get");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        using var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson())
        };

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkGetMeetingRoomBookResponse>();
    }
}
