using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Cli;
using Volo.Abp.Cli.Args;
using Volo.Abp.Cli.Commands;
using Volo.Abp.Cli.Commands.Services;
using Volo.Abp.Cli.ProjectBuilding;
using Volo.Abp.Cli.ProjectBuilding.Building;
using Volo.Abp.Cli.Utils;
using Volo.Abp.DependencyInjection;
using Volo.Abp.IO;

namespace LINGYUN.Abp.Cli.Commands
{
    public class CreateCommand : IConsoleCommand, ITransientDependency
    {
        public class FindFile
        {
            public string Path { get; }
            public string Name { get; }
            public int Depth { get; }
            public bool IsFolder { get; }
            public FindFile() { }
            public FindFile(
                string path,
                string name,
                int depth,
                bool isFolder)
            {
                Path = path;
                Name = name;
                Depth = depth;
                IsFolder = isFolder;
            }
        }

        public ILogger<CreateCommand> Logger { get; set; }

        public ConnectionStringProvider ConnectionStringProvider { get; }

        public ICreateProjectService CreateProjectService { get; }

        public CreateCommand(
            ICreateProjectService createProjectService,
            ConnectionStringProvider connectionStringProvider)
        {
            CreateProjectService = createProjectService;
            ConnectionStringProvider = connectionStringProvider;

            Logger = NullLogger<CreateCommand>.Instance;
        }

        public async Task ExecuteAsync(CommandLineArgs commandLineArgs)
        {
            var projectName = NamespaceHelper.NormalizeNamespace(commandLineArgs.Target);
            if (string.IsNullOrWhiteSpace(projectName))
            {
                throw new CliUsageException("Project name is missing!" + Environment.NewLine + Environment.NewLine + GetUsageInfo());
            }

            var packageName = commandLineArgs.Options.GetOrNull(CreateOptions.Package.Short, CreateOptions.Package.Long);
            packageName ??= "app";

            Logger.LogInformation("Package Name: " + packageName);

            var version = commandLineArgs.Options.GetOrNull(NewCommand.Options.Version.Short, NewCommand.Options.Version.Long);

            if (version != null)
            {
                Logger.LogInformation("Version: " + version);
            }

            var templateName = commandLineArgs.Options.GetOrNull(NewCommand.Options.Template.Short, NewCommand.Options.Template.Long);
            templateName ??= "lam";

            Logger.LogInformation("Template: " + templateName);

            var gitHubAbpLocalRepositoryPath = commandLineArgs.Options.GetOrNull(NewCommand.Options.GitHubAbpLocalRepositoryPath.Long);
            if (gitHubAbpLocalRepositoryPath != null)
            {
                Logger.LogInformation("GitHub Abp Local Repository Path: " + gitHubAbpLocalRepositoryPath);
            }

            var gitHubVoloLocalRepositoryPath = commandLineArgs.Options.GetOrNull(NewCommand.Options.GitHubVoloLocalRepositoryPath.Long);
            if (gitHubVoloLocalRepositoryPath != null)
            {
                Logger.LogInformation("GitHub Volo Local Repository Path: " + gitHubVoloLocalRepositoryPath);
            }

            var databaseProvider = GetDatabaseProvider(commandLineArgs);
            if (databaseProvider != DatabaseProvider.NotSpecified)
            {
                Logger.LogInformation("Database provider: " + databaseProvider);
            }

            var connectionString = GetConnectionString(commandLineArgs);
            if (connectionString != null)
            {
                Logger.LogInformation("Connection string: " + connectionString);
            }

            var databaseManagementSystem = GetDatabaseManagementSystem(commandLineArgs);
            if (databaseManagementSystem != DatabaseManagementSystem.NotSpecified)
            {
                Logger.LogInformation("DBMS: " + databaseManagementSystem);
            }

            var randomPort = string.IsNullOrWhiteSpace(
                commandLineArgs.Options.GetOrNull(CreateOptions.NoRandomPort.Short, CreateOptions.NoRandomPort.Long));
            var applicationPort = randomPort ? RandomHelper.GetRandom(5001, 65535).ToString() : "5000";

            Logger.LogInformation("Application Launch Port: " + applicationPort);

            var daprPort = randomPort ? RandomHelper.GetRandom(3501, 65535).ToString() : "3500";

            Logger.LogInformation("Dapr Listening Http Port: " + daprPort);

            var createSolutionFolder = GetCreateSolutionFolderPreference(commandLineArgs);
            var outputFolder = commandLineArgs.Options.GetOrNull(NewCommand.Options.OutputFolder.Short, NewCommand.Options.OutputFolder.Long);

            var outputFolderRoot =
                outputFolder != null ? Path.GetFullPath(outputFolder) : Directory.GetCurrentDirectory();

            outputFolder = createSolutionFolder ?
                Path.Combine(outputFolderRoot, SolutionName.Parse(projectName).FullName) :
                outputFolderRoot;

            var solutionName = SolutionName.Parse(projectName);

            DirectoryHelper.CreateIfNotExists(outputFolder);

            Logger.LogInformation("Output folder: " + outputFolder);

            if (connectionString == null &&
               databaseManagementSystem != DatabaseManagementSystem.NotSpecified &&
               databaseManagementSystem != DatabaseManagementSystem.SQLServer)
            {
                connectionString = ConnectionStringProvider.GetByDbms(databaseManagementSystem, outputFolder);
            }

            commandLineArgs.Options.Add(CliConsts.Command, commandLineArgs.Command);

            var projectArgs =  new ProjectCreateArgs(
                packageName,
                solutionName,
                templateName,
                version,
                outputFolder,
                databaseProvider,
                databaseManagementSystem,
                UiFramework.None,
                null,
                false,
                gitHubAbpLocalRepositoryPath,
                gitHubVoloLocalRepositoryPath,
                "",
                commandLineArgs.Options,
                connectionString,
                applicationPort,
                daprPort
            );

            await CreateProjectService.CreateAsync(projectArgs);
        }

