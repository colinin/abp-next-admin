using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Official.Account.Models;
public class StringScene : Scene
{
    /// <summary>
    /// 场景值ID（字符串形式的ID），字符串类型，长度限制为1到64
    /// </summary>
    [JsonProperty("scene_str")]
    [JsonPropertyName("scene_str")]
    public string SceneStr { get; }
    public StringScene(string sceneStr)
    {
        SceneStr = sceneStr;
    }

    public override string GetKey()
    {
        return SceneStr;
    }
}
