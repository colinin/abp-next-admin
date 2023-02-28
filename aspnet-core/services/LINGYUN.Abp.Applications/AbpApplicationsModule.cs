using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Applications;

[DependsOn(
    typeof(AbpAutofacModule))]
public class AbpApplicationsModule : AbpModule
{
}
