using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Localization.Persistence;

[Dependency(TryRegister = true)]
public class NoneLocalizationPersistenceReader : ILocalizationPersistenceReader, ISingletonDependency
{
    public void Fill(string resourceName, string cultureName, Dictionary<string, LocalizedString> dictionary)
    {

    }

    public Task FillAsync(string resourceName, string cultureName, Dictionary<string, LocalizedString> dictionary)
    {
        return Task.CompletedTask;
    }

    public LocalizedString GetOrNull(string resourceName, string cultureName, string name)
    {
        return null;
    }

    public Task<IEnumerable<string>> GetSupportedCulturesAsync()
    {
        IEnumerable<string> emptyCultures = new string[0];

        return Task.FromResult(emptyCultures);
    }
}
