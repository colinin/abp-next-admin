using LINGYUN.Abp.Localization.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.LocalizationManagement
{
    public interface ILocalizationCacheInitialize : IAsyncInitialize
    {

    }
    public class LocalizationCacheInitialize : ILocalizationCacheInitialize, ITransientDependency
    {
        protected IDistributedCache<LocalizationCacheItem> Cache { get; }
        protected ITextRepository TextRepository { get; }

        public LocalizationCacheInitialize(
            IDistributedCache<LocalizationCacheItem> cache,
            ITextRepository textRepository)
        {
            Cache = cache;
            TextRepository = textRepository;
        }

        public virtual async Task InitializeAsync()
        {
            var texts = await TextRepository.GetListAsync();

            foreach (var textGroup in texts.GroupBy(x => x.ResourceName))
            {
                foreach (var textCulture in textGroup.GroupBy(x => x.CultureName))
                {
                    var cacheKey = LocalizationCacheItem.NormalizeKey(textGroup.Key, textCulture.Key);
                    var cacheItem = new LocalizationCacheItem(
                        textGroup.Key,
                        textCulture.Key,
                        textCulture
                        .Select(x => new LocalizationText(x.Key, x.Value))
                        .ToList());
                    await Cache.SetAsync(cacheKey, cacheItem);
                }
            }
        }
    }
}
