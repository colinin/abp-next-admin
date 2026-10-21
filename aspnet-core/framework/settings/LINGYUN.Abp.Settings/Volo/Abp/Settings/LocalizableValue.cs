using System;

namespace Volo.Abp.Settings;

[Serializable]
public class LocalizableValue<TValue>
{
    public string ResourceName { get; set; } = default!;

    public string Name { get; set; } = default!;

    public TValue Value { get; set; } = default!;

    public LocalizableValue()
    {

    }

    public LocalizableValue(string resourceName, string name, TValue value)
    {
        ResourceName = resourceName;
        Name = name;
        Value = value;
    }
}
