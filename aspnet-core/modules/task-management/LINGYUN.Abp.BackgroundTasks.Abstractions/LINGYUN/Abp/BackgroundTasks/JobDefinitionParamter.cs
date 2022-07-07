using JetBrains.Annotations;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.BackgroundTasks;

public class JobDefinitionParamter
{
    public string Name { get; }

    public bool Required { get; }

    public ILocalizableString DisplayName { get; }

    public ILocalizableString Description { get; }

    public JobDefinitionParamter(
        [NotNull] string name,
        [NotNull] ILocalizableString displayName,
        [CanBeNull] ILocalizableString description = null,
        bool required = false)
    {
        Name = name;
        Required = required;
        DisplayName = displayName;
        Description = description;
    }
}
