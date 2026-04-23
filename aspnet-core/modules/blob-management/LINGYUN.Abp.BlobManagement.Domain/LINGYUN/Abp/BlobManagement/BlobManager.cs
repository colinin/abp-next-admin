using LINGYUN.Abp.BlobManagement.Settings;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.Content;
using Volo.Abp.Domain.Services;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.IO;
using Volo.Abp.Settings;
using Volo.Abp.Specifications;
using Volo.Abp.Threading;

namespace LINGYUN.Abp.BlobManagement;

public class BlobManager : DomainService
{
    private readonly IDistributedCache<BlobDownloadCacheItem> _blobDownloadCache;
    private readonly IDistributedEventBus _distributedEventBus;
    private readonly IMemoryCache _blobFileValidateCache;
    private readonly ISettingProvider _settingProvider;

    private readonly IBlobProvider _blobProvider;
    private readonly IBlobRepository _blobRepository;
    private readonly IBlobContainerRepository _blobContainerRepository;

    private readonly IApplicationInfoAccessor _applicationInfoAccessor;
    private readonly ICancellationTokenProvider _cancellationTokenProvider;
    public BlobManager(
        IBlobProvider blobProvider, 
        IBlobRepository blobRepository,
        IBlobContainerRepository blobContainerRepository,
        ICancellationTokenProvider cancellationTokenProvider,
        IDistributedEventBus distributedEventBus,
        IDistributedCache<BlobDownloadCacheItem> blobDownloadCache,
        IApplicationInfoAccessor applicationInfoAccessor,
        IMemoryCache blobFileValidateCache,
        ISettingProvider settingProvider)
    {
        _blobProvider = blobProvider;
        _blobRepository = blobRepository;
        _blobContainerRepository = blobContainerRepository;
        _cancellationTokenProvider = cancellationTokenProvider;
        _distributedEventBus = distributedEventBus;
        _blobDownloadCache = blobDownloadCache;
        _applicationInfoAccessor = applicationInfoAccessor;
        _blobFileValidateCache = blobFileValidateCache;
        _settingProvider = settingProvider;
    }

