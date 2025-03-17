using System;
using Volo.Abp.ObjectExtending.Modularity;

namespace PackageName.CompanyName.ProjectName.ObjectExtending;

public class ProjectNameModuleExtensionConfiguration : ModuleExtensionConfiguration
{
    public ProjectNameModuleExtensionConfiguration ConfigureProjectName(
        Action<EntityExtensionConfiguration> configureAction)
    {
        return this.ConfigureEntity(
            ProjectNameModuleExtensionConsts.EntityNames.Entity,
            configureAction
        );
    }
}
