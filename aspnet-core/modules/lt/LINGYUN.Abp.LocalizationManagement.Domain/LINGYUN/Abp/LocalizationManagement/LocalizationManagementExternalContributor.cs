using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.LocalizationManagement;

public class LocalizationManagementExternalContributor : ILocalizationResourceContributor
{
    public bool IsDynamic => true;

    private LocalizationResourceBase _resource;
    private ITextRepository _textRepository;
    private IResourceRepository _resourceRepository;
    private ILanguageRepository _languageRepository;

    public void Initialize(LocalizationResourceInitializationContext context)
    {
        _resource = context.Resource;
        _textRepository = context.ServiceProvider.GetRequiredService<ITextRepository>();
        _resourceRepository = context.ServiceProvider.GetRequiredService<IResourceRepository>();
        _languageRepository = context.ServiceProvider.GetRequiredService<ILanguageRepository>();
    }

    public virtual void Fill(string cultureName, Dictionary<string, LocalizedString> dictionary)
    {
        FillInternalAsync(_resource.ResourceName, cultureName, dictionary).GetAwaiter().GetResult();
    }

    public async virtual Task FillAsync(string cultureName, Dictionary<string, LocalizedString> dictionary)
    {
        await FillInternalAsync(_resource.ResourceName, cultureName, dictionary);
    }

    public virtual LocalizedString GetOrNull(string cultureName, string name)
    {
        return GetOrNullInternal(_resource.ResourceName, cultureName, name);
    }

    protected virtual LocalizedString GetOrNullInternal(string resourceName, string cultureName, string name)
    {
        var resource = GetResourceOrNullAsync(name).GetAwaiter().GetResult();
        if (resource == null)
        {
            return null;
        }
        var text = _textRepository.GetByCultureKeyAsync(resourceName, cultureName, name).GetAwaiter().GetResult();
        if (text != null)
        {
            return new LocalizedString(name, text.Value);
        }

        return null;
    }

    public async virtual Task<IEnumerable<string>> GetSupportedCulturesAsync()
    {
        var languages = await _languageRepository.GetActivedListAsync();

        return languages
            .Select(x => x.CultureName)
            .ToList();
    }

    protected async virtual Task FillInternalAsync(string resourceName, string cultureName, Dictionary<string, LocalizedString> dictionary)
    {
        var resource = await GetResourceOrNullAsync(resourceName);
        if (resource == null)
        {
            return;
        }

        var texts = await GetTextListByResourceAsync(resourceName, cultureName);

        foreach (var text in texts)
        {
            dictionary[text.Key] = new LocalizedString(text.Key, text.Value);
        }
    }

    protected async virtual Task<Resource> GetResourceOrNullAsync(string resourceName)
    {
        return await _resourceRepository.FindByNameAsync(resourceName);
    }

    protected async virtual Task<List<Text>> GetTextListByResourceAsync(string resourceName, string cultureName = null)
    {
        return await _textRepository.GetListAsync(resourceName, cultureName);
    }
}
