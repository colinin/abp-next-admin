using System;
using Volo.Abp.ObjectExtending.Modularity;

namespace LINGYUN.Platform.ObjectExtending
{
    public static class PlatformModuleExtensionConfigurationDictionaryExtensions
    {
        public static ModuleExtensionConfigurationDictionary ConfigurePlatform(
            this ModuleExtensionConfigurationDictionary modules,
            Action<PlatfromModuleExtensionConfiguration> configureAction)
        {
            return modules.ConfigureModule(
                PlatformModuleExtensionConsts.ModuleName,
                configureAction
            );
        }
    }
}
