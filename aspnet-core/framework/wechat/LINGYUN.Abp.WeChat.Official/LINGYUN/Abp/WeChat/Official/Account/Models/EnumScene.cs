using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Official.Account.Models;
public class EnumScene : Scene
{
    /// <summary>
    /// 场景值ID，临时二维码时为32位非0整型，永久二维码时最大值为100000（目前参数只支持1--100000）
    /// </summary>
    [JsonProperty("scene_id")]
    [JsonPropertyName("scene_id")]
    public int SceneId { get; }
    public EnumScene(int sceneId)
    {
        SceneId = sceneId;
    }

    public override string GetKey()
    {
        return SceneId.ToString();
    }
}
