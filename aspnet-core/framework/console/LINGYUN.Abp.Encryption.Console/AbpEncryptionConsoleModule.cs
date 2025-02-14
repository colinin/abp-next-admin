using LINGYUN.Abp.Encryption.SM4;
using System.Text;
using Volo.Abp.Modularity;
using Volo.Abp.Security.Encryption;

namespace LINGYUN.Abp.Encryption.Console;

[DependsOn(typeof(AbpEncryptionSM4Module))]
public class AbpEncryptionConsoleModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpStringEncryptionOptions>(options =>
        {
            options.DefaultPassPhrase = "s46c5q55nxpeS8Ra";
            options.InitVectorBytes = Encoding.ASCII.GetBytes("s83ng0abvd02js84");
            options.DefaultSalt = Encoding.ASCII.GetBytes("sf&5)s3#");
        });
    }
}
