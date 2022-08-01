using Volo.Abp.Modularity;
using Volo.Abp.Emailing;

namespace LINGYUN.Abp.Elsa.Activities.Emailing;

[DependsOn(
    typeof(AbpElsaModule),
    typeof(AbpEmailingModule))]
public class AbpElsaActivitiesEmailingModule : AbpModule
{
}
