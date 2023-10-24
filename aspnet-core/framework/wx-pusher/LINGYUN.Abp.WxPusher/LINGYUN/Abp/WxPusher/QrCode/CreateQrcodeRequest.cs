using JetBrains.Annotations;
using Newtonsoft.Json;
using System;
using Volo.Abp;

namespace LINGYUN.Abp.WxPusher.QrCode;

[Serializable]
public class CreateQrcodeRequest
{
    /// <summary>
    /// 应用的标志
    /// </summary>
    [JsonProperty("appToken")]
    public string AppToken { get; }
    /// <summary>
    /// 二维码携带的参数，最长64位
    /// </summary>
    [JsonProperty("extra")]
    public string Extra { get; }
    /// <summary>
    /// 二维码有效时间，s为单位，最大30天。
    /// </summary>
    [JsonProperty("validTime")]
    public int ValidTime { get; }

    public CreateQrcodeRequest(
        [NotNull] string appToken,
        [NotNull] string extra, 
        int validTime = 1800)
    {
        Check.NotNullOrWhiteSpace(appToken, nameof(appToken));
        Check.NotNullOrWhiteSpace(extra, nameof(extra), 64);

        AppToken = appToken;
        Extra = extra;
        ValidTime = validTime;
    }
}
