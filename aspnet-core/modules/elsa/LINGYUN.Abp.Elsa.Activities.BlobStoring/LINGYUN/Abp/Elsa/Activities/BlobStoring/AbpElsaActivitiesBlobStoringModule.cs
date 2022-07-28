using Volo.Abp.BlobStoring;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Elsa.Activities.BlobStoring;

[DependsOn(
    typeof(AbpElsaModule),
    typeof(AbpBlobStoringModule))]
public class AbpElsaActivitiesBlobStoringModule : AbpModule
{
}
