using JetBrains.Annotations;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.BackgroundTasks.Activities;

public class JobActionParamter
{
    public string Name { get; set; }
    public bool Required { get; set; }
    public ILocalizableString DisplayName { get; set; }
    public ILocalizableString Description { get; set; }

    public JobActionParamter(
        [NotNull] string name,
        [NotNull] ILocalizableString displayName,
        [CanBeNull] ILocalizableString description = null,
        bool required = false)
    {
        Name = name;
        DisplayName = displayName;
        Description = description;
        Required = required;
    }
}
