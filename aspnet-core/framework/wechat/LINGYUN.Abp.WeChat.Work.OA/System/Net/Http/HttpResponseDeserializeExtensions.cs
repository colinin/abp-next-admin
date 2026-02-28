using LINGYUN.Abp.WeChat.Work;
using Newtonsoft.Json;
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

        return JsonConvert.DeserializeObject<T>(responseContent)!;
    }
}
