using System;
using Volo.Abp.ObjectExtending.Modularity;

namespace LINGYUN.Platform.ObjectExtending
{
    public class PlatfromModuleExtensionConfiguration : ModuleExtensionConfiguration
    {
        public PlatfromModuleExtensionConfiguration ConfigureRoute(
            Action<EntityExtensionConfiguration> configureAction)
        {
            return this.ConfigureEntity(
                PlatformModuleExtensionConsts.EntityNames.Route,
                configureAction
            );
        }

        public PlatfromModuleExtensionConfiguration ConfigureAppVersion(
            Action<EntityExtensionConfiguration> configureAction)
        {
            return this.ConfigureEntity(
                PlatformModuleExtensionConsts.EntityNames.AppVersion,
                configureAction
            );
        }
    }
}
