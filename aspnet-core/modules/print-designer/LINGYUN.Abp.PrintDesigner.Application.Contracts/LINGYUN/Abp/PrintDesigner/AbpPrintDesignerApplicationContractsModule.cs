using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.PrintDesigner;

[DependsOn(
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpPrintDesignerDomainSharedModule))]
public class AbpPrintDesignerApplicationContractsModule : AbpModule
{
}