    public async virtual Task<BlobContainer> CreateContainerAsync(string name)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name));

        if (await _blobContainerRepository.FindByNameAsync(name) != null)
        {
            throw new BusinessException(
                BlobManagementErrorCodes.Container.NameAlreadyExists,
                $"A blob container named {name} already exists!")
                .WithData("Name", name);
        }

        var blobContainer = new BlobContainer(
            GuidGenerator.Create(),
            name,
            CurrentTenant.Id);

        await _blobProvider.CreateContainerAsync(blobContainer.Name, GetCancellationToken());

        blobContainer.SetProvider(_blobProvider.Name);

        await _blobContainerRepository.InsertAsync(blobContainer);

        return blobContainer;
    }

    public async virtual Task DeleteContainerAsync(BlobContainer blobContainer)
    {
        var getBlobsExistsSpec = new ExpressionSpecification<Blob>(
            x => x.ContainerId == blobContainer.Id);

        var existsBlobsCount = await _blobRepository.GetCountAsync(getBlobsExistsSpec);
        if (existsBlobsCount > 0)
        {
            throw new BusinessException(
                BlobManagementErrorCodes.Container.DeleteWithNotEmpty,
                "The blob container is not empty and cannot be deleted!");
        }

        await _blobProvider.DeleteContainerAsync(blobContainer.Name, GetCancellationToken());

        await _blobContainerRepository.DeleteAsync(blobContainer);
    }

    public async virtual Task<Blob> CreateBlobAsync(
        BlobContainer blobContainer,
        string name,
        BlobType type,
        long? size = null,
        Blob? parentBlob = null,
        bool overrideExisting = false)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name));

        var existsBlob = await _blobRepository.FindByNameAsync(blobContainer.Id, name, parentBlob?.Id);
        if (existsBlob != null)
        {
            if (!overrideExisting)
            {
                throw new BusinessException(
                    BlobManagementErrorCodes.Blob.NameAlreadyExists,
                    $"There is already a file/directory named {name} in the current directory!")
                    .WithData("Name", name);
            }

            await DeleteBlobAsync(existsBlob);
        }
        Blob blob;
        if (parentBlob != null)
        {
            blob = parentBlob.AddBlob(
                GuidGenerator.Create(),
                name,
                type);

            await _blobRepository.UpdateAsync(parentBlob);
        }
        else
        {
            blob = new Blob(
                GuidGenerator.Create(),
                blobContainer.Id,
                name,
                type,
                size);
        }

        if (type == BlobType.Folder)
        {
            await _blobProvider.CreateFolderAsync(blobContainer.Name, blob.FullName, GetCancellationToken());

            blob.SetProvider(_blobProvider.Name);
        }

        await _blobRepository.InsertAsync(blob);

        return blob;
    }

    public async virtual Task DeleteBlobAsync(Blob blob)
    {
        if (blob.Blobs.Count > 0)
        {
            throw new BusinessException(
                BlobManagementErrorCodes.Blob.DeleteWithNotEmpty,
                "The current directory is not empty and cannot be deleted!");
        }

        var blobContainer = await _blobContainerRepository.GetAsync(blob.ContainerId);

        if (blob.ParentId.HasValue)
        {
            var parentBlob = await _blobRepository.GetAsync(blob.ParentId.Value);

            parentBlob.RemoveBlob(blob.Name);

            await _blobRepository.UpdateAsync(parentBlob);
        }

        await _blobRepository.DeleteAsync(blob);

        if (blob.Type == BlobType.Folder)
        {
            await _blobProvider.DeleteFolderAsync(blobContainer.Name, blob.FullName, GetCancellationToken());
        }
        else
        {
            await _blobProvider.DeleteBlobAsync(blobContainer.Name, blob.FullName, GetCancellationToken());
        }
    }

    public async virtual Task<Blob> UploadBlobAsync(
        BlobContainer blobContainer, 
        Blob blob, 
        Stream stream,
        string? contentType = null,
        string? compareMd5 = null)
    {
        await ValidateUploadFileAsync(blob.Name, stream.Length);

        if (!compareMd5.IsNullOrWhiteSpace())
        {
            var blobMd5 = stream.MD5();
            if (!string.Equals(blobMd5, compareMd5, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new BusinessException(
                    BlobManagementErrorCodes.Blob.BlobHashValidFailed,
                    "File fingerprint verification failed. Please check the file's legitimacy!");
            }
            blob.SetContentMd5(blobMd5);
        }

        await _blobProvider.UploadBlobAsync(blobContainer.Name, blob.FullName, stream, GetCancellationToken());

        blob.SetProvider(_blobProvider.Name);
        blob.SetFileInfo(contentType, stream.Length);

        return blob;
    }

    public async virtual Task CreateChunkBlobAsync(
        string containerIdOrName,
        string name,
        IRemoteStreamContent file,
        long chunkSize,
        long currentChunkSize,
        long chunkNumber,
        long totalChunks,
        string? compareMd5 = null,
        Guid? parentId = null)
    {
        await ValidateUploadFileAsync(name, totalChunks * chunkSize);

        var tempFilePath = GetChunkBlobTempPath(containerIdOrName, name, parentId);

        try
        {
            // 创建分片文件缓存目录
            DirectoryHelper.CreateIfNotExists(tempFilePath);

            var cancellationToken = GetCancellationToken();

            // 创建分片文件
            var tempSavedFile = Path.Combine(tempFilePath, $"{chunkNumber}.chunk");
            // 分片传输中断重续时删除原文件
            FileHelper.DeleteIfExists(tempSavedFile);
            // 当前分片文件缓存
            using (var fs = File.OpenWrite(tempSavedFile))
            {
                await file.GetStream().CopyToAsync(fs, cancellationToken);
            }
            // 最后一个分片时合并文件
            if (chunkNumber == totalChunks)
            {
                var mergeTmpFile = Path.Combine(tempFilePath, name);
                FileHelper.DeleteIfExists(mergeTmpFile);
                // 创建临时合并文件流
                using (var mergeFileStream = new FileStream(mergeTmpFile, FileMode.Create, FileAccess.ReadWrite))
                {
                    // 获取并排序所有分片文件
                    var mergeFiles = Directory.GetFiles(tempFilePath, "*.chunk").OrderBy(name => name);
                    // 创建临时合并文件
                    foreach (var mergeFile in mergeFiles)
                    {
                        // 写入到合并文件流
                        await mergeFileStream.WriteAsync(await FileHelper.ReadAllBytesAsync(mergeFile), cancellationToken);
                        // 删除已参与合并的临时文件分片
                        FileHelper.DeleteIfExists(mergeFile);
                    }
                    // 创建Blob对象
                    BlobContainer blobContainer;
                    Blob? parentBlob = null;
                    if (Guid.TryParse(containerIdOrName, out var containerId))
                    {
                        blobContainer = await _blobContainerRepository.GetAsync(containerId);
                    }
                    else
                    {
                        blobContainer = await _blobContainerRepository.GetByNameAsync(containerIdOrName);
                    }
                    if (parentId.HasValue)
                    {
                        parentBlob = await _blobRepository.GetAsync(parentId.Value);
                    }
                    var blob = await CreateBlobAsync(
                        blobContainer,
                        name,
                        BlobType.File,
                        mergeFileStream.Length,
                        parentBlob);

                    // 多分片文件流合并上传
                    mergeFileStream.Seek(0, SeekOrigin.Begin);
                    await UploadBlobAsync(blobContainer, blob, mergeFileStream, compareMd5);
                }
                // 文件保存之后删除临时文件目录
                DirectoryHelper.DeleteIfExists(tempFilePath, true);
            }
        }
        catch
        {
            // 发生异常删除临时文件目录
            DirectoryHelper.DeleteIfExists(tempFilePath, true);
            throw;
        }
    }

    public async virtual Task<Stream?> DownloadBlobsync(Blob blob)
    {
        var containerName = await _blobContainerRepository.GetNameAsync(blob.ContainerId);

        return await DownloadBlobsync(containerName, blob.FullName);
    }

    public virtual string GetBlobProvider()
    {
        return _blobProvider.Name;
    }

    protected async virtual Task<Stream?> DownloadBlobsync(string containerName, string blobName)
    {
        var blobStream = await _blobProvider.DownloadBlobAsync(containerName, blobName, GetCancellationToken());
        if (blobStream != null)
        {
            var downloadCacheKey = BlobDownloadCacheItem.CalculateCacheKey(containerName, blobName);
            var downloadCacheItem = await _blobDownloadCache.GetAsync(downloadCacheKey)
                ?? new BlobDownloadCacheItem(0);
            downloadCacheItem.DownloadCount += 1;
            await _blobDownloadCache.SetAsync(downloadCacheKey, downloadCacheItem);

            await _distributedEventBus.PublishAsync(new BlobDownloadEto(containerName, blobName, CurrentTenant.Id));
        }

        return blobStream;
    }

    protected async virtual Task ValidateUploadFileAsync(string blobName, long blobSize)
    {
        var fileValidateCacheItem = await GetFileValidateCacheItemAsync();
        if (fileValidateCacheItem.SizeLimit * 1024 * 1024 < blobSize)
        {
            throw new BusinessException(
                BlobManagementErrorCodes.Blob.UploadFileSizeTooLong,
                $"Upload file size cannot exceed {fileValidateCacheItem.SizeLimit} MB!")
                .WithData("Limit", fileValidateCacheItem.SizeLimit);
        }
        var fileExtensionName = FileHelper.GetExtension(blobName);
        if (fileExtensionName.IsNullOrWhiteSpace() ||
            !fileValidateCacheItem.AllowedExtensions
                .Any(fe => fileExtensionName.EndsWith(fe, StringComparison.CurrentCultureIgnoreCase)))
        {
            throw new BusinessException(
                BlobManagementErrorCodes.Blob.UploadFileExtendCanNotBeMatch,
                $"Not allowed file extension: {fileExtensionName}!")
                .WithData("Name", fileExtensionName ?? "Unknown");
        }
    }

    protected async virtual Task<BlobUploadFileValidateCacheItem> GetFileValidateCacheItemAsync()
    {
        var fileValidateCacheItem = _blobFileValidateCache.Get<BlobUploadFileValidateCacheItem>(BlobUploadFileValidateCacheItem.CacheKey);
        if (fileValidateCacheItem == null)
        {
            var fileSizeLimited = await _settingProvider
                .GetAsync(
                    BlobManagementSettingNames.FileLimitLength,
                    BlobManagementSettingNames.DefaultFileLimitLength);
            var fileAllowExtensions = await _settingProvider
                .GetOrNullAsync(BlobManagementSettingNames.AllowFileExtensions) ?? BlobManagementSettingNames.DefaultAllowFileExtensions;

            fileValidateCacheItem = new BlobUploadFileValidateCacheItem(fileSizeLimited, fileAllowExtensions.Split(','));

            _blobFileValidateCache.Set(BlobUploadFileValidateCacheItem.CacheKey,
                fileValidateCacheItem,
                new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(2)
                });
        }
        return fileValidateCacheItem;
    }

    protected virtual string GetChunkBlobTempPath(
        string containerName, 
        string blobName,
        Guid? blobParentId = null)
    {
        return Path.Combine(
            Path.GetTempPath(),
            _applicationInfoAccessor.ApplicationName ?? "BlobManagement",
            "chunk_blob_temp",
            string.Concat(
                CurrentTenant.Id?.ToString() ?? "",
                containerName, 
                blobParentId?.ToString() ?? "", 
                blobName).ToMd5());
    }

    protected virtual CancellationToken GetCancellationToken(CancellationToken cancellationToken = default)
    {
        return _cancellationTokenProvider.FallbackToProvider(cancellationToken);
    }
}
