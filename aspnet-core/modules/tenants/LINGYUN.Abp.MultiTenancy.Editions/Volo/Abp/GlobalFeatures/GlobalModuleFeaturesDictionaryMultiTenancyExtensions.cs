using JetBrains.Annotations;
using LINGYUN.Abp.MultiTenancy.Editions.GlobalFeatures;
using System;
using System.Collections.Generic;

namespace Volo.Abp.GlobalFeatures;

public static class GlobalModuleFeaturesDictionaryMultiTenancyExtensions
{
    public static GlobalMultiTenancyFeatures MultiTenancy(
        [NotNull] this GlobalModuleFeaturesDictionary modules)
    {
        Check.NotNull(modules, nameof(modules));

        return modules
                .GetOrAdd(
                    GlobalMultiTenancyFeatures.ModuleName,
                    _ => new GlobalMultiTenancyFeatures(modules.FeatureManager)
                )
            as GlobalMultiTenancyFeatures;
    }

    public static GlobalModuleFeaturesDictionary MultiTenancy(
        [NotNull] this GlobalModuleFeaturesDictionary modules,
        [NotNull] Action<GlobalMultiTenancyFeatures> configureAction)
    {
        Check.NotNull(configureAction, nameof(configureAction));

        configureAction(modules.MultiTenancy());

        return modules;
    }
}
