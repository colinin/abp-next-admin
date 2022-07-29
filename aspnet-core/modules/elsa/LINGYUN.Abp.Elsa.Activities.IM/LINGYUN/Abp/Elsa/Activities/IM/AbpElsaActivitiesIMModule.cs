using LINGYUN.Abp.IM;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Elsa.Activities.IM;

[DependsOn(
    typeof(AbpElsaModule),
    typeof(AbpIMModule))]
public class AbpElsaActivitiesIMModule : AbpModule
{
}
