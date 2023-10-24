using LINGYUN.Abp.WeChat.Work;
using System.Threading.Tasks;
using Volo.Abp.Http.Client;

namespace System.Net.Http;
internal static class HttpResponseDeserializeExtensions
{
    public async static Task<T> DeserializeObjectAsync<T>(this HttpResponseMessage response) where T : WeChatWorkResponse
    {
        if (!response.IsSuccessStatusCode)
        {
            throw new AbpRemoteCallException($"Wechat work request error: {response.StatusCode} - {response.ReasonPhrase}");
        }
        var responseContent = await response.Content.ReadAsStringAsync();

        return responseContent.DeserializeObject<T>();
    }
}
