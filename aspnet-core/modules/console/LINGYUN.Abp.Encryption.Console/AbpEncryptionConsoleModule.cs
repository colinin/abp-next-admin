using Volo.Abp.Modularity;
using Volo.Abp.Security;

namespace LINGYUN.Abp.Encryption.Console
{
    [DependsOn(
        typeof(AbpSecurityModule))]
    public class AbpEncryptionConsoleModule : AbpModule
    {

    }
}
