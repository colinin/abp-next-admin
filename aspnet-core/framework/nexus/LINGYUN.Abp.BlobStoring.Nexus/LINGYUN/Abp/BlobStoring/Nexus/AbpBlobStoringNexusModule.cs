using LINGYUN.Abp.Sonatype.Nexus;
using Volo.Abp.BlobStoring;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.BlobStoring.Nexus;

[DependsOn(
    typeof(AbpBlobStoringModule),
    typeof(AbpSonatypeNexusModule))]
public class AbpBlobStoringNexusModule : AbpModule
{
}
