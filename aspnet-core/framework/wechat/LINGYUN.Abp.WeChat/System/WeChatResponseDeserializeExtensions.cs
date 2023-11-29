using LINGYUN.Abp.WeChat;
using Newtonsoft.Json;

namespace System;
public static class WeChatResponseDeserializeExtensions
{
    public static T DeserializeObject<T>(this string responseContent) where T : WeChatResponse
    {
        return JsonConvert.DeserializeObject<T>(responseContent);
    }
}
