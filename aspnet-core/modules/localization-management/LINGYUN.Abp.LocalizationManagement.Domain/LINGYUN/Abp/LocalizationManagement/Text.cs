using JetBrains.Annotations;
using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.Abp.LocalizationManagement;

public class Text : Entity<int>
{
    public virtual string CultureName { get; protected set; } = default!;
    public virtual string Key { get; protected set; } = default!;
    public virtual string? Value { get; protected set; }
    public virtual string ResourceName { get; protected set; } = default!;
    protected Text() { }
    public Text(
        [NotNull] string resourceName,
        [NotNull] string cultureName,
        [NotNull] string key,
        [CanBeNull] string? value = null)
    {
        ResourceName = Check.NotNull(resourceName, nameof(resourceName), ResourceConsts.MaxNameLength);
        CultureName = Check.NotNullOrWhiteSpace(cultureName, nameof(cultureName), LanguageConsts.MaxCultureNameLength);
        Key = Check.NotNullOrWhiteSpace(key, nameof(key), TextConsts.MaxKeyLength);

        Value = !value.IsNullOrWhiteSpace()
            ? Check.Length(value, nameof(value), TextConsts.MaxValueLength)
            : "";
    }

    public void SetValue(string? value)
    {
        Value = !value.IsNullOrWhiteSpace()
            ? Check.Length(value, nameof(value), TextConsts.MaxValueLength)
            : Value;
    }
}
