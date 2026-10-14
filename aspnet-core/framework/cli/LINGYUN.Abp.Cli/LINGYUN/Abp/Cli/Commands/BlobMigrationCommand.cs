using LINGYUN.Abp.BlobManagement.Integration;
using LINGYUN.Abp.Cli.Blob;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Volo.Abp.Cli.Args;
using Volo.Abp.Cli.Commands;
using Volo.Abp.Content;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client;

namespace LINGYUN.Abp.Cli.Commands;

public class BlobMigrationCommand : IConsoleCommand, ITransientDependency
{
    public const string Name = "blob-migration";

    protected string CommandName => Name;

    protected IBlobIntegrationService BlobIntegrationService { get; }
    protected ICurrentBlob CurrentBlob { get; }
    public ILogger<BlobMigrationCommand> Logger { protected get; set; }

    public BlobMigrationCommand(
        IBlobIntegrationService blobIntegrationService,
        ICurrentBlob currentBlob)
    {
        BlobIntegrationService = blobIntegrationService;
        CurrentBlob = currentBlob;

        Logger = NullLogger<BlobMigrationCommand>.Instance;
    }

    public async virtual Task ExecuteAsync(CommandLineArgs commandLineArgs)
    {
        var workingDirectory = commandLineArgs.Options.GetOrNull(BlobCommandOptions.WorkingDirectory.Short, BlobCommandOptions.WorkingDirectory.Long);
        if (workingDirectory.IsNullOrWhiteSpace())
        {
            workingDirectory = Directory.GetCurrentDirectory();
        }
        if (workingDirectory != null)
        {
            Logger.LogInformation("Working Directory: {workingDirectory}", workingDirectory);
        }
        if (!Directory.Exists(workingDirectory))
        {
            Logger.LogWarning("The working directory: {workingDirectory} does not exist. Please check if the input is valid!", workingDirectory);
            return;
        }

        var searchOption = SearchOption.TopDirectoryOnly;
        var searchOptionArg = commandLineArgs.Options.GetOrNull(BlobCommandOptions.SearchOption.Short, BlobCommandOptions.SearchOption.Long)
            ?? "top";
        if (searchOptionArg != null)
        {
            Logger.LogInformation("Search Option: {searchOptionArg}", searchOptionArg);
            searchOption = searchOptionArg switch
            {
                "all" => SearchOption.AllDirectories,
                _ => SearchOption.TopDirectoryOnly,
            };
        }
        var searchPattern = commandLineArgs.Options.GetOrNull(BlobCommandOptions.SearchPattern.Short, BlobCommandOptions.SearchPattern.Long)
            ?? "*.*";
        if (searchPattern != null)
        {
            Logger.LogInformation("Search Pattern: {searchPattern}", searchPattern);
        }
        var bucketName = commandLineArgs.Options.GetOrNull(BlobCommandOptions.BucketName.Short, BlobCommandOptions.BucketName.Long)
            ?? "blobs";
        if (bucketName != null)
        {
            Logger.LogInformation("Bucket Name: {bucketName}", bucketName);
        }
        var pathName = commandLineArgs.Options.GetOrNull(BlobCommandOptions.PathName.Short, BlobCommandOptions.PathName.Long);
        if (pathName != null)
        {
            Logger.LogInformation("Path Name: {pathName}", pathName);
        }
        var objectName = commandLineArgs.Options.GetOrNull(BlobCommandOptions.ObjectName.Short, BlobCommandOptions.ObjectName.Long);
        if (objectName != null)
        {
            Logger.LogInformation("Object Name: {objectName}", objectName);
        }
        var baseUrl = commandLineArgs.Options.GetOrNull(BlobCommandOptions.Url.Short, BlobCommandOptions.Url.Long);
        if (baseUrl != null)
        {
            Logger.LogInformation("Base Url: {baseUrl}", baseUrl);
        }
        var tenantId = commandLineArgs.Options.GetOrNull(BlobCommandOptions.Tenant.Short, BlobCommandOptions.Tenant.Long);
        if (tenantId != null)
        {
            Logger.LogInformation("Tenant Id: {tenantId}", tenantId);
        }

        CurrentBlob.BaseUrl = baseUrl;
        CurrentBlob.TenantId = tenantId;
        var isListFile = commandLineArgs.Options.ContainsKey(BlobCommandOptions.ListFile.Short) 
            || commandLineArgs.Options.ContainsKey(BlobCommandOptions.ListFile.Long);
        var overwrite = commandLineArgs.Options.ContainsKey(BlobCommandOptions.Overwrite.Short)
            || commandLineArgs.Options.ContainsKey(BlobCommandOptions.Overwrite.Long);
        Logger.LogInformation("Overwrite: {overwrite}", overwrite);

        var successd = 0;
        var failed = 0;
        var ignored = 0;
        var files = Directory.GetFiles(workingDirectory, searchPattern, searchOption);
        Logger.LogInformation("In the directory: {directory}, a total of {count} files that match the search pattern: {pattern} were found.", workingDirectory, files.Length, searchPattern);

        foreach (var file in files)
        {
            var defaultBlobPath = Directory.GetParent(file).FullName
                .RemovePreFix(workingDirectory)
                .Replace("\\", "/");
            var defaultBlobName = Path.GetFileName(file)
                .RemovePreFix(workingDirectory)
                .Replace("\\", "/");
            var fileName = Path.GetFileName(file);
            var blobPath = pathName ?? defaultBlobPath;
            if (!blobPath.IsNullOrWhiteSpace())
            {
                blobPath = blobPath.EnsureEndsWith('/');
            }
            var blobName = blobPath + (objectName ?? defaultBlobName).RemovePreFix("/");

            if (isListFile)
            {
                Logger.LogInformation("Container: {containerName}, Blob: {blobName}", bucketName, blobName);
            }
            else
            {
                Logger.LogInformation("Starting to upload file: {blobName}.", blobName);
                try
                {
                    if (!overwrite)
                    {
                        var blobExists = await BlobIntegrationService.ExistsAsync(
                            new BlobManagement.Integration.Dtos.BlobFileGetByNameIntegrationDto
                            {
                                ContainerName = bucketName,
                                BlobName = HttpUtility.UrlEncode(blobName),
                            });
                        if (blobExists)
                        {
                            ignored += 1;
                            Logger.LogInformation("File: {blobName} already exists. Skipping the upload of this file!", blobName);
                            continue;
                        }
                    }
                    using (var fs = File.OpenRead(file))
                    {
                        var blobDto = await BlobIntegrationService.CreateAsync(
                            new BlobManagement.Integration.Dtos.BlobFileCreateIntegrationDto
                            {
                                ContainerName = bucketName,
                                BlobName = HttpUtility.UrlEncode(blobName),
                                File = new RemoteStreamContent(fs, fileName),
                                Overwrite = overwrite,
                            });

                        if (blobDto != null)
                        {
                            successd += 1;
                            Logger.LogInformation("File: {blobName} has been uploaded successfully, Id: {id}", blobName, blobDto.Id.ToString());
                        }
                        else
                        {
                            failed += 1;
                            Logger.LogWarning("File: {blobName} upload failed!", blobName);
                        }
                    }
                }
                catch (AbpRemoteCallException cex)
                {
                    failed += 1;
                    Logger.LogWarning("File: {blobName} upload error, Code: {code}, message: {message}", blobName, cex.Code, cex.Message);
                }
                catch (Exception ex)
                {
                    failed += 1;
                    Logger.LogWarning("File: {blobName} upload error, message: {message}", blobName, ex.Message);
                }
            }
        }

        if (!isListFile)
        {
            Logger.LogInformation("{count} file upload tasks have been completed. Ignore: {ignored}, Success: {successd}, Failure: {failed}!", files.Length, ignored, successd, failed);
        }
    }

