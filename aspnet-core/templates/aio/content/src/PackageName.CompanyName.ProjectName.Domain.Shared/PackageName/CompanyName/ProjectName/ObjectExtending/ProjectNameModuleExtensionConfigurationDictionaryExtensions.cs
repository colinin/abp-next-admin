using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.ObjectExtending.Modularity;

namespace PackageName.CompanyName.ProjectName.ObjectExtending;

public static class ProjectNameModuleExtensionConfigurationDictionaryExtensions
{
    public static ModuleExtensionConfigurationDictionary ConfigureProjectName(
        this ModuleExtensionConfigurationDictionary modules,
        Action<ProjectNameModuleExtensionConfiguration> configureAction)
    {
        return modules.ConfigureModule(
            ProjectNameModuleExtensionConsts.ModuleName,
            configureAction
        );
    }
}
