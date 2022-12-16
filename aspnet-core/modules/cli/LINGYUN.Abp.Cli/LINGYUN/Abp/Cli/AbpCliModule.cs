using LINGYUN.Abp.Cli.Commands;
using LINGYUN.Abp.Cli.ServiceProxying.CSharp;
using LINGYUN.Abp.Cli.ServiceProxying.TypeScript;
using Volo.Abp.Autofac;
using Volo.Abp.Cli;
using Volo.Abp.Cli.ServiceProxying;
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
                options.Commands[HelpCommand.Name] = typeof(HelpCommand);
                options.Commands[CreateCommand.Name] = typeof(CreateCommand);
                options.Commands[GenerateProxyCommand.Name] = typeof(GenerateProxyCommand);
            });

            Configure<AbpCliServiceProxyOptions>(options =>
            {
                options.Generators[TypeScriptServiceProxyGenerator.Name] = typeof(TypeScriptServiceProxyGenerator);
                options.Generators[CSharpServiceProxyGenerator.Name] = typeof(CSharpServiceProxyGenerator);
            });

            Configure<TypeScriptServiceProxyOptions>(options =>
            {
                options.ScriptGenerators[AxiosHttpApiScriptGenerator.Name] = new AxiosHttpApiScriptGenerator();
                options.ScriptGenerators[VbenAxiosHttpApiScriptGenerator.Name] = new VbenAxiosHttpApiScriptGenerator();
                options.ScriptGenerators[VbenDynamicHttpApiScriptGenerator.Name] = new VbenDynamicHttpApiScriptGenerator();
                options.ScriptGenerators[UniAppAxiosHttpApiScriptGenerator.Name] = new UniAppAxiosHttpApiScriptGenerator(); 
            });
        }
    }
}
