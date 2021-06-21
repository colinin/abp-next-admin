using LINGYUN.Abp.MultiTenancy;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace LINGYUN.Platform.EventBus.Handlers
{
    public class TenantSynchronizer :
        IDistributedEventHandler<CreateEventData>,
        ITransientDependency
    {
        protected IDataSeeder DataSeeder { get; }

        public TenantSynchronizer(IDataSeeder dataSeeder)
        {
            DataSeeder = dataSeeder;
        }

        /// <summary>
        /// 租户创建之后需要预置种子数据
        /// </summary>
        /// <param name="eventData"></param>
        /// <returns></returns>
        public virtual async Task HandleEventAsync(CreateEventData eventData)
        {
            await DataSeeder.SeedAsync(
                new DataSeedContext(eventData.Id));
        }
    }
}
