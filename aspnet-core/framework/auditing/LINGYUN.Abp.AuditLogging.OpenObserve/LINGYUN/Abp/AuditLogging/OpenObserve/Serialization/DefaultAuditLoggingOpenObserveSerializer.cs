using System;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;

namespace LINGYUN.Abp.AuditLogging.OpenObserve.Serialization;

/// <summary>
/// 默认序列化器
/// </summary>
/// <remarks>
/// 直接使用 ABP 的 <see cref="IJsonSerializer"/>, 以复用全局的
/// <c>AbpSystemTextJsonSerializerModifiersOptions</c> 修饰器配置
/// </remarks>
public class DefaultAuditLoggingOpenObserveSerializer : IAuditLoggingOpenObserveSerializer, ITransientDependency
{
    protected IJsonSerializer JsonSerializer { get; }

    public DefaultAuditLoggingOpenObserveSerializer(IJsonSerializer jsonSerializer)
    {
        JsonSerializer = jsonSerializer;
    }

    public virtual string Serialize<T>(T value)
    {
        return JsonSerializer.Serialize(value!);
    }

    public virtual T? Deserialize<T>(string jsonString)
    {
        if (jsonString.IsNullOrWhiteSpace())
        {
            return default;
        }

        try
        {
            return JsonSerializer.Deserialize<T>(jsonString);
        }
        catch (Exception)
        {
            // 历史数据或非预期结构时返回默认值
            return default;
        }
    }
}
