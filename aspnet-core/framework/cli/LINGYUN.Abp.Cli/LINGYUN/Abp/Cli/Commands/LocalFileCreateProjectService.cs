using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Cli.ProjectBuilding.Building;
using Volo.Abp.Cli.Utils;
using Volo.Abp.DependencyInjection;
using Volo.Abp.IO;

namespace LINGYUN.Abp.Cli.Commands
{
    public class LocalFileCreateProjectService : ICreateProjectService, ISingletonDependency
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

        protected ICmdHelper CmdHelper { get; }
        public ILogger<CreateCommand> Logger { get; set; }

        public LocalFileCreateProjectService(ICmdHelper cmdHelper)
        {
            CmdHelper = cmdHelper;

            Logger = NullLogger<CreateCommand>.Instance;
        }

        public async virtual Task CreateAsync(ProjectCreateArgs createArgs)
        {
            Logger.LogInformation("Execute dotnet command...");

            var dbm = createArgs.DatabaseManagementSystem switch
            {
                DatabaseManagementSystem.SQLite => "Sqlite",
                DatabaseManagementSystem.Oracle => "Oracle",
                DatabaseManagementSystem.OracleDevart => "OracleDevart",
                DatabaseManagementSystem.PostgreSQL => "PostgreSql",
                DatabaseManagementSystem.SQLServer => "SqlServer",
                _ => "MySQL",
            };
            var commandBuilder = new StringBuilder("dotnet new");
            commandBuilder.AppendFormat(" {0}", createArgs.TemplateName);
            commandBuilder.AppendFormat(" -n {0}", createArgs.SolutionName.ProjectName);
            commandBuilder.AppendFormat(" -o {0}", createArgs.OutputFolder);
            commandBuilder.AppendFormat(" --DatabaseManagement {0}", dbm);
            commandBuilder.AppendFormat(" --AuthenticationScheme {0}", createArgs.AuthenticationScheme);

            Logger.LogInformation("Execute command: " + commandBuilder.ToString());

            var cmdError = CmdHelper.RunCmdAndGetOutput(commandBuilder.ToString(), out bool isSuccessful);
            if (!isSuccessful)
            {
                Logger.LogError("Execute command error: " + cmdError);
                return;
            }

            Logger.LogInformation("Executed command: " + cmdError);

            var projectFiles = new List<FindFile>();

            Logger.LogInformation("Search Solution files.");
            SearchSolutionPath(projectFiles, createArgs.OutputFolder, 0);

            Logger.LogInformation("Rewrite Package and company name.");

            await TryReplacePackageAndCompanyNameWithProjectFile(
                projectFiles,
                createArgs.PackageName,
                createArgs.SolutionName.CompanyName,
                createArgs.DatabaseManagementSystem);

            Logger.LogInformation("Rewrite appsettings.json.");
            await TryReplaceAppSettingsWithProjectFile(
                projectFiles,
                createArgs.PackageName,
                createArgs.SolutionName.CompanyName,
                createArgs.SolutionName.ProjectName,
                createArgs.ConnectionString);

            Logger.LogInformation("Rewrite application url.");
            await TryReplaceApplicationUrlWithProjectFile(
                projectFiles,
                createArgs.ApplicationPort,
                createArgs.DaprPort);

            Logger.LogInformation("Rewrite package version.");
            await TryReplaceVersionWithProjectFile(
                projectFiles,
                createArgs.Version);

            Logger.LogInformation("Rewrite project folder.");
            await TryReplacePackageAndCompanyNameWithProjectFolder(
                projectFiles,
                createArgs.PackageName,
                createArgs.SolutionName.CompanyName);

            Logger.LogInformation($"'{createArgs.SolutionName.ProjectName}' has been successfully created to '{createArgs.OutputFolder}'");
        }

        protected virtual void SearchSolutionPath(List<FindFile> projectFiles, string solutionPath, int depth)
        {
            var searchFiles = Directory.GetFileSystemEntries(solutionPath, "*.*", SearchOption.TopDirectoryOnly);
            searchFiles = searchFiles.Where(f => !CreateOptions.ExclusionFolder.Any(ef => f.EndsWith(ef))).ToArray();
            foreach (var searchFile in searchFiles)
            {
                projectFiles.Add(new FindFile(solutionPath, searchFile, depth, Directory.Exists(searchFile)));
                if (Directory.Exists(searchFile))
                {
                    SearchSolutionPath(projectFiles, searchFile, depth++);
                }
            }
        }

