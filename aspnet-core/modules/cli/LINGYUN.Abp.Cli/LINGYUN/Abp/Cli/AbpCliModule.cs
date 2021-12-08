using LINGYUN.Abp.Cli.Commands;
using Volo.Abp.Autofac;
using Volo.Abp.Cli;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Cli
{
    [DependsOn(
        typeof(AbpCliCoreModule),
        typeof(AbpAutofacModule)
    )]
    public class AbpCliModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpCliOptions>(options =>
            {
                options.Commands.Clear();
                options.Commands["help"] = typeof(HelpCommand);
                options.Commands["create"] = typeof(CreateCommand);
            });
        }
    }
}
