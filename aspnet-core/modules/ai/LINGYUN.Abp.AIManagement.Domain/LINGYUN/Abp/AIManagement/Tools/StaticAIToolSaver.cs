using LINGYUN.Abp.AI.Tools;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Guids;
using Volo.Abp.Json.SystemTextJson.Modifiers;
using Volo.Abp.Threading;
using Volo.Abp.Uow;

namespace LINGYUN.Abp.AIManagement.Tools;
public class StaticAIToolSaver : IStaticAIToolSaver, ITransientDependency
{
    protected IStaticAIToolDefinitionStore StaticStore { get; }
    protected IAIToolDefinitionRecordRepository AIToolDefinitionRecordRepository { get; }
    protected IAIToolDefinitionSerializer AIToolDefinitionSerializer { get; }
    protected IDistributedCache Cache { get; }
    protected IApplicationInfoAccessor ApplicationInfoAccessor { get; }
    protected IAbpDistributedLock DistributedLock { get; }
    protected AbpAIToolsOptions AIToolOptions { get; }
    protected ICancellationTokenProvider CancellationTokenProvider { get; }
    protected AbpDistributedCacheOptions CacheOptions { get; }
    protected IUnitOfWorkManager UnitOfWorkManager { get; }
    protected IGuidGenerator GuidGenerator { get; }

    public StaticAIToolSaver(
        IStaticAIToolDefinitionStore staticStore,
        IAIToolDefinitionRecordRepository aiToolDefinitionRecordRepository,
        IAIToolDefinitionSerializer aiToolDefinitionSerializer,
        IDistributedCache cache,
        IOptions<AbpDistributedCacheOptions> cacheOptions,
        IApplicationInfoAccessor applicationInfoAccessor,
        IAbpDistributedLock distributedLock,
        IOptions<AbpAIToolsOptions> aiToolOptions,
        ICancellationTokenProvider cancellationTokenProvider,
        IUnitOfWorkManager unitOfWorkManager,
        IGuidGenerator guidGenerator)
    {
        StaticStore = staticStore;
        AIToolDefinitionRecordRepository = aiToolDefinitionRecordRepository;
        AIToolDefinitionSerializer = aiToolDefinitionSerializer;
        Cache = cache;
        ApplicationInfoAccessor = applicationInfoAccessor;
        DistributedLock = distributedLock;
        CancellationTokenProvider = cancellationTokenProvider;
        AIToolOptions = aiToolOptions.Value;
        CacheOptions = cacheOptions.Value;
        UnitOfWorkManager = unitOfWorkManager;
        GuidGenerator = guidGenerator;
    }

