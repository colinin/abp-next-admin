using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.OpenObserve.Models.Stream;

public static class StreamTypeExtensions
{
    /// <summary>
    /// 转换为接口所需的字符串值
    /// </summary>
    public static string AsString(this StreamType streamType)
    {
        return streamType switch
        {
            StreamType.Metrics => OpenObserveConstants.StreamTypes.Metrics,
            StreamType.Traces => OpenObserveConstants.StreamTypes.Traces,
            _ => OpenObserveConstants.StreamTypes.Logs,
        };
    }
}
