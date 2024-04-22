using LINGYUN.Abp.Cli.UI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Cli;
using Volo.Abp.Cli.Args;
using Volo.Abp.Cli.Commands;
using Volo.Abp.Cli.ServiceProxying;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Cli.Commands;
public class GenerateViewCommand : IConsoleCommand, ITransientDependency
{
    public const string Name = "generate-view";

    protected string CommandName => Name;

    protected AbpCliViewGeneratorOptions ViewGeneratorOptions { get; }

    protected IServiceScopeFactory ServiceScopeFactory { get; }

    public GenerateViewCommand(
        IOptions<AbpCliViewGeneratorOptions> viewGeneratorOptions,
        IServiceScopeFactory serviceScopeFactory)
    {
        ServiceScopeFactory = serviceScopeFactory;
        ViewGeneratorOptions = viewGeneratorOptions.Value;
    }

    public async Task ExecuteAsync(CommandLineArgs commandLineArgs)
    {
        var generateType = commandLineArgs.Options.GetOrNull(Options.GenerateType.Short, Options.GenerateType.Long)?.ToUpper();

        if (!ViewGeneratorOptions.Generators.ContainsKey(generateType))
        {
            throw new CliUsageException("Option Type value is invalid" +
            Environment.NewLine +
                GetUsageInfo());
        }

        using var scope = ServiceScopeFactory.CreateScope();
        var generatorType = ViewGeneratorOptions.Generators[generateType];
        var serviceProxyGenerator = scope.ServiceProvider.GetService(generatorType).As<IViewGenerator>();

        await serviceProxyGenerator.GenerateAsync(BuildArgs(commandLineArgs));
    }

    private GenerateViewArgs BuildArgs(CommandLineArgs commandLineArgs)
    {
        var url = commandLineArgs.Options.GetOrNull(Options.Url.Short, Options.Url.Long);
        var output = commandLineArgs.Options.GetOrNull(Options.Output.Short, Options.Output.Long);
        var module = commandLineArgs.Options.GetOrNull(Options.Module.Short, Options.Module.Long) ?? "app";
        var serviceTypeArg = commandLineArgs.Options.GetOrNull(Options.ServiceType.Short, Options.ServiceType.Long);

        ServiceType? serviceType = null;
        if (!serviceTypeArg.IsNullOrWhiteSpace())
        {
            serviceType = serviceTypeArg.ToLower() == "application"
                ? ServiceType.Application
                : serviceTypeArg.ToLower() == "integration"
                    ? ServiceType.Integration
                    : null;
        }

        return new GenerateViewArgs(
            module,
            url,
            output,
            serviceType);
    }

    public string GetShortDescription()
    {
        return "Generate the view code from the http api proxy.";
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
        sb.AppendLine("-t|--type <type>                         The name of generate type (vben-view).");
        sb.AppendLine("     vben-view");
        sb.AppendLine("         -o|--output <output-name>                         js/vue file path or folder to place generated code in.");
        sb.AppendLine("     flutter-view");
        sb.AppendLine("         -o|--output <output-name>                         flutter file path or folder to place generated code in.");
        sb.AppendLine("-u|--url <url>                                    API definition URL from.");
        sb.AppendLine("-m|--module <module-name>                         (default: 'app') The name of the backend module you wish to generate proxies for.");
        sb.AppendLine("");
        sb.AppendLine("Examples:");
        sb.AppendLine("");

        sb.AppendLine("  labp generate-proxy -t vben-view -m identity -o api/identity -url https://localhost:44302/");
        sb.AppendLine("  labp generate-proxy -t flutter-view -m identity -o api/identity -url https://localhost:44302/");

        return sb.ToString();
    }

    public static class Options
    {
        public static class GenerateType
        {
            public const string Short = "t";
            public const string Long = "type";
        }

        public static class Module
        {
            public const string Short = "m";
            public const string Long = "module";
        }

        public static class Output
        {
            public const string Short = "o";
            public const string Long = "output";
        }

        public static class Folder
        {
            public const string Long = "folder";
        }

        public static class Url
        {
            public const string Short = "u";
            public const string Long = "url";
        }

        public static class ServiceType
        {
            public const string Short = "st";
            public const string Long = "service-type";
        }
    }
}
