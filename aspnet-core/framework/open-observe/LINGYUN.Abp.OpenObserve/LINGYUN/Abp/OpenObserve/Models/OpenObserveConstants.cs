namespace LINGYUN.Abp.OpenObserve.Models;

public static class OpenObserveConstants
{
    /// <summary>
    /// 搜索接口默认超时时间(秒)
    /// </summary>
    public const int ZO_QUERY_TIMEOUT = 30;
    /// <summary>
    /// 默认端点
    /// </summary>
    public const string DefaultEndpoint = "http://localhost:5080";
    /// <summary>
    /// 默认请求超时时间
    /// </summary>
    public const int DefaultTimeoutSeconds = 30;
    /// <summary>
    /// 默认组织名称
    /// </summary>
    public const string DefaultOrganization = "default";
    /// <summary>
    /// 元数据组织名称
    /// </summary>
    /// <remarks>
    /// 集群信息等接口仅对 _meta 组织开放
    /// </remarks>
    public const string MetaOrganization = "_meta";
    /// <summary>
    /// JSON 内容类型
    /// </summary>
    public const string JsonContentType = "application/json";
    /// <summary>
    /// NDJSON 内容类型
    /// </summary>
    /// <remarks>
    /// 用于 _bulk 与 _multi 接口(每行一个 JSON 对象)
    /// </remarks>
    public const string NdJsonContentType = "application/x-ndjson";

    /// <summary>
    /// 数据流类型
    /// </summary>
    public static class StreamTypes
    {
        public const string Logs = "logs";
        public const string Metrics = "metrics";
        public const string Traces = "traces";
    }

    /// <summary>
    /// _bulk 支持的动作
    /// </summary>
    /// <remarks>
    /// delete 不被支持
    /// </remarks>
    public static class BulkActions
    {
        public const string Index = "index";
        public const string Create = "create";
        public const string Update = "update";
    }

    /// <summary>
    /// 默认文件夹
    /// </summary>
    public const string DefaultFolder = "default";
}
