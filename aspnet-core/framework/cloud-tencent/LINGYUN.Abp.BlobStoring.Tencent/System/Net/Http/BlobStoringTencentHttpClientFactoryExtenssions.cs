using LINGYUN.Abp.BlobStoring.Tencent;

namespace System.Net.Http;
public static class BlobStoringTencentHttpClientFactoryExtenssions
{
    public static HttpClient CreateTenantOssClient(
        this IHttpClientFactory httpClientFactory)
    {
        return httpClientFactory.CreateClient(BlobStoringTencentConsts.HttpClient); ;
    }
}
