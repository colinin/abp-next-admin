using JetBrains.Annotations;
using System;
using Volo.Abp;

namespace LINGYUN.Abp.MultiTenancy.Editions;

[Serializable]
public class EditionConfiguration
{
    public Guid Id { get; set; }

    public string DisplayName { get; set; }

    public EditionConfiguration()
    {
    }

    public EditionConfiguration(
        Guid id,
        [NotNull] string displayName)
    {
        Check.NotNull(displayName, nameof(displayName));

        Id = id;
        DisplayName = displayName;
    }
}
