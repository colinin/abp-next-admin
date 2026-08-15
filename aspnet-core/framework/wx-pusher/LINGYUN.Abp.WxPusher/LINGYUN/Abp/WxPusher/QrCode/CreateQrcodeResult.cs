using Newtonsoft.Json;
using System;

namespace LINGYUN.Abp.WxPusher.QrCode;

[Serializable]
public class CreateQrcodeResult
{
    [JsonProperty("expires")]
    public long Expires { get; set; }

    [JsonProperty("code")]
    public string Code { get; set; } = default!;

    [JsonProperty("shortUrl")]
    public string ShortUrl { get; set; } = default!;

    [JsonProperty("url")]
    public string Url { get; set; } = default!;

    [JsonProperty("extra")]
    public string? Extra { get; set; }
}
