using LINGYUN.Abp.WeChat;
using LINGYUN.Abp.WeChat.Common;
using System.Threading.Tasks;

namespace System.Net.Http;
public static class HttpResponseDeserializeExtensions
{
    public static void ThrowNotSuccessStatusCode(this HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            throw new AbpWeChatException($"Wechat request error: {response.StatusCode} - {response.ReasonPhrase}");
        }
    }

    public async static Task<T> DeserializeObjectAsync<T>(this HttpResponseMessage response) where T : WeChatResponse
    {
        response.ThrowNotSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();

        return responseContent.DeserializeObject<T>();
    }
}
