using LINGYUN.Abp.Cli.Blob;
using LINGYUN.Abp.Cli.Commands;
using LINGYUN.Abp.Cli.ServiceProxying.CSharp;
using LINGYUN.Abp.Cli.ServiceProxying.Flutter;
using LINGYUN.Abp.Cli.ServiceProxying.TypeScript;
using LINGYUN.Abp.Cli.UI;
using LINGYUN.Abp.Cli.UI.Flutter.GetX;
using LINGYUN.Abp.Cli.UI.Vben;
using Microsoft.Extensions.DependencyInjection;
using System;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.Cli;
using Volo.Abp.Cli.ServiceProxying;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.TextTemplating.Scriban;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.Cli
{
    [DependsOn(
        typeof(AbpCliCoreModule),
        typeof(AbpHttpClientModule),
        typeof(AbpTextTemplatingScribanModule),
        typeof(AbpAutofacModule)
    )]
    public class AbpCliModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddSingleton<ICurrentBlob, CurrentBlob>();
            context.Services.AddTransient<BlobHttpClientHandler>();

            PreConfigure<AbpHttpClientBuilderOptions>(options =>
            {
                options.ProxyClientBuildActions.Add((remoteService, builder) =>
                {
                    builder.AddHttpMessageHandler<BlobHttpClientHandler>();
                });
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddStaticHttpClientProxies(
                typeof(AbpCliModule).Assembly,
                "BlobManagement"
             );

            Configure<AbpRemoteServiceOptions>(options =>
            {
                options.RemoteServices.Add("BlobManagement", new RemoteServiceConfiguration
                {
                    BaseUrl = "http://localhost:8080"
                });
            });

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
                options.Commands[BlobMigrationCommand.Name] = typeof(BlobMigrationCommand);
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
