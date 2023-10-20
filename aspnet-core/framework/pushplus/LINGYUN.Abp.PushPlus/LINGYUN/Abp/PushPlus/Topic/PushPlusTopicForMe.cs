using Newtonsoft.Json;

namespace LINGYUN.Abp.PushPlus.Topic;
/// <summary>
/// 我加入的群详情
/// </summary>
public class PushPlusTopicForMe : PushPlusTopic
{
    /// <summary>
    /// 联系方式
    /// </summary>
    [JsonProperty("contact")]
    public string Contact { get; set; }
    /// <summary>
    /// 群组简介
    /// </summary>
    [JsonProperty("introduction")]
    public string Introduction { get; set; }
}
