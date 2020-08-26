using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Domain.Entities.Events
{
    [DependsOn(typeof(AbpDddDomainModule))]
    public class AbpDddDomainEntitesEventsModule : AbpModule
    {
    }
}
