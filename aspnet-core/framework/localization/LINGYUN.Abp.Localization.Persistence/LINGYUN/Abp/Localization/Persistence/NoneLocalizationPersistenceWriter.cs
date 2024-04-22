using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.Localization.Persistence;

[Dependency(TryRegister = true)]
public class NoneLocalizationPersistenceWriter : ILocalizationPersistenceWriter, ISingletonDependency
{
    public Task<IEnumerable<string>> GetExistsTextsAsync(
        string resourceName, 
        string cultureName, 
        IEnumerable<string> keys, 
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(keys);
    }

    public Task<bool> WriteLanguageAsync(LanguageInfo language, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(false);
    }

    public Task<bool> WriteResourceAsync(LocalizationResourceBase resource, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(false);
    }

    public Task<bool> WriteTextsAsync(IEnumerable<LocalizableStringText> texts, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(false);
    }
}
