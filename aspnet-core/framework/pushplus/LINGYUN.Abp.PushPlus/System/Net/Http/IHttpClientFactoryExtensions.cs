namespace System.Net.Http;

internal static class IHttpClientFactoryExtensions
{
    public static HttpClient GetPushPlusClient(
        this IHttpClientFactory httpClientFactory)
    {
        return httpClientFactory.CreateClient("_Abp_PushPlus_Client");
    }
}
