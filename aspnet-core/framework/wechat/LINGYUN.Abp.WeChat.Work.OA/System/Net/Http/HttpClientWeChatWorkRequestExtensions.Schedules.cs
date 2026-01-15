using LINGYUN.Abp.WeChat.Work;
using LINGYUN.Abp.WeChat.Work.OA.Schedules.Request;
using LINGYUN.Abp.WeChat.Work.OA.Schedules.Response;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;

internal static partial class HttpClientWeChatWorkRequestExtensions
{
    public async static Task<WeChatWorkCreateCalendarResponse> CreateCalendarAsync(
    this HttpMessageInvoker client,
    string accessToken,
    WeChatWorkCreateCalendarRequest request,
    CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/oa/calendar/add");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        using var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson())
        };

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkCreateCalendarResponse>();
    }

    public async static Task<WeChatWorkUpdateCalendarResponse> UpdateCalendarAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkUpdateCalendarRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/oa/calendar/update");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        using var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson())
        };

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkUpdateCalendarResponse>();
    }

    public async static Task<WeChatWorkGetCalendarListResponse> GetCalendarListAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkGetCalendarListRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/oa/calendar/get");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        using var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson())
        };

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkGetCalendarListResponse>();
    }

    public async static Task<WeChatWorkResponse> DeleteCalendarAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkDeleteCalendarRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/oa/calendar/del");
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

    public async static Task<WeChatWorkCreateScheduleResponse> CreateScheduleAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkCreateScheduleRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/oa/schedule/add");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        using var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson())
        };

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkCreateScheduleResponse>();
    }

    public async static Task<WeChatWorkUpdateScheduleResponse> UpdateScheduleAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkUpdateScheduleRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/oa/schedule/update");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        using var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson())
        };

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkUpdateScheduleResponse>();
    }

    public async static Task<WeChatWorkResponse> AddAttendeesAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkScheduleAddAttendeesRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/oa/schedule/add_attendees");
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

    public async static Task<WeChatWorkResponse> DeleteAttendeesAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkScheduleDeleteAttendeesRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/oa/schedule/del_attendees");
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

    public async static Task<WeChatWorkGetScheduleListByCalendarResponse> GetScheduleListByCalendarAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkGetScheduleListByCalendarRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/oa/schedule/get_by_calendar");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        using var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson())
        };

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkGetScheduleListByCalendarResponse>();
    }

    public async static Task<WeChatWorkGetScheduleListResponse> GetScheduleListAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkGetScheduleListRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/oa/schedule/get");
        urlBuilder.AppendFormat("?access_token={0}", accessToken);

        using var httpRequest = new HttpRequestMessage(
           HttpMethod.Post,
           urlBuilder.ToString())
        {
            Content = new StringContent(request.SerializeToJson())
        };

        using var httpResponse = await client.SendAsync(httpRequest, cancellationToken);

        return await httpResponse.DeserializeObjectAsync<WeChatWorkGetScheduleListResponse>();
    }

    public async static Task<WeChatWorkResponse> DeleteScheduleAsync(
        this HttpMessageInvoker client,
        string accessToken,
        WeChatWorkDeleteScheduleRequest request,
        CancellationToken cancellationToken = default)
    {
        var urlBuilder = new StringBuilder();
        urlBuilder.Append("/cgi-bin/oa/schedule/del");
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
}
