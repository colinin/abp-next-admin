using JetBrains.Annotations;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace PackageName.CompanyName.ProjectName.EntityFrameworkCore;

public class ProjectNameModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
{
    public ProjectNameModelBuilderConfigurationOptions(
        [NotNull] string tablePrefix = "",
        [CanBeNull] string schema = null)
        : base(
            tablePrefix,
            schema)
    {

    }
}
