using Volo.Abp.Emailing;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Account.Templates
{
    [DependsOn(
        typeof(AbpEmailingModule), 
        typeof(AbpAccountApplicationContractsModule))]
    public class AbpAccountTemplatesModule : AbpModule
    {

    }
}
