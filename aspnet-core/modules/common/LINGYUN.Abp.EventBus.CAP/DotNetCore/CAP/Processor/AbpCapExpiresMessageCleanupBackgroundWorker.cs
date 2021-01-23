using DotNetCore.CAP.Persistence;
using LINGYUN.Abp.EventBus.CAP;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Threading;

namespace DotNetCore.CAP.Processor
{
    /// <summary>
    /// 过期消息清理任务
    /// </summary>
    public class AbpCapExpiresMessageCleanupBackgroundWorker : AsyncPeriodicBackgroundWorkerBase
    {
        /// <summary>
        /// 过期消息清理配置
        /// </summary>
        protected AbpCAPEventBusOptions Options { get; }
        /// <summary>
        /// Initializer
        /// </summary>
        protected IStorageInitializer Initializer { get; }
        /// <summary>
        /// Storage
        /// </summary>
        protected IDataStorage Storage{ get; }
        /// <summary>
        /// 创建过期消息清理任务
        /// </summary>
        /// <param name="timer"></param>
        /// <param name="storage"></param>
        /// <param name="initializer"></param>
        /// <param name="options"></param>
        /// <param name="serviceScopeFactory"></param>
        public AbpCapExpiresMessageCleanupBackgroundWorker(
            AbpAsyncTimer timer,
            IDataStorage storage,
            IStorageInitializer initializer,
            IOptions<AbpCAPEventBusOptions> options,
            IServiceScopeFactory serviceScopeFactory) 
            : base(timer, serviceScopeFactory)
        {
            Storage = storage;
            Options = options.Value;
            Initializer = initializer;

            timer.Period = Options.CleanUpExpiresMessageInterval;
        }

        /// <summary>
        /// 异步执行任务
        /// </summary>
        /// <param name="workerContext"></param>
        /// <returns></returns>
        protected override async Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
        {
            var tables = new[]
            {
                Initializer.GetPublishedTableName(),
                Initializer.GetReceivedTableName()
            };

            foreach (var table in tables)
            {
                Logger.LogDebug($"Collecting expired data from table: {table}");
                var time = DateTime.Now;
                await Storage.DeleteExpiresAsync(table, time, Options.CleanUpExpiresMessageBatch);
            }
        }
    }
}
