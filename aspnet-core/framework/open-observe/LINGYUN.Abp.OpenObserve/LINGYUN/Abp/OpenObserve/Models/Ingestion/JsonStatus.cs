using System.Text.Json.Serialization;

namespace LINGYUN.Abp.OpenObserve.Models.Ingestion;

public class JsonStatus
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = default!;

    [JsonPropertyName("successful")]
    public int Successful { get; set; }

    [JsonPropertyName("failed")]
    public int Failed { get; set; }

    /// <summary>
    /// 失败原因
    /// </summary>
    /// <remarks>
    /// 如: flatten value must be an object
    /// </remarks>
    [JsonPropertyName("error")]
    public string? Error { get; set; }

    /// <summary>
    /// 是否全部成功
    /// </summary>
    [JsonIgnore]
    public bool IsSuccess => Failed == 0;
}
