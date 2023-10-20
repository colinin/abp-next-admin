namespace System.Net.Http;

internal static class IHttpClientFactoryExtensions
{
    public static HttpClient GetTuiJuheClient(
        this IHttpClientFactory httpClientFactory)
    {
        return httpClientFactory.CreateClient("_Abp_TuiJuhe_Client");
    }
}
