using LINGYUN.Abp.AI;
using LINGYUN.Abp.AI.Workspaces;
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

namespace LINGYUN.Abp.AIManagement.Workspaces;
public class StaticWorkspaceSaver : IStaticWorkspaceSaver, ITransientDependency
{
    protected IStaticWorkspaceDefinitionStore StaticStore { get; }
    protected IWorkspaceRepository WorkspaceRepository { get; }
    protected IWorkspaceDefinitionSerializer WorkspaceSerializer { get; }
    protected IDistributedCache Cache { get; }
    protected IApplicationInfoAccessor ApplicationInfoAccessor { get; }
    protected IAbpDistributedLock DistributedLock { get; }
    protected AbpAICoreOptions AIOptions { get; }
    protected ICancellationTokenProvider CancellationTokenProvider { get; }
    protected AbpDistributedCacheOptions CacheOptions { get; }
    protected IUnitOfWorkManager UnitOfWorkManager { get; }
    protected IGuidGenerator GuidGenerator { get; }

    public StaticWorkspaceSaver(
        IStaticWorkspaceDefinitionStore staticStore,
        IWorkspaceRepository workspaceRepository,
        IWorkspaceDefinitionSerializer workspaceSerializer,
        IDistributedCache cache,
        IOptions<AbpDistributedCacheOptions> cacheOptions,
        IApplicationInfoAccessor applicationInfoAccessor,
        IAbpDistributedLock distributedLock,
        IOptions<AbpAICoreOptions> settingOptions,
        ICancellationTokenProvider cancellationTokenProvider,
        IUnitOfWorkManager unitOfWorkManager,
        IGuidGenerator guidGenerator)
    {
        StaticStore = staticStore;
        WorkspaceRepository = workspaceRepository;
        WorkspaceSerializer = workspaceSerializer;
        Cache = cache;
        ApplicationInfoAccessor = applicationInfoAccessor;
        DistributedLock = distributedLock;
        CancellationTokenProvider = cancellationTokenProvider;
        AIOptions = settingOptions.Value;
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

        var workspaces = await WorkspaceSerializer.SerializeAsync(await StaticStore.GetAllAsync());
        var currentHash = CalculateHash(workspaces, AIOptions.DeletedWorkspaces);

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
                throw new AbpException("Could not acquire distributed lock for saving static Workspaces!");
            }

            using (var unitOfWork = UnitOfWorkManager.Begin(requiresNew: true, isTransactional: true))
            {
                try
                {
                    var hasChangesInWorkspaces = await UpdateChangedWorkspacesAsync(workspaces);

                    if (hasChangesInWorkspaces)
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

    private async Task<bool> UpdateChangedWorkspacesAsync(Workspace[] workspaces)
    {
        var newRecords = new List<Workspace>();
        var changedRecords = new List<Workspace>();

        var workspaceRecordsInDatabase = (await WorkspaceRepository.GetListAsync()).ToDictionary(x => x.Name);

        foreach (var record in workspaces)
        {
            var workspaceRecordInDatabase = workspaceRecordsInDatabase.GetOrDefault(record.Name);
            if (workspaceRecordInDatabase == null)
            {
                /* New group */
                newRecords.Add(record);
                continue;
            }

            if (record.HasSameData(workspaceRecordInDatabase))
            {
                /* Not changed */
                continue;
            }

            /* Changed */
            workspaceRecordInDatabase.Patch(record);
            changedRecords.Add(workspaceRecordInDatabase);
        }

        /* Deleted */
        var deletedRecords = new List<Workspace>();

        if (AIOptions.DeletedWorkspaces.Any())
        {
            deletedRecords.AddRange(workspaceRecordsInDatabase.Values.Where(x => AIOptions.DeletedWorkspaces.Contains(x.Name)));
        }

        if (newRecords.Any())
        {
            await WorkspaceRepository.InsertManyAsync(newRecords);
        }

        if (changedRecords.Any())
        {
            await WorkspaceRepository.UpdateManyAsync(changedRecords);
        }

        if (deletedRecords.Any())
        {
            await WorkspaceRepository.DeleteManyAsync(deletedRecords);
        }

        return newRecords.Any() || changedRecords.Any() || deletedRecords.Any();
    }

    private string GetApplicationDistributedLockKey()
    {
        return $"{CacheOptions.KeyPrefix}_{ApplicationInfoAccessor.ApplicationName}_AbpWorkspaceUpdateLock";
    }

    private string GetCommonDistributedLockKey()
    {
        return $"{CacheOptions.KeyPrefix}_Common_AbpWorkspaceUpdateLock";
    }

    private string GetApplicationHashCacheKey()
    {
        return $"{CacheOptions.KeyPrefix}_{ApplicationInfoAccessor.ApplicationName}_AbpWorkspacesHash";
    }

    private string GetCommonStampCacheKey()
    {
        return $"{CacheOptions.KeyPrefix}_AbpInMemoryWorkspaceCacheStamp";
    }

    private string CalculateHash(Workspace[] workspaces, IEnumerable<string> deletedWorkspaces)
    {
        var jsonSerializerOptions = new JsonSerializerOptions
        {
            TypeInfoResolver = new DefaultJsonTypeInfoResolver
            {
                Modifiers =
                {
                    new AbpIgnorePropertiesModifiers<Workspace, Guid>().CreateModifyAction(x => x.Id),
                }
            }
        };

        var stringBuilder = new StringBuilder();

        stringBuilder.Append("Workspaces:");
        stringBuilder.AppendLine(JsonSerializer.Serialize(workspaces, jsonSerializerOptions));

        stringBuilder.Append("DeletedWorkspace:");
        stringBuilder.Append(deletedWorkspaces.JoinAsString(","));

        return stringBuilder
            .ToString()
            .ToMd5();
    }
}
