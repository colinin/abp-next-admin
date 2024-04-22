﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Cli;
using Volo.Abp.Cli.Args;
using Volo.Abp.Cli.Commands;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Cli.Commands
{
    public class HelpCommand : IConsoleCommand, ITransientDependency
    {
        public const string Name = "help";

        protected string CommandName => Name;

        public ILogger<HelpCommand> Logger { get; set; }
        protected AbpCliOptions AbpCliOptions { get; }
        protected IServiceScopeFactory ServiceScopeFactory { get; }

        public HelpCommand(IOptions<AbpCliOptions> cliOptions,
            IServiceScopeFactory serviceScopeFactory)
        {
            ServiceScopeFactory = serviceScopeFactory;
            Logger = NullLogger<HelpCommand>.Instance;
            AbpCliOptions = cliOptions.Value;
        }

        public Task ExecuteAsync(CommandLineArgs commandLineArgs)
        {
            if (string.IsNullOrWhiteSpace(commandLineArgs.Target))
            {
                Logger.LogInformation(GetUsageInfo());
                return Task.CompletedTask;
            }

            if (!AbpCliOptions.Commands.ContainsKey(commandLineArgs.Target))
            {
                Logger.LogWarning($"There is no command named {commandLineArgs.Target}.");
                Logger.LogInformation(GetUsageInfo());
                return Task.CompletedTask;
            }

            var commandType = AbpCliOptions.Commands[commandLineArgs.Target];

            using (var scope = ServiceScopeFactory.CreateScope())
            {
                var command = (IConsoleCommand)scope.ServiceProvider.GetRequiredService(commandType);
                Logger.LogInformation(command.GetUsageInfo());
            }

            return Task.CompletedTask;
        }

        public string GetUsageInfo()
        {
            var sb = new StringBuilder();

            sb.AppendLine("");
            sb.AppendLine("Usage:");
            sb.AppendLine("");
            sb.AppendLine("    labp <command> <target> [options]");
            sb.AppendLine("");
            sb.AppendLine("Command List:");
            sb.AppendLine("");

            foreach (var command in AbpCliOptions.Commands.ToArray())
            {
                string shortDescription;

                using (var scope = ServiceScopeFactory.CreateScope())
                {
                    shortDescription = ((IConsoleCommand)scope.ServiceProvider
                            .GetRequiredService(command.Value)).GetShortDescription();
                }

                sb.Append("    > ");
                sb.Append(command.Key);
                sb.Append(string.IsNullOrWhiteSpace(shortDescription) ? "" : ":");
                sb.Append(" ");
                sb.AppendLine(shortDescription);
            }

            sb.AppendLine("");
            sb.AppendLine("To get a detailed help for a command:");
            sb.AppendLine("");
            sb.AppendLine("    labp help <command>");
            sb.AppendLine("");

            return sb.ToString();
        }

        public string GetShortDescription()
        {
            return "Show command line help. Write ` labp help <command> `";
        }
    }
}
