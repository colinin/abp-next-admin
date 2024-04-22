using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.Localization.Persistence;

/// <summary>
/// 空接口, 使用此提供者可持久化本地化资源到持久设施
/// </summary>
public class LocalizationSaveToPersistenceContributor : ILocalizationResourceContributor
{
    public bool IsDynamic => true;

    public void Fill(string cultureName, Dictionary<string, LocalizedString> dictionary)
    {
    }

    public Task FillAsync(string cultureName, Dictionary<string, LocalizedString> dictionary)
    {
        return Task.CompletedTask;
    }

    public LocalizedString GetOrNull(string cultureName, string name)
    {
        return null;
    }

    public Task<IEnumerable<string>> GetSupportedCulturesAsync()
    {
        IEnumerable<string> emptyCultures = new string[0];

        return Task.FromResult(emptyCultures);
    }

    public void Initialize(LocalizationResourceInitializationContext context)
    {
    }
}
