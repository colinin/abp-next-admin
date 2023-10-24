using LINGYUN.Abp.Cli.Commands;
using LINGYUN.Abp.Cli.ServiceProxying.CSharp;
using LINGYUN.Abp.Cli.ServiceProxying.Flutter;
using LINGYUN.Abp.Cli.ServiceProxying.TypeScript;
using LINGYUN.Abp.Cli.UI;
using LINGYUN.Abp.Cli.UI.Flutter.GetX;
using LINGYUN.Abp.Cli.UI.Vben;
using Volo.Abp.Autofac;
using Volo.Abp.Cli;
using Volo.Abp.Cli.ServiceProxying;
using Volo.Abp.Modularity;
using Volo.Abp.TextTemplating.Scriban;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.Cli
{
    [DependsOn(
        typeof(AbpCliCoreModule),
        typeof(AbpTextTemplatingScribanModule),
        typeof(AbpAutofacModule)
    )]
    public class AbpCliModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpCliModule>();
            });

            Configure<AbpCliOptions>(options =>
            {
                options.Commands.Clear();
                options.Commands[HelpCommand.Name] = typeof(HelpCommand);
                options.Commands[CreateCommand.Name] = typeof(CreateCommand);
                options.Commands[GenerateProxyCommand.Name] = typeof(GenerateProxyCommand);
                options.Commands[GenerateViewCommand.Name] = typeof(GenerateViewCommand);
            });

            Configure<AbpCliServiceProxyOptions>(options =>
            {
                options.Generators[TypeScriptServiceProxyGenerator.Name] = typeof(TypeScriptServiceProxyGenerator);
                options.Generators[CSharpServiceProxyGenerator.Name] = typeof(CSharpServiceProxyGenerator);
                options.Generators[FlutterServiceProxyGenerator.Name] = typeof(FlutterServiceProxyGenerator);
            });

            Configure<TypeScriptServiceProxyOptions>(options =>
            {
                options.ScriptGenerators[AxiosHttpApiScriptGenerator.Name] = new AxiosHttpApiScriptGenerator();
                options.ScriptGenerators[VbenAxiosHttpApiScriptGenerator.Name] = new VbenAxiosHttpApiScriptGenerator();
                options.ScriptGenerators[VbenDynamicHttpApiScriptGenerator.Name] = new VbenDynamicHttpApiScriptGenerator();
                options.ScriptGenerators[UniAppAxiosHttpApiScriptGenerator.Name] = new UniAppAxiosHttpApiScriptGenerator(); 
            });

            Configure<FlutterServiceProxyOptions>(options =>
            {
                options.ScriptGenerators[RestServiceScriptGenerator.Name] = new RestServiceScriptGenerator();
            });

            Configure<AbpCliViewGeneratorOptions>(options =>
            {
                options.Generators[VbenViewGenerator.Name] = typeof(VbenViewGenerator);
                options.Generators[FlutterViewGenerator.Name] = typeof(FlutterViewGenerator);
            });
        }
    }
}
