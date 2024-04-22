using Newtonsoft.Json;

namespace System;
internal static class WeChatObjectSerializeExtensions
{
    public static string SerializeToJson(this object @object)
    {
        return JsonConvert.SerializeObject(@object);
    }
}