        public string GetShortDescription()
        {
            return "Generate a new solution based on the customed ABP startup templates.";
        }

        public string GetUsageInfo()
        {
            var sb = new StringBuilder();

            sb.AppendLine("");
            sb.AppendLine("Usage:");
            sb.AppendLine("");
            sb.AppendLine("  labp create <project-name> [options]");
            sb.AppendLine("");
            sb.AppendLine("Options:");
            sb.AppendLine("");
            sb.AppendLine("-pk|--package <package-name>               (default: app)");
            sb.AppendLine("-t|--template <template-name>               (default: lam)");
            sb.AppendLine("-d|--database-provider <database-provider>  (if supported by the template)");
            sb.AppendLine("-o|--output-folder <output-folder>          (default: current folder)");
            sb.AppendLine("-v|--version <version>                      (default: latest version)");
            //sb.AppendLine("-ts|--template-source <template-source>     (your local or network abp template source)");
            sb.AppendLine("-csf|--create-solution-folder               (default: true)");
            sb.AppendLine("-cs|--connection-string <connection-string> (your database connection string)");
            sb.AppendLine("--dbms <database-management-system>         (your database management system)");
            sb.AppendLine("--no-random-port                            (Use template's default ports)");
            sb.AppendLine("");
            sb.AppendLine("Examples:");
            sb.AppendLine("");
            sb.AppendLine("  labp create Acme.BookStore");
            sb.AppendLine("  labp create Acme.BookStore -p Com");
            sb.AppendLine("  labp create Acme.BookStore -d mongodb");
            sb.AppendLine("  labp create Acme.BookStore -d mongodb -o d:\\my-project");
            sb.AppendLine("  labp create Acme.BookStore -ts \"D:\\localTemplate\\abp\"");
            sb.AppendLine("  labp create Acme.BookStore -csf false");
            sb.AppendLine("  labp create Acme.BookStore --local-framework-ref --abp-path \"D:\\github\\abp\"");
            sb.AppendLine("  labp create Acme.BookStore --dbms mysql");
            sb.AppendLine("  labp create Acme.BookStore --connection-string \"Server=myServerName\\myInstanceName;Database=myDatabase;User Id=myUsername;Password=myPassword\"");
            sb.AppendLine("");
            // TODO: 文档
            // sb.AppendLine("See the documentation for more info: https://docs.abp.io/en/abp/latest/CLI");

            return sb.ToString();
        }

        protected bool GetCreateSolutionFolderPreference(CommandLineArgs commandLineArgs)
        {
            return commandLineArgs.Options.ContainsKey(NewCommand.Options.CreateSolutionFolder.Long)
                || commandLineArgs.Options.ContainsKey(NewCommand.Options.CreateSolutionFolder.Short);
        }

        protected virtual DatabaseProvider GetDatabaseProvider(CommandLineArgs commandLineArgs)
        {
            var optionValue = commandLineArgs.Options.GetOrNull(
                NewCommand.Options.DatabaseProvider.Short,
                NewCommand.Options.DatabaseProvider.Long);
            switch (optionValue)
            {
                case "ef":
                    return DatabaseProvider.EntityFrameworkCore;
                case "mongodb":
                    return DatabaseProvider.MongoDb;
                default:
                    return DatabaseProvider.NotSpecified;
            }
        }

        protected virtual DatabaseManagementSystem GetDatabaseManagementSystem(CommandLineArgs commandLineArgs)
        {
            var optionValue = commandLineArgs.Options.GetOrNull(
                NewCommand.Options.DatabaseManagementSystem.Short,
                NewCommand.Options.DatabaseManagementSystem.Long);

            if (optionValue == null)
            {
                return DatabaseManagementSystem.NotSpecified;
            }

            switch (optionValue.ToLowerInvariant())
            {
                case "sqlserver":
                    return DatabaseManagementSystem.SQLServer;
                case "mysql":
                    return DatabaseManagementSystem.MySQL;
                case "postgresql":
                    return DatabaseManagementSystem.PostgreSQL;
                case "oracle-devart":
                    return DatabaseManagementSystem.OracleDevart;
                case "sqlite":
                    return DatabaseManagementSystem.SQLite;
                case "oracle":
                    return DatabaseManagementSystem.Oracle;
                default:
                    return DatabaseManagementSystem.NotSpecified;
            }
        }

        protected static string GetConnectionString(CommandLineArgs commandLineArgs)
        {
            var connectionString = commandLineArgs.Options.GetOrNull(
                NewCommand.Options.ConnectionString.Short,
                NewCommand.Options.ConnectionString.Long);
            return string.IsNullOrWhiteSpace(connectionString) ? null : connectionString;
        }
    }
}
