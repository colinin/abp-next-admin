using Newtonsoft.Json;

namespace LINGYUN.Abp.PushPlus.Token;

public class PushPlusToken
{
    /// <summary>
    /// 访问令牌，后续请求需加到header中
    /// </summary>
    [JsonProperty("accessKey")]
    public string AccessKey { get; set; }
    /// <summary>
    /// 过期时间，过期后需要重新获取
    /// </summary>
    [JsonProperty("expiresIn")]
    public int ExpiresIn { get; set; }

    public PushPlusToken()
    {

    }

    public PushPlusToken(string accessKey, int expiresIn)
    {
        AccessKey = accessKey;
        ExpiresIn = expiresIn;
    }
}
