using Newtonsoft.Json;

namespace LINGYUN.Abp.PushPlus.Topic;

public class PushPlusTopicQrCode
{
    /// <summary>
    /// 群组二维码图片路径
    /// </summary>
    [JsonProperty("qrCodeImgUrl")]
    public string QrCodeImgUrl { get; set; }
    /// <summary>
    /// 二维码类型；
    /// 0-临时二维码，
    /// 1-永久二维码
    /// </summary>
    [JsonProperty("forever")]
    public PushPlusTopicQrCodeType Forever { get; set; }
}
