using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.LocalizationManagement;

public interface ILocalizationTextStoreCache
{
    LocalizedString GetOrNull(LocalizationResourceBase resource, string cultureName, string name);

    void Fill(LocalizationResourceBase resource, string cultureName, Dictionary<string, LocalizedString> dictionary);

    Task FillAsync(LocalizationResourceBase resource, string cultureName, Dictionary<string, LocalizedString> dictionary);
}
