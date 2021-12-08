using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DistributedLocking;
using WorkflowCore.Interface;

namespace LINGYUN.Abp.WorkflowCore.DistributedLock
{
    public class AbpDistributedLockProvider : IDistributedLockProvider
    {
        private readonly IMemoryCache _lockCache;
        private readonly IAbpDistributedLock _distributedLock;

        private readonly TimeSpan _lockTimeout = TimeSpan.FromMinutes(1);

        public AbpDistributedLockProvider(
            IMemoryCache memoryCache,
            IAbpDistributedLock abpDistributedLock)
        {
            _lockCache = memoryCache;
            _distributedLock = abpDistributedLock;
        }

        public virtual async Task<bool> AcquireLock(string Id, CancellationToken cancellationToken)
        {
            var handle = await _distributedLock.TryAcquireAsync(Id, _lockTimeout, cancellationToken);
            if (handle == null)
            {
                return false;
            }

            var cacheItem = new LockCacheItem(Id, handle);
            // 预留一点时间
            _lockCache.Set(Id, cacheItem, TimeSpan.FromMinutes(1.5d));

            return true;
        }

        public virtual async Task ReleaseLock(string Id)
        {
            var cacheItem = _lockCache.Get<LockCacheItem>(Id);
            if (cacheItem == null)
            {
                await cacheItem.Handle.DisposeAsync();
            }
        }

        public Task Start()
        {
            return Task.CompletedTask;
        }

        public Task Stop()
        {
            return Task.CompletedTask;
        }

        private class LockCacheItem
        {
            public string Id { get; set; }
            public IAbpDistributedLockHandle Handle { get; set; }
            public LockCacheItem() { }
            public LockCacheItem(
                string id,
                IAbpDistributedLockHandle handle)
            {
                Id = id;
                Handle = handle;
            }
        }
    }
}
