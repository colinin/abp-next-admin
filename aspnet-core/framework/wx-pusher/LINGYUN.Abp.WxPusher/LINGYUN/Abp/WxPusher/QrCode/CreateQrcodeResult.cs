using Newtonsoft.Json;
using System;

namespace LINGYUN.Abp.WxPusher.QrCode;

[Serializable]
public class CreateQrcodeResult
{
    [JsonProperty("expires")]
    public long Expires { get; set; }

    [JsonProperty("code")]
    public string Code { get; set; }

    [JsonProperty("shortUrl")]
    public string ShortUrl { get; set; }

    [JsonProperty("url")]
    public string Url { get; set; }

    [JsonProperty("extra")]
    public string Extra { get; set; }
}
