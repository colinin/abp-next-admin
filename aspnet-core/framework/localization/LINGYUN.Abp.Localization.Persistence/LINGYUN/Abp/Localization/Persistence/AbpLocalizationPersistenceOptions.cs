using System;
using System.Collections.Generic;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.Localization.Persistence;

public class AbpLocalizationPersistenceOptions
{
    public bool SaveStaticLocalizationsToPersistence { get; set; }

    public HashSet<string> SaveToPersistenceResources { get; }

    public AbpLocalizationPersistenceOptions()
    {
        SaveStaticLocalizationsToPersistence = true;

        SaveToPersistenceResources = new HashSet<string>();
    }

    public void AddPersistenceResource<TResource>()
    {
        AddPersistenceResource(typeof(TResource));
    }

    public void AddPersistenceResource(Type resourceType)
    {
        var resourceName = LocalizationResourceNameAttribute.GetName(resourceType);
        if (SaveToPersistenceResources.Contains(resourceName))
        {
            return;
        }

        SaveToPersistenceResources.Add(resourceName);
    }
}
