using LINGYUN.Abp.AuditLogging;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Json;
using Volo.Abp.Reflection;

namespace LINGYUN.Abp.EntityChange;

public abstract class EntityRestoreAppService<TEntity, TKey> : EntityChangeAppService<TEntity>, IEntityRestoreAppService
    where TEntity : class, IEntity<TKey>
{
    protected virtual string RestorePolicy { get; set; }

    protected IRepository<TEntity, TKey> Repository { get; }

    protected IJsonSerializer JsonSerializer => LazyServiceProvider.LazyGetRequiredService<IJsonSerializer>();

    protected EntityRestoreAppService(
        IEntityChangeStore entityChangeStore,
        IRepository<TEntity, TKey> repository)
        : base(entityChangeStore)
    {
        Repository = repository;
    }

    public async virtual Task RestoreEntitesAsync(RestoreEntitiesInput input)
    {
        if (!RestorePolicy.IsNullOrWhiteSpace())
        {
            await AuthorizationService.AuthorizeAsync(RestorePolicy);
        }

        foreach (var restoreEntity in input.Entities)
        {
            await RestoreEntityByAuditLogAsync(restoreEntity);
        }

        await CurrentUnitOfWork.SaveChangesAsync();
    }

    public async virtual Task RestoreEntityAsync(RestoreEntityInput input)
    {
        if (!RestorePolicy.IsNullOrWhiteSpace())
        {
            await AuthorizationService.AuthorizeAsync(RestorePolicy);
        }

        await RestoreEntityByAuditLogAsync(input);

        await CurrentUnitOfWork.SaveChangesAsync();
    }

    protected virtual TKey MapToEntityKey(string entityId)
    {
        return (TKey)Convert.ChangeType(entityId, typeof(TKey));
    }

    protected async virtual Task RestoreEntityByAuditLogAsync(RestoreEntityInput input)
    {
        var entityChanges = await EntityChangeStore.GetListAsync(
            entityId: input.EntityId,
            entityTypeFullName: typeof(TEntity).FullName);
        var entityKey = MapToEntityKey(input.EntityId);

        if (entityChanges != null && entityChanges.Any())
        {
            var entity = await Repository.GetAsync(entityKey);
            var entityProperties = typeof(TEntity).GetProperties();
            var entityChange = entityChanges
                .WhereIf(input.EntityChangeId.HasValue, x => x.Id == input.EntityChangeId)
                .OrderByDescending(x => x.ChangeTime)
                .First();

            foreach (var propertyChange in entityChange.PropertyChanges)
            {
                var propertyName = propertyChange.PropertyName;
                var entityProperty = entityProperties.FirstOrDefault(x => x.Name == propertyName && x.GetSetMethod(true) != null);
                if (entityProperty != null)
                {
                    if (TypeHelper.IsPrimitiveExtended(entityProperty.PropertyType, includeEnums: true))
                    {
                        var conversionType = entityProperty.PropertyType;
                        var isNullableType = TypeHelper.IsNullable(conversionType);
                        if (isNullableType)
                        {
                            conversionType = conversionType.GetFirstGenericArgumentIfNullable();
                        }

                        var currentValue = propertyChange.OriginalValue;
                        if (currentValue.IsNullOrWhiteSpace() || string.Equals("null", currentValue, StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (isNullableType)
                            {
                                entityProperty.SetValue(entity, null);
                            }
                            continue;
                        }

                        var setPropertyValue = JsonSerializer.Deserialize(conversionType, currentValue);
                        entityProperty.SetValue(entity, setPropertyValue);
                    }
                }
            }

            await Repository.UpdateAsync(entity);
        }
    }
}