    public static string GetShortDescription()
    {
        return "Command line for migrating the old version of OssManagement to BlobManagement storage.";
    }

    public virtual string GetUsageInfo()
    {
        var sb = new StringBuilder();

        sb.AppendLine("");
        sb.AppendLine("Usage:");
        sb.AppendLine("");
        sb.AppendLine("  labp blob-migration [options]");
        sb.AppendLine("");
        sb.AppendLine("Warning:");
        sb.AppendLine("");
        sb.AppendLine("This function is only applicable to the OssManagement integration using the local file system(AbpOssManagementFileSystemModule). Other solutions are not supported at present.");
        sb.AppendLine("");
        sb.AppendLine("Options:");
        sb.AppendLine("");
        sb.AppendLine("-wd|--working-directory <directory-path>     (default: Current directory) Execution directory.");
        sb.AppendLine("-so|--search-option <search-option>          (default: top) Search for directory file option (top, all).");
        sb.AppendLine("-sp|--search-pattern <search-pattern>        (default: *.*) Search for directory file pattern. Example: *.jpg");
        sb.AppendLine("-bn|--bucket <bucket-name>                   (default: blobs) Storage container name");
        sb.AppendLine("-pn|--path <path-name>                       Storage file path. The path is parsed from the current file by default.");
        sb.AppendLine("-on|--object <object-name>                   Store file name. The default is to use the current file name.");
        sb.AppendLine("-u |--url <url>                               (default: http://localhost:8080) BlobManagement service URL from.");
        sb.AppendLine("-ls|--list-file                              Only list the files without uploading them. This is for confirming before uploading.");
        sb.AppendLine("-ow|--overwrite                              If this parameter is carried, it will forcibly overwrite the existing file in the target storage.");
        sb.AppendLine("");
        sb.AppendLine("Examples:");
        sb.AppendLine("");
        sb.AppendLine("  labp blob-migration -wd D:\\\\Blobs\\\\ -so top -ls");
        sb.AppendLine("  labp blob-migration -wd D:\\\\Blobs\\\\ -so top -sp *.dll -ls");
        sb.AppendLine("  labp blob-migration -wd D:\\\\Blobs\\\\ -bn users -ls");
        sb.AppendLine("  labp blob-migration -wd D:\\\\Blobs\\\\ -bn users -u https://localhost:8443");
        sb.AppendLine("  labp blob-migration -wd D:\\\\Blobs\\\\ -bn users -pn 8e12e0ce-8534-41d2-af49-da1ff828005a -on a.jpg");
        sb.AppendLine("  labp blob-migration -wd D:\\\\Blobs\\\\ -bn users -pn 8e12e0ce-8534-41d2-af49-da1ff828005a -on a.jpg -ls");
        sb.AppendLine("  labp blob-migration -wd D:\\\\Blobs\\\\ -bn users -pn 8e12e0ce-8534-41d2-af49-da1ff828005a -on a.jpg -u https://localhost:8443");
        sb.AppendLine("  labp blob-migration -wd D:\\\\Blobs\\\\ -bn users -pn 8e12e0ce-8534-41d2-af49-da1ff828005a -on a.jpg -u https://localhost:8443 -t 3a21eaee-36d2-12ad-3f3a-332d7e6e5ac4 -ow");
        sb.AppendLine("");

        return sb.ToString();
    }

    public static class BlobCommandOptions
    {
        public static class WorkingDirectory
        {
            public const string Short = "wd";
            public const string Long = "working-directory";
        }

        public static class SearchOption
        {
            public const string Short = "so";
            public const string Long = "search-option";
        }

        public static class SearchPattern
        {
            public const string Short = "sp";
            public const string Long = "search-pattern";
        }

        public static class ListFile
        {
            public const string Short = "ls";
            public const string Long = "list-file";
        }

        public static class BucketName
        {
            public const string Short = "bn";
            public const string Long = "bucket";
        }

        public static class PathName
        {
            public const string Short = "pn";
            public const string Long = "path";
        }

        public static class ObjectName
        {
            public const string Short = "on";
            public const string Long = "object";
        }

        public static class Tenant
        {
            public const string Short = "t";
            public const string Long = "tenant";
        }

        public static class Url
        {
            public const string Short = "u";
            public const string Long = "url";
        }

        public static class Overwrite
        {
            public const string Short = "ow";
            public const string Long = "overwrite";
        }
    }
}
