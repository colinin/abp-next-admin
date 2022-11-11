using Volo.Abp.Modularity;

namespace LINGYUN.Abp.DynamicQueryable.EntityFrameworkCore;

[DependsOn(typeof(AbpEntityFrameworkCoreTestModule))]
public class AbpDynamicQueryableEntityFrameworkCoreTestModule : AbpModule
{

}