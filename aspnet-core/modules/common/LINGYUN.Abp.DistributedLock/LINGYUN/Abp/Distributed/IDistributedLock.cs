using System;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Distributed
{
    /// <summary>
    /// 分布式锁接口
    /// </summary>
    public interface IDistributedLock
    {
        /// <summary>
        /// 分布式锁
        /// 需要手动释放锁
        /// </summary>
        /// <param name="lockKey">锁键名</param>
        /// <param name="lockValue">锁定对象</param>
        /// <param name="lockSecond">锁定时间（秒）</param>
        /// <returns></returns>
        bool Lock(string lockKey, string lockValue, int lockSecond = 30);
        /// <summary>
        /// 分布式锁
        /// using块自动释放锁
        /// </summary>
        /// <param name="lockKey">锁键名</param>
        /// <param name="lockSecond">锁定时间（秒）</param>
        /// <returns></returns>
        IDisposable Lock(string lockKey, int lockSecond = 30);
        /// <summary>
        /// 分布式锁
        /// using块自动释放锁
        /// </summary>
        /// <param name="lockKey">锁键名</param>
        /// <param name="lockSecond">锁定时间（秒）</param>
        /// <returns></returns>
        Task<IDisposable> LockAsync(string lockKey, int lockSecond = 30, CancellationToken token = default(CancellationToken));
        /// <summary>
        /// 分布式锁
        /// 需要手动释放锁
        /// </summary>
        /// <param name="lockKey">锁键名</param>
        /// <param name="lockValue">锁定对象</param>
        /// <param name="lockSecond">锁定时间（秒）</param>
        /// <returns></returns>
        Task<bool> LockAsync(string lockKey, string lockValue, int lockSecond = 30, CancellationToken token = default(CancellationToken));
        /// <summary>
        /// 释放锁资源
        /// </summary>
        /// <param name="lockKey">锁键名</param>
        /// <param name="lockValue">锁定对象</param>
        /// <returns></returns>
        bool Release(string lockKey, string lockValue);
        /// <summary>
        /// 释放锁资源
        /// </summary>
        /// <param name="lockKey">锁键名</param>
        /// <param name="lockValue">锁定对象</param>
        /// <returns></returns>
        Task<bool> ReleaseAsync(string lockKey, string lockValue, CancellationToken token = default(CancellationToken));
    }
}
