namespace LINGYUN.Abp.AuditLogging.OpenObserve.Serialization;

/// <summary>
/// 审计日志 OpenObserve 序列化器
/// </summary>
/// <remarks>
/// 用于需要以 JSON 字符串列保存的成员(如实体变更的属性明细 <c>propertychanges</c>),
/// 默认实现委托给 ABP 的 <c>IJsonSerializer</c>, 从而复用
/// <c>AbpSystemTextJsonSerializerModifiersOptions</c> 注册的修饰器
/// </remarks>
public interface IAuditLoggingOpenObserveSerializer
{
    string Serialize<T>(T value);

    T? Deserialize<T>(string jsonString);
}
