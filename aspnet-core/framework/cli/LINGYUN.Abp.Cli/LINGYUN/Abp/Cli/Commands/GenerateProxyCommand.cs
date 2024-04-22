using LINGYUN.Abp.Cli.ServiceProxying;
using LINGYUN.Abp.Cli.ServiceProxying.TypeScript;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Cli;
using Volo.Abp.Cli.Args;
using Volo.Abp.Cli.Commands;
using Volo.Abp.DependencyInjection;
using AbpCliServiceProxyOptions = Volo.Abp.Cli.ServiceProxying.AbpCliServiceProxyOptions;
using IServiceProxyGenerator = Volo.Abp.Cli.ServiceProxying.IServiceProxyGenerator;
using VoloProxyOptions = Volo.Abp.Cli.Commands.ProxyCommandBase<Volo.Abp.Cli.Commands.GenerateProxyCommand>.Options;
using VoloGenerateProxyArgs = Volo.Abp.Cli.ServiceProxying.GenerateProxyArgs;
using ServiceType = Volo.Abp.Cli.ServiceProxying.ServiceType;

namespace LINGYUN.Abp.Cli.Commands;

public class GenerateProxyCommand : IConsoleCommand, ITransientDependency
{
    public const string Name = "generate-proxy";

    protected string CommandName => Name;

    protected AbpCliServiceProxyOptions ServiceProxyOptions { get; }

    protected IServiceScopeFactory ServiceScopeFactory { get; }

    public GenerateProxyCommand(
        IOptions<AbpCliServiceProxyOptions> serviceProxyOptions,
        IServiceScopeFactory serviceScopeFactory)
    {
        ServiceScopeFactory = serviceScopeFactory;
        ServiceProxyOptions = serviceProxyOptions.Value;
    }

    public async Task ExecuteAsync(CommandLineArgs commandLineArgs)
    {
        var generateType = commandLineArgs.Options.GetOrNull(Options.GenerateType.Short, Options.GenerateType.Long)?.ToUpper();

        if (!ServiceProxyOptions.Generators.ContainsKey(generateType))
        {
            throw new CliUsageException("Option Type value is invalid" +
                Environment.NewLine +
                GetUsageInfo());
        }

        using var scope = ServiceScopeFactory.CreateScope();
        var generatorType = ServiceProxyOptions.Generators[generateType];
        var serviceProxyGenerator = scope.ServiceProvider.GetService(generatorType).As<IServiceProxyGenerator>();

        await serviceProxyGenerator.GenerateProxyAsync(BuildArgs(commandLineArgs));
    }

    private VoloGenerateProxyArgs BuildArgs(CommandLineArgs commandLineArgs)
    {
        var provider = commandLineArgs.Options.GetOrNull(Options.Provider.Short, Options.Provider.Long);
        var apiScriptProxy = commandLineArgs.Options.GetOrNull(Options.ApiScriptProxy.Short, Options.ApiScriptProxy.Long) ??
            VbenDynamicHttpApiScriptGenerator.Name;
        var url = commandLineArgs.Options.GetOrNull(VoloProxyOptions.Url.Short, VoloProxyOptions.Url.Long);
        var target = commandLineArgs.Options.GetOrNull(VoloProxyOptions.Target.Long);
        var module = commandLineArgs.Options.GetOrNull(VoloProxyOptions.Module.Short, VoloProxyOptions.Module.Long) ?? "app";
        var output = commandLineArgs.Options.GetOrNull(VoloProxyOptions.Output.Short, VoloProxyOptions.Output.Long);
        var apiName = commandLineArgs.Options.GetOrNull(VoloProxyOptions.ApiName.Short, VoloProxyOptions.ApiName.Long);
        var source = commandLineArgs.Options.GetOrNull(VoloProxyOptions.Source.Short, VoloProxyOptions.Source.Long);
        var workDirectory = commandLineArgs.Options.GetOrNull(VoloProxyOptions.WorkDirectory.Short, VoloProxyOptions.WorkDirectory.Long) ?? Directory.GetCurrentDirectory();
        var folder = commandLineArgs.Options.GetOrNull(VoloProxyOptions.Folder.Long);
        var serviceTypeArg = commandLineArgs.Options.GetOrNull(VoloProxyOptions.ServiceType.Short, VoloProxyOptions.ServiceType.Long);
        var entryPoint = commandLineArgs.Options.GetOrNull(VoloProxyOptions.EntryPoint.Short, VoloProxyOptions.EntryPoint.Long);

        ServiceType? serviceType = null;
        if (!serviceTypeArg.IsNullOrWhiteSpace())
        {
            serviceType = serviceTypeArg.ToLower() == "application"
                ? ServiceType.Application
                : serviceTypeArg.ToLower() == "integration"
                    ? ServiceType.Integration
                    : null;
        }

        var withoutContracts = commandLineArgs.Options.ContainsKey(VoloProxyOptions.WithoutContracts.Short) ||
                               commandLineArgs.Options.ContainsKey(VoloProxyOptions.WithoutContracts.Long);

        return new GenerateProxyArgs(
            CommandName,
            workDirectory, 
            module, 
            url, 
            output, 
            target, 
            apiName,
            source, 
            folder, 
            provider,
            apiScriptProxy,
            serviceType,
            entryPoint,
            withoutContracts,
            commandLineArgs.Options);
    }

