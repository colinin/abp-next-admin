using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Users;

namespace LINGYUN.Abp.AspNetCore.Mvc.Client
{
    public class MvcCurrentApplicationConfigurationCacheResetEventHandler :
        IDistributedEventHandler<CurrentApplicationConfigurationCacheResetEventData>,
        ITransientDependency
    {
        protected ICurrentUser CurrentUser { get; }
        protected IDistributedCache<ApplicationConfigurationDto> Cache { get; }

        public MvcCurrentApplicationConfigurationCacheResetEventHandler(ICurrentUser currentUser,
            IDistributedCache<ApplicationConfigurationDto> cache)
        {
            CurrentUser = currentUser;
            Cache = cache;
        }

        public async virtual Task HandleEventAsync(CurrentApplicationConfigurationCacheResetEventData eventData)
        {
            await Cache.RemoveAsync(CreateCacheKey());
        }

        protected virtual string CreateCacheKey()
        {
            return MvcCachedApplicationConfigurationClientHelper.CreateCacheKey(CurrentUser);
        }
    }
}
