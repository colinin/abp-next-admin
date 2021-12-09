using System.Collections.Generic;
using Volo.Abp.Cli.ProjectBuilding;
using Volo.Abp.Cli.ProjectBuilding.Building;

namespace LINGYUN.Abp.Cli.Commands
{
    public class ProjectCreateArgs : ProjectBuildArgs
    {
        public string PackageName { get; }
        public string ApplicationPort { get; }
        public string DaprPort { get; }
        public ProjectCreateArgs(
            string packageName,
            SolutionName solutionName, 
            string templateName = null, 
            string version = null,
            string outputFolder = null,
            DatabaseProvider databaseProvider = DatabaseProvider.NotSpecified, 
            DatabaseManagementSystem databaseManagementSystem = DatabaseManagementSystem.NotSpecified, 
            UiFramework uiFramework = UiFramework.NotSpecified,
            MobileApp? mobileApp = null, 
            bool publicWebSite = false, 
            string abpGitHubLocalRepositoryPath = null, 
            string voloGitHubLocalRepositoryPath = null, 
            string templateSource = null, 
            Dictionary<string, string> extraProperties = null, 
            string connectionString = null,
            string applicationPort = "5000",
            string daprPort = "3500") 
            : base(
                  solutionName, 
                  templateName, 
                  version,
                  outputFolder,
                  databaseProvider,
                  databaseManagementSystem,
                  uiFramework, 
                  mobileApp, 
                  publicWebSite, 
                  abpGitHubLocalRepositoryPath,
                  voloGitHubLocalRepositoryPath, 
                  templateSource,
                  extraProperties, 
                  connectionString)
        {
            PackageName = packageName;
            ApplicationPort = applicationPort;
            DaprPort = daprPort;
        }
    }
}
