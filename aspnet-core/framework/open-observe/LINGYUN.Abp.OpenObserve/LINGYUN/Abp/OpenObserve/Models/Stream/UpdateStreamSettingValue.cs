using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.OpenObserve.Models.Stream;

/// <summary>
/// 支持 add/set/remove 的设置项
/// </summary>
public class UpdateStreamSettingValue<T>
{
    /// <summary>
    /// 覆盖
    /// </summary>
    [JsonPropertyName("set")]
    public T[]? Set { get; set; }

    /// <summary>
    /// 追加
    /// </summary>
    [JsonPropertyName("add")]
    public T[]? Add { get; set; }

    /// <summary>
    /// 移除
    /// </summary>
    [JsonPropertyName("remove")]
    public T[]? Remove { get; set; }
}
