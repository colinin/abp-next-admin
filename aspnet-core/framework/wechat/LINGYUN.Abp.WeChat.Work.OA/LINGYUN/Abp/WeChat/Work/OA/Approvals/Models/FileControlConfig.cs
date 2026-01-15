using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.OA.Approvals.Models;
/// <summary>
/// 附件控件配置
/// </summary>
public class FileControlConfig : ControlConfig
{
    /// <summary>
    /// 附件控件
    /// </summary>
    [NotNull]
    [JsonProperty("file")]
    [JsonPropertyName("file")]
    public FileConfig File { get; set; }
    public FileControlConfig()
    {

    }

    public FileControlConfig(FileConfig file)
    {
        File = file;
    }
}

public class FileConfig
{
    /// <summary>
    /// 是否只允许拍照，1--是， 0--否
    /// </summary>
    [NotNull]
    [JsonProperty("is_only_photo")]
    [JsonPropertyName("is_only_photo")]
    public byte IsOnlyPhoto { get; set; }
    public FileConfig()
    {

    }

    public FileConfig(byte isOnlyPhoto)
    {
        IsOnlyPhoto = isOnlyPhoto;
    }
}
