using System;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DistributedLocking;
using Volo.Abp.EventBus.Distributed;

namespace LINGYUN.Abp.BlobManagement;

public class BlobDownloadIncreaser :
    IDistributedEventHandler<BlobDownloadEto>,
    ITransientDependency
{
    private readonly IDistributedCache<BlobDownloadCacheItem> _blobDownloadCache;
    private readonly IAbpDistributedLock _distributedLock;

    private readonly IBlobContainerRepository _blobContainerRepository;
    private readonly IBlobRepository _blobRepository;

    public BlobDownloadIncreaser(
        IDistributedCache<BlobDownloadCacheItem> blobDownloadCache,
        IAbpDistributedLock distributedLock,
        IBlobContainerRepository blobContainerRepository,
        IBlobRepository blobRepository)
    {
        _blobDownloadCache = blobDownloadCache;
        _distributedLock = distributedLock;
        _blobContainerRepository = blobContainerRepository;
        _blobRepository = blobRepository;
    }

    public async virtual Task HandleEventAsync(BlobDownloadEto eventData)
    {
        await using var handle = await _distributedLock.TryAcquireAsync($"{nameof(BlobDownloadIncreaser)}_{nameof(BlobDownloadEto)}");
        if (handle == null)
        {
            return;
        }

        var blobContainer = await _blobContainerRepository.FindByNameAsync(eventData.ContainerName);
        if (blobContainer == null)
        {
            return;
        }
        var blob = await _blobRepository.FindByFullNameAsync(eventData.FullName, blobContainer.Id);
        if (blob != null)
        {
            var downloadCacheKey = BlobDownloadCacheItem.CalculateCacheKey(blobContainer.Name, eventData.FullName);
            var downloadCacheItem = await _blobDownloadCache.GetAsync(downloadCacheKey);
            if (downloadCacheItem == null)
            {
                downloadCacheItem = new BlobDownloadCacheItem(blob.DownloadCount);
                await _blobDownloadCache.SetAsync(downloadCacheKey, downloadCacheItem);
            }
            blob.SetDownloadCount(downloadCacheItem.DownloadCount + 1);

            await _blobRepository.UpdateAsync(blob);
        }
    }
}