    public string GetUsageInfo()
    {
        var sb = new StringBuilder();

        sb.AppendLine("");
        sb.AppendLine("Usage:");
        sb.AppendLine("");
        sb.AppendLine($"  labp {CommandName}");
        sb.AppendLine("");
        sb.AppendLine("Options:");
        sb.AppendLine("");
        sb.AppendLine("-m|--module <module-name>                         (default: 'app') The name of the backend module you wish to generate proxies for.");
        sb.AppendLine("-wd|--working-directory <directory-path>          Execution directory.");
        sb.AppendLine("-u|--url <url>                                    API definition URL from.");
        sb.AppendLine("-t|--type <generate-type>                         The name of generate type (csharp, ts).");
        sb.AppendLine("  csharp");
        sb.AppendLine("    --folder <folder-name>                            (default: 'ClientProxies') Folder name to place generated CSharp code in.");
        sb.AppendLine("  ts");
        sb.AppendLine("    -asp|--api-script-proxy <api-script-proxy>        The generated api proxy type(axios, vben-axios, vben-dynamic). default: vben-dynamic.");
        sb.AppendLine("    -o|--output <output-name>                         TypeScript file path or folder to place generated code in.");
        sb.AppendLine("  flutter");
        sb.AppendLine("    -asp|--api-script-proxy <api-script-proxy>        The generated api proxy type(dio, rest-service). default: rest-service.");
        sb.AppendLine("    -o|--output <output-name>                         Flutter script file path or folder to place generated code in.");

        sb.AppendLine("-p|--provider <client-proxy-provider>             The client proxy provider(http, dapr).");
        sb.AppendLine("See the documentation for more info: https://docs.abp.io/en/abp/latest/CLI");

        sb.AppendLine("");
        sb.AppendLine("Examples:");
        sb.AppendLine("");
        sb.AppendLine("  labp generate-proxy");
        sb.AppendLine("  labp generate-proxy -p dapr");
        sb.AppendLine("  labp generate-proxy -m identity -o Pages/Identity/client-proxies.js -url https://localhost:44302/");
        sb.AppendLine("  labp generate-proxy --folder MyProxies/InnerFolder -url https://localhost:44302/");
        sb.AppendLine("  labp generate-proxy -t ts -m identity -o api/identity -url https://localhost:44302/");
        sb.AppendLine("  labp generate-proxy -t ts -asp vben-dynamic -m identity -o api/identity -url https://localhost:44302/");
        sb.AppendLine("  labp generate-proxy -t flutter -asp rest-service -m identity -o api/identity -url https://localhost:44302/");

        return sb.ToString();
    }

    public string GetShortDescription()
    {
        return "Generates client service proxies and DTOs to consume HTTP APIs.";
    }

    public static class Options
    {
        public static class GenerateType
        {
            public const string Short = "t";
            public const string Long = "type";
        }

        public static class Provider
        {
            public const string Short = "p";
            public const string Long = "provider";
        }

        public static class ApiScriptProxy
        {
            public const string Short = "asp";
            public const string Long = "api-script-proxy";
        }
    }
}
