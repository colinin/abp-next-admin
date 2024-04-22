namespace System.Net.Http;

internal static class IHttpClientFactoryExtensions
{
    public static HttpClient GetWxPusherClient(
        this IHttpClientFactory httpClientFactory)
    {
        return httpClientFactory.CreateClient("_Abp_WxPusher_Client");
    }
}
