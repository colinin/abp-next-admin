using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.LocalizationManagement;

[ExposeServices(
    typeof(ILanguageProvider),
    typeof(LanguageProvider))]
public class LanguageProvider : ILanguageProvider, ITransientDependency
{
    protected ILanguageRepository Repository { get; }

    public LanguageProvider(ILanguageRepository repository)
    {
        Repository = repository;
    }

    public async virtual Task<IReadOnlyList<LanguageInfo>> GetLanguagesAsync()
    {
        var languages = await Repository.GetActivedListAsync();

        return languages.Select(MapToLanguageInfo).ToImmutableList();
    }

    protected virtual LanguageInfo MapToLanguageInfo(Language language)
    {
        return new LanguageInfo(
            language.CultureName,
            language.UiCultureName,
            language.DisplayName,
            language.FlagIcon);
    }
}
