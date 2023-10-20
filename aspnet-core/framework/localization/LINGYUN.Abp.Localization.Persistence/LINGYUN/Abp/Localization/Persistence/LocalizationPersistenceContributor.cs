using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.Localization.Persistence;

public class LocalizationPersistenceContributor : ILocalizationResourceContributor
{
    public bool IsDynamic => true;

    private LocalizationResourceBase _resource;
    private ILocalizationPersistenceReader _persistenceSupport;
    public void Initialize(LocalizationResourceInitializationContext context)
    {
        _resource = context.Resource;
        _persistenceSupport = context.ServiceProvider.GetRequiredService<ILocalizationPersistenceReader>();
    }

    public virtual void Fill(string cultureName, Dictionary<string, LocalizedString> dictionary)
    {
        _persistenceSupport.Fill(_resource.ResourceName, cultureName, dictionary);
    }

    public async virtual Task FillAsync(string cultureName, Dictionary<string, LocalizedString> dictionary)
    {
        await _persistenceSupport.FillAsync(_resource.ResourceName, cultureName, dictionary);
    }

    public virtual LocalizedString GetOrNull(string cultureName, string name)
    {
        return _persistenceSupport.GetOrNull(_resource.ResourceName, cultureName, name);
    }

    public async virtual Task<IEnumerable<string>> GetSupportedCulturesAsync()
    {
        return await _persistenceSupport.GetSupportedCulturesAsync();
    }
}
