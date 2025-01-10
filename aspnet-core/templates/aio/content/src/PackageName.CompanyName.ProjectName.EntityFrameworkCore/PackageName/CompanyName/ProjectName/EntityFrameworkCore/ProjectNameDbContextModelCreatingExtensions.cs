using Microsoft.EntityFrameworkCore;
using System;
using Volo.Abp;

namespace PackageName.CompanyName.ProjectName.EntityFrameworkCore;

public static class ProjectNameDbContextModelCreatingExtensions
{
    public static void ConfigureProjectName(
        this ModelBuilder builder,
        Action<ProjectNameModelBuilderConfigurationOptions> optionsAction = null)
    {
        Check.NotNull(builder, nameof(builder));

        var options = new ProjectNameModelBuilderConfigurationOptions(
            ProjectNameDbProperties.DbTablePrefix,
            ProjectNameDbProperties.DbSchema
        );
        optionsAction?.Invoke(options);
    }
}
