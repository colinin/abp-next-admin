using LINGYUN.Abp.Localization.Persistence;
using System;

namespace Volo.Abp.Localization;
public static class AbpLocalizationOptionsExtensions
{
    public static void UsePersistence<TResource>(
        this AbpLocalizationOptions options)
    {
        options.Resources
            .Get<TResource>()
            .Contributors
            .Add(new LocalizationSaveToPersistenceContributor());
    }

    public static void UsePersistence(
        this AbpLocalizationOptions options,
        Type localizationResourceType)
    {
        options.Resources
            .Get(localizationResourceType)
            .Contributors
            .Add(new LocalizationSaveToPersistenceContributor());
    }

    public static void UsePersistences(
        this AbpLocalizationOptions options,
        params Type[] localizationResourceTypes)
    {
        foreach (var localizationResourceType in localizationResourceTypes)
        {
            options.UsePersistence(localizationResourceType);
        }
    }

    public static void UseAllPersistence(
        this AbpLocalizationOptions options)
    {
        foreach (var resource in options.Resources)
        {
            resource.Value.Contributors.Add(new LocalizationSaveToPersistenceContributor());
        }
    }
}
