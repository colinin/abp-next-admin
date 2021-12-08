using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using Volo.Abp.Cli;
using Volo.Abp.Cli.Args;
using Volo.Abp.Cli.Commands;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Cli.Commands
{
    [Dependency(ReplaceServices = true)]
    public class CommandSelector : ICommandSelector, ITransientDependency
    {
        protected AbpCliOptions Options { get; }

        public CommandSelector(IOptions<AbpCliOptions> options)
        {
            Options = options.Value;
        }

        public Type Select(CommandLineArgs commandLineArgs)
        {
            if (commandLineArgs.Command.IsNullOrWhiteSpace())
            {
                return typeof(HelpCommand);
            }

            return Options.Commands.GetOrDefault(commandLineArgs.Command)
                   ?? typeof(HelpCommand);
        }
    }
}
