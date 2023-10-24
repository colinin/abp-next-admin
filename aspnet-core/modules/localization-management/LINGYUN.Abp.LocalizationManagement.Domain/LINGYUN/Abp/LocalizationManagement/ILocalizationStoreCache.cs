using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.LocalizationManagement;

public interface ILocalizationStoreCache
{
    string CacheStamp { get; set; }

    SemaphoreSlim SyncSemaphore { get; }

    DateTime? LastCheckTime { get; set; }

    Task InitializeAsync(LocalizationStoreCacheInitializeContext context);

    LocalizationResourceBase GetResourceOrNull(string resourceName);

    LocalizedString GetLocalizedStringOrNull(string resourceName, string cultureName, string name);

    IReadOnlyList<LocalizationResourceBase> GetResources();

    IReadOnlyList<LanguageInfo> GetLanguages();

    IDictionary<string, LocalizationDictionary> GetAllLocalizedStrings(string cultureName);
}