        protected async virtual Task TryReplaceVersionWithProjectFile(
            List<FindFile> projectFiles,
            string version)
        {
            if (version.IsNullOrWhiteSpace())
            {
                return;
            }
            var assemblyVersion = GetType().Assembly.GetName().Version;
            var currentVersion = $"{assemblyVersion.Major}.{assemblyVersion.Minor}.{assemblyVersion.Build}";

            var buildPropsFile = projectFiles.FirstOrDefault(f => f.Name.EndsWith("Directory.Build.props"));
            if (buildPropsFile != null)
            {
                await ReplaceFileTextAsync(
                    buildPropsFile, 
                    $"<VoloAbpPackageVersion>{currentVersion}</VoloAbpPackageVersion>",
                    $"<VoloAbpPackageVersion>{version}</VoloAbpPackageVersion>");

                await ReplaceFileTextAsync(
                     buildPropsFile,
                     $"<LINGYUNAbpPackageVersion>{currentVersion}</LINGYUNAbpPackageVersion>",
                     $"<LINGYUNAbpPackageVersion>{version}</LINGYUNAbpPackageVersion>");
            }

            var commonPropsFile = projectFiles.FirstOrDefault(f => f.Name.EndsWith("common.props"));
            if (commonPropsFile != null)
            {
                await ReplaceFileTextAsync(
                     commonPropsFile,
                     $"<Version>{currentVersion}</Version>",
                     $"<Version>{version}</Version>");
            }
        }

        protected async virtual Task TryReplaceApplicationUrlWithProjectFile(
            List<FindFile> projectFiles,
            string port = "5000",
            string daprPort = "3500")
        {
            var launchFile = projectFiles.FirstOrDefault(f => f.Name.EndsWith("launchSettings.json"));
            if (launchFile != null)
            {
                string applicationUrl = $"http://127.0.0.1:{port}";
                await ReplaceFileTextAsync(launchFile, "http://127.0.0.1:5000", applicationUrl);
            }

            var daprScriptFile = projectFiles.FirstOrDefault(f => f.Name.EndsWith("dapr.sh"));
            if (daprScriptFile != null)
            {
                await ReplaceFileTextAsync(daprScriptFile, "--app-port 5000 -H 3500", $"--app-port {port} -H {daprPort}");
            }
        }

        protected async virtual Task TryReplaceAppSettingsWithProjectFile(
            List<FindFile> projectFiles,
            string packageName,
            string companyName,
            string projectName,
            string connectionString = null)
        {
            var canReplaceFiles = projectFiles.Where(f => !f.IsFolder && f.Name.Contains("appsettings"));
            foreach (var projectFile in canReplaceFiles)
            {
                await ReplaceFileTextAsync(projectFile, "PackageName", packageName);
                await ReplaceFileTextAsync(projectFile, "CompanyName", companyName);

                var defaultConnectionString = $"Server=127.0.0.1;Database={projectName};User Id=root;Password=123456";
                connectionString ??= defaultConnectionString;
                await ReplaceFileTextAsync(projectFile, defaultConnectionString, connectionString);
            }
        }

        protected async virtual Task TryReplacePackageAndCompanyNameWithProjectFile(
            List<FindFile> projectFiles,
            string packageName,
            string companyName,
            DatabaseManagementSystem database = DatabaseManagementSystem.NotSpecified)
        {
            var canReplaceFiles = projectFiles.Where(f => !f.IsFolder && !f.Name.Contains("appsettings"));
            foreach (var projectFile in canReplaceFiles)
            {
                await ReplaceFileTextAsync(projectFile, "PackageName", packageName);
                await ReplaceFileTextAsync(projectFile, "CompanyName", companyName);
            }
        }

        protected virtual Task TryReplacePackageAndCompanyNameWithProjectFolder(
            List<FindFile> projectFiles,
            string packageName,
            string companyName)
        {
            var canReplaceFiles = projectFiles
                .OrderByDescending(f => f.Depth)
                .OrderByDescending(f => !f.IsFolder);
            foreach (var projectFile in canReplaceFiles)
            {
                var replaceFileName = projectFile.Name.Replace("PackageName", packageName).Replace("CompanyName", companyName);

                if (File.Exists(projectFile.Name))
                {
                    DirectoryHelper.CreateIfNotExists(Path.GetDirectoryName(replaceFileName));
                    File.Move(projectFile.Name, replaceFileName);
                }
            }

            var canReplacePaths = projectFiles
                .Where(projectFile => projectFile.Name.Contains("PackageName") || projectFile.Name.Contains("CompanyName"))
                .OrderByDescending(f => f.Depth)
                .OrderByDescending(f => f.IsFolder);
            foreach (var projectFile in canReplacePaths)
            {
                DirectoryHelper.DeleteIfExists(projectFile.Name, true);
            }

            return Task.CompletedTask;
        }

        protected static async Task ReplaceFileTextAsync(FindFile projectFile, string sourceText, string replaceText)
        {
            string fileText;
            using (var stream = new StreamReader(projectFile.Name, Encoding.UTF8))
            {
                fileText = await stream.ReadToEndAsync();
            }
            fileText = fileText.Replace(sourceText, replaceText, StringComparison.InvariantCultureIgnoreCase);

            using (StreamWriter writer = new StreamWriter(projectFile.Name, false, Encoding.UTF8))
            {
                await writer.WriteAsync(fileText);
            }
        }
    }
}
