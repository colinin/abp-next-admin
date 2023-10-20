using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Localization.Persistence;

public interface ILocalizationPersistenceReader
{
    LocalizedString GetOrNull(string resourceName, string cultureName, string name);

    void Fill(string resourceName, string cultureName, Dictionary<string, LocalizedString> dictionary);

    Task FillAsync(string resourceName, string cultureName, Dictionary<string, LocalizedString> dictionary);

    Task<IEnumerable<string>> GetSupportedCulturesAsync();
}
