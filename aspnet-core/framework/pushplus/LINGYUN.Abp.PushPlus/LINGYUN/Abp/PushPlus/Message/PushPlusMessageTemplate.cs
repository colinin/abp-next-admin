namespace LINGYUN.Abp.PushPlus.Message;
/// <summary>
/// 模板（template）枚举。默认使用html模板
/// </summary>
public enum PushPlusMessageTemplate
{
    /// <summary>
    /// 默认模板，支持html文本
    /// </summary>
    Html = 0,
    /// <summary>
    /// 纯文本展示，不转义html
    /// </summary>
    Text = 1,
    /// <summary>
    /// 内容基于json格式展示
    /// </summary>
    Json = 2,
    /// <summary>
    /// 内容基于markdown格式展示
    /// </summary>
    Markdown = 3,
    /// <summary>
    /// 阿里云监控报警定制模板
    /// </summary>
    CloudMonitor = 4,
    /// <summary>
    /// jenkins插件定制模板
    /// </summary>
    Jenkins = 5,
    /// <summary>
    /// 路由器插件定制模板
    /// </summary>
    Route = 6
}
