using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.LocalizationManagement.External;

public interface IExternalLocalizationTextStoreCache
{
    Dictionary<string, string> GetTexts(LocalizationResourceBase resource, string cultureName);

    Task<Dictionary<string, string>> GetTextsAsync(LocalizationResourceBase resource, string cultureName);

    Task RemoveAsync(string resourceName, string cultureName);
}
