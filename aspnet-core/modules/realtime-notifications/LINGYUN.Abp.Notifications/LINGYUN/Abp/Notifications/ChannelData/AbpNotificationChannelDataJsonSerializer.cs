using System;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;

namespace LINGYUN.Abp.Notifications;

[Dependency(TryRegister = true)]
public class AbpNotificationChannelDataJsonSerializer : INotificationChannelDataJsonSerializer, ISingletonDependency
{
    protected IJsonSerializer JsonSerializer { get; }

    public AbpNotificationChannelDataJsonSerializer(IJsonSerializer jsonSerializer)
    {
        JsonSerializer = jsonSerializer;
    }

    public virtual string Serialize(object value, Type valueType)
    {
        return JsonSerializer.Serialize(value);
    }

    public virtual object? Deserialize(string json, Type targetType)
    {
        if (json.IsNullOrWhiteSpace())
        {
            return null;
        }

        return JsonSerializer.Deserialize(targetType, json);
    }
}
