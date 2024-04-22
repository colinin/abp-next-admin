using LINGYUN.Abp.WeChat.Work;
using Newtonsoft.Json;

namespace System;
internal static class WeChatWorkResponseDeserializeExtensions
{
    public static T DeserializeObject<T>(this string responseContent) where T : WeChatWorkResponse
    {
        return JsonConvert.DeserializeObject<T>(responseContent);
    }
}
