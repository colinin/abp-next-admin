using System;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.Logging.Serilog.OpenObserve.Models.Search;

public class SearchQuery
{
    /// <summary>
    /// use SQL query data, and filter data by start_time and end_time, and default order by _timestamp, you can use order by override order, and fetch offset limit by form and size
    /// </summary>
    [JsonPropertyName("sql")]
    public string Sql { get; set; }
    /// <summary>
    /// unit: microseconds, filter data by time range, you need always provide this value
    /// </summary>
    [JsonPropertyName("start_time")]
    public long StartTime { get; set; }
    /// <summary>
    /// unit: microseconds, filter data by time range, you need always provide this value
    /// </summary>
    [JsonPropertyName("end_time")]
    public long EndTime { get; set; }
    /// <summary>
    /// offset in SQL
    /// </summary>
    [JsonPropertyName("from")]
    public long From { get; set; }
    /// <summary>
    /// limit in SQL
    /// </summary>
    [JsonPropertyName("size")]
    public long Size { get; set; }
    public SearchQuery(string sql, DateTime startTime, DateTime endTime, long from = 0, long size = 10)
    {
        Sql = sql;
        StartTime = startTime.ToMicroseconds();
        EndTime = endTime.ToMicroseconds();
        From = from;
        Size = size;
    }
}
