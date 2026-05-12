using LINGYUN.Abp.BlobManagement.Dtos;
using LINGYUN.Abp.BlobManagement.Features;
using LINGYUN.Abp.BlobManagement.Integration.Dtos;
using LINGYUN.Abp.Features.LimitValidation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Volo.Abp;
using Volo.Abp.Content;
using Volo.Abp.Domain.ChangeTracking;
using Volo.Abp.Features;

namespace LINGYUN.Abp.BlobManagement.Integration;

[RequiresFeature(BlobManagementFeatureNames.Blob.Enable)]
public class BlobIntegrationService : BlobAppServiceBase, IBlobIntegrationService
{
    public BlobIntegrationService(
        IBlobContainerRepository blobContainerRepository,
        IBlobRepository blobRepository,
        BlobManager blobManager) 
        : base(blobContainerRepository, blobRepository, blobManager)
    {
    }

    [RequiresFeature(BlobManagementFeatureNames.Blob.UploadFile)]
    [RequiresLimitFeature(
        BlobManagementFeatureNames.Blob.UploadLimit,
        BlobManagementFeatureNames.Blob.UploadInterval,
        LimitPolicy.Month)]
    public async virtual Task<BlobDto> CreateAsync(BlobFileCreateIntegrationDto input)
    {
        var blobContainer = await BlobContainerRepository.GetByNameAsync(input.ContainerName);

        // Example: /a/b/c/d/f.txt
        var blobName = HttpUtility.UrlDecode(input.BlobName);
        var blobPath = blobName;
        var pathIndex = blobName.LastIndexOf('/');
        if (pathIndex > 0)
        {
            // /a/b/c/d
            blobPath = blobName.Substring(0, pathIndex);
            // f.txt
            blobName = blobName.Substring(pathIndex + 1);
        }

        // New: ["/a/b/c/d", "/a/b/c", "/a/b", "/a"]
        // Return: "/a/b/c/d"
        var parentBlob = await GetOrCreateParentBlobAsync(blobContainer, blobPath);

        var blobStream = input.File.GetStream();

        var blob = await BlobManager.CreateBlobAsync(
            blobContainer,
            blobName,
            BlobType.File,
            blobStream.Length,
            parentBlob,
            input.Overwrite == true);

        await BlobManager.UploadBlobAsync(blobContainer, blob, blobStream, input.File.ContentType);

        return ObjectMapper.Map<Blob, BlobDto>(blob);
    }

    public async virtual Task DeleteAsync(BlobFileGetByNameIntegrationDto input)
    {
        var blobContainer = await BlobContainerRepository.GetByNameAsync(input.ContainerName);

        var blob = await FindBlobByNameAsync(blobContainer, input.BlobName)
            ?? throw new BusinessException(
                BlobManagementErrorCodes.Blob.NameNotFound,
                $"There is no file/directory named {input.BlobName}!")
                .WithData("Name", input.BlobName);

        await CheckDeletePolicyAsync(blobContainer.Name, blob);

        await DeleteBlobAsync(blob);
    }

    [DisableEntityChangeTracking]
    public async virtual Task<bool> ExistsAsync(BlobFileGetByNameIntegrationDto input)
    {
        try
        {
            return await BlobRepository.ExistsAsync(input.ContainerName, input.BlobName);
        }
        catch (Exception ex)
        {
            Logger.LogWarning(ex, "Failed to load the Blob: {message}", ex.Message);
            return false;
        }
    }

    [RequiresFeature(BlobManagementFeatureNames.Blob.DownloadFile)]
    [RequiresLimitFeature(
        BlobManagementFeatureNames.Blob.DownloadLimit,
        BlobManagementFeatureNames.Blob.DownloadInterval,
        LimitPolicy.Month)]
    public async virtual Task<IRemoteStreamContent> GetContentAsync(BlobFileGetByNameIntegrationDto input)
    {
        try
        {
            var blobContainer = await BlobContainerRepository.GetByNameAsync(input.ContainerName);
            return await GetContentByNameAsync(blobContainer, input.BlobName);
        }
        catch (Exception ex)
        {
            Logger.LogWarning(ex, "Failed to load the Blob: {message}", ex.Message);

            return new RemoteStreamContent(Stream.Null);
        }
    }

    protected async virtual Task<Blob> GetOrCreateParentBlobAsync(BlobContainer blobContainer, string blobPath)
    {
        var normalizedPath = blobPath.EnsureStartsWith('/').TrimEnd('/');

        var allPaths = GetAllPathLevels(normalizedPath);

        var existingFolders = await BlobRepository.GetFolderListAsync(allPaths, blobContainer.Id);

        var allFolders = await CreateMissingFoldersAsync(
            blobContainer,
            allPaths,
            existingFolders
        );

        return allFolders.Last();
    }

    private async Task<List<Blob>> CreateMissingFoldersAsync(
        BlobContainer blobContainer,
        List<string> allPaths,
        List<Blob> existingFolders)
    {
        var existingDict = existingFolders.ToDictionary(x => x.FullName, x => x);
        var result = new List<Blob>();
        var needInsert = new List<Blob>();

        for (var i = 0; i < allPaths.Count; i++)
        {
            var currentPath = allPaths[i];

            if (existingDict.TryGetValue(currentPath, out var existingFolder))
            {
                result.Add(existingFolder);
            }
            else
            {
                var folderName = GetFolderNameFromPath(currentPath);
                var parentPath = i > 0 ? allPaths[i - 1] : "/";
                var parentFolder = i > 0 ? result[i - 1] : null;

                var newFolder = await BlobManager.CreateBlobAsync(
                    blobContainer,
                    folderName,
                    BlobType.Folder,
                    parentBlob: parentFolder);

                newFolder.SetFullName(currentPath);
                newFolder.SetProvider(blobContainer.Provider);

                needInsert.Add(newFolder);
                result.Add(newFolder);

                existingDict[currentPath] = newFolder;
            }
        }

        if (needInsert.Count > 0)
        {
            await BlobRepository.InsertManyAsync(needInsert, autoSave: true);
        }

        return result;
    }

    private static List<string> GetAllPathLevels(string path)
    {
        var levels = new List<string>();
        var normalizedPath = path.EnsureStartsWith('/').TrimEnd('/');

        if (normalizedPath.IsNullOrEmpty())
        {
            return levels;
        }

        var segments = normalizedPath.Split('/', StringSplitOptions.RemoveEmptyEntries);
        var currentPath = "/";

        foreach (var segment in segments)
        {
            currentPath = currentPath.EnsureEndsWith('/') + segment;
            levels.Add(currentPath);
        }

        return levels;
    }

    private static string GetFolderNameFromPath(string path)
    {
        var normalizedPath = path.EnsureEndsWith('/').TrimEnd('/');
        var lastSlashIndex = normalizedPath.LastIndexOf('/');

        if (lastSlashIndex < 0 || lastSlashIndex == normalizedPath.Length - 1)
        {
            return normalizedPath;
        }

        return normalizedPath.Substring(lastSlashIndex + 1);
    }
}
