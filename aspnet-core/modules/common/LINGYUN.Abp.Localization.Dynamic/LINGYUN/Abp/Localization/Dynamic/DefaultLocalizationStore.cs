using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.Localization.Dynamic
{
    public class DefaultLocalizationStore : ILocalizationStore, ITransientDependency
    {
        public DefaultLocalizationStore()
        {
        }

        public Task<List<LanguageInfo>> GetLanguageListAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new List<LanguageInfo>());
        }

        public Task<Dictionary<string, ILocalizationDictionary>> GetLocalizationDictionaryAsync(string resourceName, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new Dictionary<string, ILocalizationDictionary>());
        }

        public Task<bool> ResourceExistsAsync(string resourceName, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(false);
        }
    }
}
