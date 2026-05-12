using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.FileSystem;
using Volo.Abp.IO;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.BlobManagement.FileSystem;

public class FileSystemBlobProvider : IBlobProvider
{
    public const string ProviderName = "FileSystem";
    public string Name => ProviderName;

    protected ICurrentTenant CurrentTenant { get; }
    protected FileSystemBlobNamingNormalizer BlobNamingNormalizer { get; }
    protected IBlobContainerConfigurationProvider ConfigurationProvider { get; }

    public FileSystemBlobProvider(
        ICurrentTenant currentTenant,
        FileSystemBlobNamingNormalizer blobNamingNormalizer,
        IBlobContainerConfigurationProvider configurationProvider)
    {
        CurrentTenant = currentTenant;
        BlobNamingNormalizer = blobNamingNormalizer;
        ConfigurationProvider = configurationProvider;
    }

    public virtual Task CreateContainerAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        var filePath = CalculateBlobName(NormalizeContainerName(name));

        DirectoryHelper.CreateIfNotExists(filePath);

        return Task.CompletedTask;
    }

    public virtual Task DeleteContainerAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        var filePath = CalculateBlobName(NormalizeContainerName(name));

        DirectoryHelper.DeleteIfExists(filePath);

        return Task.CompletedTask;
    }

    public virtual Task DeleteBlobAsync(
        string containerName,
        string blobName,
        CancellationToken cancellationToken = default)
    {
        var fileName = CalculateBlobName(NormalizeContainerName(containerName.RemovePostFix("/")), blobName);

        FileHelper.DeleteIfExists(fileName);

        return Task.CompletedTask;
    }

    public virtual Task DeleteFolderAsync(
        string containerName,
        string blobName,
        CancellationToken cancellationToken = default)
    {
        var filePath = CalculateBlobName(NormalizeContainerName(containerName.RemovePostFix("/")), blobName);

        var children = Directory.GetFileSystemEntries(filePath);
        if (children.Length > 0)
        {
            throw new BusinessException(
                BlobManagementErrorCodes.Blob.DeleteWithNotEmpty,
                "The current directory is not empty and cannot be deleted!");
        }

        DirectoryHelper.DeleteIfExists(filePath);

        return Task.CompletedTask;
    }

    public virtual Task<Stream?> DownloadBlobAsync(
        string containerName,
        string blobName,
        CancellationToken cancellationToken = default)
    {
        var fileName = CalculateBlobName(NormalizeContainerName(containerName.RemovePostFix("/")), blobName);

        if (!File.Exists(fileName))
        {
            return Task.FromResult<Stream?>(null);
        }

        return Task.FromResult<Stream?>(File.OpenRead(fileName));
    }

    public virtual Task CreateFolderAsync(
        string containerName, 
        string blobName,
        CancellationToken cancellationToken = default)
    {
        var filePath = CalculateBlobName(NormalizeContainerName(containerName.RemovePostFix("/")), blobName);

        DirectoryHelper.CreateIfNotExists(filePath);

        return Task.CompletedTask;
    }

    public async virtual Task UploadBlobAsync(
        string containerName, 
        string blobName, 
        Stream content, 
        CancellationToken cancellationToken = default)
    {
        var fileName = CalculateBlobName(NormalizeContainerName(containerName.RemovePostFix("/")), blobName);

        DirectoryHelper.CreateIfNotExists(Path.GetDirectoryName(fileName)!);

        using var fileStream = File.Open(fileName, FileMode.Create, FileAccess.ReadWrite);

        await content.CopyToAsync(fileStream, cancellationToken);

        await fileStream.FlushAsync(cancellationToken);
    }

    protected virtual FileSystemBlobProviderConfiguration GetBlobConfiguration()
    {
        var configuration = ConfigurationProvider.Get<BlobManagementContainer>();
        var blobConfiguration = configuration.GetFileSystemConfiguration();
        return blobConfiguration;
    }

    protected virtual string NormalizeContainerName(string containerName)
    {
        if (!containerName.StartsWith("abp-"))
        {
            containerName = "abp-" + containerName;
        }
        return BlobNamingNormalizer.NormalizeContainerName(containerName);
    }

    protected virtual string CalculateBlobName(string bucketName, string blobName = "")
    {
        var blobConfiguration = GetBlobConfiguration();
        var blobPath = blobConfiguration.BasePath;

        if (CurrentTenant.Id == null)
        {
            blobPath = Path.Combine(blobPath, "host");
        }
        else
        {
            blobPath = Path.Combine(blobPath, "tenants", CurrentTenant.Id.Value.ToString("D"));
        }
        DirectoryHelper.CreateIfNotExists(blobPath);

        if (blobConfiguration.AppendContainerNameToBasePath &&
            !bucketName.IsNullOrWhiteSpace())
        {
            blobPath = Path.Combine(blobPath, bucketName);
        }
        if (!blobName.IsNullOrWhiteSpace())
        {
            blobPath = Path.Combine(blobPath, blobName.RemovePreFix("/"));
        }

        return blobPath;
    }
}
