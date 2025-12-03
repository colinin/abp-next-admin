using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.OA.Approvals.Models;
/// <summary>
/// 附件控件值
/// </summary>
public class FileControlValue : ControlValue
{
    /// <summary>
    /// 附件列表
    /// </summary>
    [NotNull]
    [JsonProperty("files")]
    [JsonPropertyName("files")]
    public List<FileValue> Files { get; set; }
    public FileControlValue()
    {

    }

    public FileControlValue(List<FileValue> files)
    {
        Files = files;
    }
}

public class FileValue
{
    /// <summary>
    /// 文件id，该id为临时素材上传接口返回的的media_id，注：提单后将作为单据内容转换为长期文件存储；目前一个审批申请单，全局仅支持上传6个附件，否则将失败。
    /// </summary>
    [NotNull]
    [JsonProperty("file_id")]
    [JsonPropertyName("file_id")]
    public string FileId { get; set; }
    /// <summary>
    /// 文件名称，类型为string，如果没有可以填空字符串。
    /// </summary>
    [CanBeNull]
    [JsonProperty("file_name")]
    [JsonPropertyName("file_name")]
    public string FileName { get; set; }
    /// <summary>
    /// 文件大小，类型为number，如果没有可以填空字符串。
    /// </summary>
    [CanBeNull]
    [JsonProperty("file_size")]
    [JsonPropertyName("file_size")]
    public long? FileSize { get; set; }
    /// <summary>
    /// 文件类型，类型为string，如果没有可以填空字符串。
    /// </summary>
    [CanBeNull]
    [JsonProperty("file_type")]
    [JsonPropertyName("file_type")]
    public string FileType { get; set; }
    /// <summary>
    /// 文件地址，类型为string，如果没有可以填空字符串。
    /// </summary>
    [CanBeNull]
    [JsonProperty("file_url")]
    [JsonPropertyName("file_url")]
    public string FileUrl { get; set; }
    public FileValue()
    {

    }

    public FileValue(string fileId)
    {
        FileId = fileId;
    }
}
