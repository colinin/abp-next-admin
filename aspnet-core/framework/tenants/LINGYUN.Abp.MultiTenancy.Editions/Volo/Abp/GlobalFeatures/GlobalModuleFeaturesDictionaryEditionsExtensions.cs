using JetBrains.Annotations;
using LINGYUN.Abp.MultiTenancy.Editions.GlobalFeatures;
using System;
using System.Collections.Generic;

namespace Volo.Abp.GlobalFeatures;

public static class GlobalModuleFeaturesDictionaryEditionsExtensions
{
    public static GlobalEditionsFeatures Editions(
        [NotNull] this GlobalModuleFeaturesDictionary modules)
    {
        Check.NotNull(modules, nameof(modules));

        return modules
                .GetOrAdd(
                    GlobalEditionsFeatures.ModuleName,
                    _ => new GlobalEditionsFeatures(modules.FeatureManager)
                )
            as GlobalEditionsFeatures;
    }

    public static GlobalModuleFeaturesDictionary Editions(
        [NotNull] this GlobalModuleFeaturesDictionary modules,
        [NotNull] Action<GlobalEditionsFeatures> configureAction)
    {
        Check.NotNull(configureAction, nameof(configureAction));

        configureAction(modules.Editions());

        return modules;
    }
}
