using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.Localization.Persistence;
public interface ILocalizationPersistenceWriter
{
    Task<bool> WriteLanguageAsync(
        LanguageInfo language,
        CancellationToken cancellationToken = default);

    Task<bool> WriteResourceAsync(
       LocalizationResourceBase resource,
       CancellationToken cancellationToken = default);

    Task<IEnumerable<string>> GetExistsTextsAsync(
        string resourceName,
        string cultureName,
        IEnumerable<string> keys,
        CancellationToken cancellationToken = default);

    Task<bool> WriteTextsAsync(
        IEnumerable<LocalizableStringText> texts,
        CancellationToken cancellationToken = default);
}
