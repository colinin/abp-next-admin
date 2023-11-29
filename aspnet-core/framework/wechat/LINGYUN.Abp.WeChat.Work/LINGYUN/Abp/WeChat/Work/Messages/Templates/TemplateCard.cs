using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.WeChat.Work.Messages.Templates;

public abstract class TemplateCard
{
    /// <summary>
    /// 模板卡片类型
    /// </summary>
    [NotNull]
    [JsonProperty("card_type")]
    [JsonPropertyName("card_type")]
    public string Type { get; }
    /// <summary>
    /// 任务id，同一个应用任务id不能重复，只能由数字、字母和“_-@”组成，最长128字节
    /// </summary>
    [CanBeNull]
    [JsonProperty("task_id")]
    [JsonPropertyName("task_id")]
    public string TaskId { get; set; }
    protected TemplateCard(string type, string taskId = "")
    {
        Type = type;
        TaskId = taskId;
    }
}
