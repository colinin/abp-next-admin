using Newtonsoft.Json;
using System;

namespace LINGYUN.Abp.WxPusher.QrCode;

[Serializable]
public class GetScanQrCodeResult
{
    [JsonProperty("appId")]
    public int AppId { get; set; }

    [JsonProperty("appKey")]
    public string AppKey { get; set; }

    [JsonProperty("appName")]
    public string AppName { get; set; }

    [JsonProperty("extra")]
    public string Extra { get; set; }

    [JsonProperty("source")]
    public string Source { get; set; }

    [JsonProperty("time")]
    public long Time { get; set; }

    [JsonProperty("uid")]
    public string Uid { get; set; }

    [JsonProperty("userHeadImg")]
    public string UserHeadImg { get; set; }

    [JsonProperty("userName")]
    public string UserName { get; set; }
}
