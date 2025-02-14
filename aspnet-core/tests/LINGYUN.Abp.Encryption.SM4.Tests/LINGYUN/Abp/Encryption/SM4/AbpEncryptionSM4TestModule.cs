using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Encryption.SM4;

[DependsOn(typeof(AbpEncryptionSM4Module))]
public class AbpEncryptionSM4TestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {

    }
}
