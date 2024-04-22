using JetBrains.Annotations;
using System;
using Volo.Abp;

namespace LINGYUN.Abp.MultiTenancy.Editions;

[Serializable]
public class EditionInfo
{
    public Guid Id { get; set; }

    public string DisplayName { get; set; }

    public EditionInfo()
    {
    }

    public EditionInfo(
        Guid id,
        [NotNull] string displayName)
    {
        Check.NotNull(displayName, nameof(displayName));

        Id = id;
        DisplayName = displayName;
    }
}
