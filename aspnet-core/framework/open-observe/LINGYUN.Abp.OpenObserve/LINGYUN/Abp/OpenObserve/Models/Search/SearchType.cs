namespace LINGYUN.Abp.OpenObserve.Models.Search;

/// <summary>
/// 搜索来源
/// </summary>
/// <remarks>
/// 文档给出 ui / dashboards / reports / alerts, 实现还接受 values / other / rum /
/// derived_stream / search_job / download / insights, 非法值返回 400
/// </remarks>
public enum SearchType
{
    UI = 0,
    Dashboards = 1,
    Reports = 2,
    Alerts = 3,
    Values = 4,
    Other = 5,
    Rum = 6,
    DerivedStream = 7,
    SearchJob = 8,
    Download = 9,
    Insights = 10,
}

public static class SearchTypeExtensions
{
    /// <summary>
    /// 转换为接口所需的字符串值
    /// </summary>
    public static string AsString(this SearchType searchType)
    {
        return searchType switch
        {
            SearchType.Dashboards => "dashboards",
            SearchType.Reports => "reports",
            SearchType.Alerts => "alerts",
            SearchType.Values => "values",
            SearchType.Other => "other",
            SearchType.Rum => "rum",
            SearchType.DerivedStream => "derived_stream",
            SearchType.SearchJob => "search_job",
            SearchType.Download => "download",
            SearchType.Insights => "insights",
            _ => "ui",
        };
    }
}