    [UnitOfWork]
    public async Task SaveAsync()
    {
        await using var applicationLockHandle = await DistributedLock.TryAcquireAsync(
            GetApplicationDistributedLockKey()
        );

        if (applicationLockHandle == null)
        {
            return;
        }

        var cacheKey = GetApplicationHashCacheKey();
        var cachedHash = await Cache.GetStringAsync(cacheKey, CancellationTokenProvider.Token);

        var aiTools = await AIToolDefinitionSerializer.SerializeAsync(await StaticStore.GetAllAsync());
        var currentHash = CalculateHash(aiTools, AIToolOptions.DeletedAITools);

        if (cachedHash == currentHash)
        {
            return;
        }

        await using (var commonLockHandle = await DistributedLock.TryAcquireAsync(
                         GetCommonDistributedLockKey(),
                         TimeSpan.FromMinutes(5)))
        {
            if (commonLockHandle == null)
            {
                /* It will re-try */
                throw new AbpException("Could not acquire distributed lock for saving static AITool!");
            }

            using (var unitOfWork = UnitOfWorkManager.Begin(requiresNew: true, isTransactional: true))
            {
                try
                {
                    var hasChangesInAITools = await UpdateChangedAIToolsAsync(aiTools);

                    if (hasChangesInAITools)
                    {
                        await Cache.SetStringAsync(
                            GetCommonStampCacheKey(),
                            Guid.NewGuid().ToString(),
                            new DistributedCacheEntryOptions
                            {
                                SlidingExpiration = TimeSpan.FromDays(30)
                            },
                            CancellationTokenProvider.Token
                        );
                    }
                }
                catch
                {
                    try
                    {
                        await unitOfWork.RollbackAsync();
                    }
                    catch
                    {
                        /* ignored */
                    }

                    throw;
                }

                await unitOfWork.CompleteAsync();
            }
        }

        await Cache.SetStringAsync(
            cacheKey,
            currentHash,
            new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromDays(30)
            },
            CancellationTokenProvider.Token
        );
    }

    private async Task<bool> UpdateChangedAIToolsAsync(AIToolDefinitionRecord[] aiToolRecords)
    {
        var newRecords = new List<AIToolDefinitionRecord>();
        var changedRecords = new List<AIToolDefinitionRecord>();

        var aiToolRecordsInDatabase = (await AIToolDefinitionRecordRepository.GetListAsync()).ToDictionary(x => x.Name);

        foreach (var record in aiToolRecords)
        {
            var aiToolRecordInDatabase = aiToolRecordsInDatabase.GetOrDefault(record.Name);
            if (aiToolRecordInDatabase == null)
            {
                /* New group */
                newRecords.Add(record);
                continue;
            }

            if (record.HasSameData(aiToolRecordInDatabase))
            {
                /* Not changed */
                continue;
            }

            /* Changed */
            aiToolRecordInDatabase.Patch(record);
            changedRecords.Add(aiToolRecordInDatabase);
        }

        /* Deleted */
        var deletedRecords = new List<AIToolDefinitionRecord>();

        if (AIToolOptions.DeletedAITools.Any())
        {
            deletedRecords.AddRange(aiToolRecordsInDatabase.Values.Where(x => AIToolOptions.DeletedAITools.Contains(x.Name)));
        }

        if (newRecords.Any())
        {
            await AIToolDefinitionRecordRepository.InsertManyAsync(newRecords);
        }

        if (changedRecords.Any())
        {
            await AIToolDefinitionRecordRepository.UpdateManyAsync(changedRecords);
        }

        if (deletedRecords.Any())
        {
            await AIToolDefinitionRecordRepository.DeleteManyAsync(deletedRecords);
        }

        return newRecords.Any() || changedRecords.Any() || deletedRecords.Any();
    }

    private string GetApplicationDistributedLockKey()
    {
        return $"{CacheOptions.KeyPrefix}_{ApplicationInfoAccessor.ApplicationName}_AbpAIToolUpdateLock";
    }

    private string GetCommonDistributedLockKey()
    {
        return $"{CacheOptions.KeyPrefix}_Common_AbpAIToolUpdateLock";
    }

    private string GetApplicationHashCacheKey()
    {
        return $"{CacheOptions.KeyPrefix}_{ApplicationInfoAccessor.ApplicationName}_AbpAIToolsHash";
    }

    private string GetCommonStampCacheKey()
    {
        return $"{CacheOptions.KeyPrefix}_AbpInMemoryAIToolCacheStamp";
    }

    private string CalculateHash(AIToolDefinitionRecord[] aiToolRecords, IEnumerable<string> deletedAITool)
    {
        var jsonSerializerOptions = new JsonSerializerOptions
        {
            TypeInfoResolver = new DefaultJsonTypeInfoResolver
            {
                Modifiers =
                {
                    new AbpIgnorePropertiesModifiers<AIToolDefinitionRecord, Guid>().CreateModifyAction(x => x.Id),
                }
            }
        };

        var stringBuilder = new StringBuilder();

        stringBuilder.Append("AITools:");
        stringBuilder.AppendLine(JsonSerializer.Serialize(aiToolRecords, jsonSerializerOptions));

        stringBuilder.Append("DeletedAITool:");
        stringBuilder.Append(deletedAITool.JoinAsString(","));

        return stringBuilder
            .ToString()
            .ToMd5();
    }
}
