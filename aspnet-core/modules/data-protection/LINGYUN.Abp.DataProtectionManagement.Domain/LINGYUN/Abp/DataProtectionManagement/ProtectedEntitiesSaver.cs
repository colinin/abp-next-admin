using LINGYUN.Abp.DataProtection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Guids;
using Volo.Abp.Localization;
using Volo.Abp.Uow;

namespace LINGYUN.Abp.DataProtectionManagement;
public class ProtectedEntitiesSaver : IProtectedEntitiesSaver, ITransientDependency
{
    protected IApplicationInfoAccessor ApplicationInfoAccessor { get; }
    protected AbpDistributedCacheOptions CacheOptions { get; }
    protected AbpDataProtectionOptions Options { get; }
    protected DataProtectionManagementOptions ManagementOptions { get; }
    protected IGuidGenerator GuidGenerator { get; }
    protected IAbpDistributedLock DistributedLock { get; }
    protected IEntityTypeInfoRepository EntityTypeInfoRepository { get; }
    protected ILocalizableStringSerializer LocalizableStringSerializer { get; }

    public ProtectedEntitiesSaver(
        IApplicationInfoAccessor applicationInfoAccessor, 
        IOptions<AbpDistributedCacheOptions> cacheOptions, 
        IOptions<AbpDataProtectionOptions> options, 
        IOptions<DataProtectionManagementOptions> managementOptions,
        IGuidGenerator guidGenerator,
        IAbpDistributedLock distributedLock, 
        IEntityTypeInfoRepository entityTypeInfoRepository,
        ILocalizableStringSerializer localizableStringSerializer)
    {
        ApplicationInfoAccessor = applicationInfoAccessor;
        CacheOptions = cacheOptions.Value;
        Options = options.Value;
        ManagementOptions = managementOptions.Value;
        GuidGenerator = guidGenerator;
        DistributedLock = distributedLock;
        EntityTypeInfoRepository = entityTypeInfoRepository;
        LocalizableStringSerializer = localizableStringSerializer;
    }

    [UnitOfWork]
    public async virtual Task SaveAsync(CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        await using var applicationLockHandle = await DistributedLock.TryAcquireAsync(
            GetApplicationDistributedLockKey()
        );

        if (applicationLockHandle == null)
        {
            return;
        }

        var newRecords = new List<EntityTypeInfo>();
        var changeRecords = new List<EntityTypeInfo>();

        var entityTypeList = await EntityTypeInfoRepository.GetListAsync();
        foreach (var protectedEntityMap in ManagementOptions.ProtectedEntities)
        {
            var resourceType = protectedEntityMap.Key;
            var resourceName = LocalizationResourceNameAttribute.GetName(resourceType);
            foreach (var entityType in protectedEntityMap.Value)
            {
                var entityTypeInfo = entityTypeList.FirstOrDefault(x => x.TypeFullName == entityType.FullName);
                if (entityTypeInfo == null)
                {
                    // TODO: support localization
                    var typeDisplayName = entityType.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName;
                    if (typeDisplayName.IsNullOrWhiteSpace())
                    {
                        var typeDisplayNameString = LocalizableString.Create($"DisplayName:{entityType.Name}", resourceName);
                        typeDisplayName = LocalizableStringSerializer.Serialize(typeDisplayNameString);
                    }
                    var isDataAudited = entityType.GetCustomAttribute<DisableDataProtectedAttribute>() == null;
                    entityTypeInfo = new EntityTypeInfo(
                        GuidGenerator.Create(),
                        entityType.Name,
                        typeDisplayName ?? entityType.Name,
                        entityType.FullName,
                        isDataAudited);

                    var typeProperties = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty)
                        .Where(x => !Options.IgnoreAuditedProperties.Contains(x.Name));
                    foreach (var typeProperty in typeProperties)
                    {
                        var propDisplayName = typeProperty.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName;
                        if (propDisplayName.IsNullOrWhiteSpace())
                        {
                            var propDisplayNameString = LocalizableString.Create($"DisplayName:{entityType.Name}", resourceName);
                            propDisplayName = LocalizableStringSerializer.Serialize(propDisplayNameString);
                        }
                        var propTypeFullName = typeProperty.PropertyType.FullName;
                        string valueRange = null;
                        if (typeProperty.PropertyType.IsEnum)
                        {
                            propTypeFullName = typeof(int).FullName;
                            var enumType = typeProperty.PropertyType;
                            var enumValues = enumType.GetEnumValues().Cast<int>();
                            valueRange = enumValues.JoinAsString(",");
                        }
                        entityTypeInfo.AddProperty(
                            GuidGenerator,
                            typeProperty.Name,
                            propDisplayName ?? typeProperty.Name,
                            typeProperty.PropertyType.FullName,
                            valueRange);
                    }
                    newRecords.Add(entityTypeInfo);
                }
                else
                {
                    var hasChanged = false;
                    var typeProperties = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty)
                        .Where(x => !Options.IgnoreAuditedProperties.Contains(x.Name));
                    entityTypeInfo.Properties.RemoveAll(x => !typeProperties.Any(p => p.Name == x.Name));
                    foreach (var typeProperty in typeProperties)
                    {
                        if (!entityTypeInfo.HasExistsProperty(typeProperty.Name))
                        {
                            hasChanged = true;
                            var propDisplayName = typeProperty.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName;
                            if (propDisplayName.IsNullOrWhiteSpace())
                            {
                                var propDisplayNameString = LocalizableString.Create($"DisplayName:{typeProperty.Name}", resourceName);
                                propDisplayName = LocalizableStringSerializer.Serialize(propDisplayNameString);
                            }
                            var propTypeFullName = typeProperty.PropertyType.FullName;
                            string valueRange = null;
                            if (typeProperty.PropertyType.IsEnum)
                            {
                                propTypeFullName = typeof(int).FullName;
                                var enumType = typeProperty.PropertyType;
                                var enumValues = enumType.GetEnumValues().Cast<int>();
                                valueRange = enumValues.JoinAsString(",");
                            }
                            entityTypeInfo.AddProperty(
                                GuidGenerator,
                                typeProperty.Name,
                                propDisplayName ?? typeProperty.Name,
                                typeProperty.PropertyType.FullName,
                                valueRange);
                        }
                    }
                    if (hasChanged)
                    {
                        changeRecords.Add(entityTypeInfo);
                    }
                }
            }
        }

        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        if (newRecords.Any())
        {
            await EntityTypeInfoRepository.InsertManyAsync(newRecords);
        }

        if (changeRecords.Any())
        {
            await EntityTypeInfoRepository.UpdateManyAsync(changeRecords);
        }
    }

    private string GetApplicationDistributedLockKey()
    {
        return $"{CacheOptions.KeyPrefix}_{ApplicationInfoAccessor.ApplicationName}_AbpDataProtectionUpdateLock";
    }
}
