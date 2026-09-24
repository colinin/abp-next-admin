namespace LINGYUN.Abp.AuditLogging.Elasticsearch;
/// <summary>
/// 索引滚动间隔
/// </summary>
public enum RollingInterval
{
    /// <summary>
    /// 索引名称不会随时间滚动, 索引中不包含时间范围信息
    /// </summary>
    Infinite,
    /// <summary>
    /// 每分钟滚动,索引后缀增加yyyyMMddHHmm
    /// </summary>
    Minute,
    /// <summary>
    /// 每小时滚动,索引后缀增加yyyyMMddHH
    /// </summary>
    Hour,
    /// <summary>
    /// 每天滚动,索引后缀增加yyyyMMdd
    /// </summary>
    Day,
    /// <summary>
    /// 每月滚动,索引后缀增加yyyyMM
    /// </summary>
    Month,
    /// <summary>
    /// 每年滚动,索引后缀增加yyyy
    /// </summary>
    Year,
}
