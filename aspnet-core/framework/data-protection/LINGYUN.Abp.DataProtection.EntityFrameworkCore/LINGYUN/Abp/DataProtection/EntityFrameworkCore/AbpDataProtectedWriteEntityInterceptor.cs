using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Authorization;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Users;

namespace LINGYUN.Abp.DataProtection.EntityFrameworkCore;
public class AbpDataProtectedWriteEntityInterceptor : SaveChangesInterceptor, ITransientDependency
{
    public IAbpLazyServiceProvider LazyServiceProvider { get; set; } = default!;
    public IOptions<AbpDataProtectionOptions> DataProtectionOptions => LazyServiceProvider.LazyGetRequiredService<IOptions<AbpDataProtectionOptions>>();
    public ICurrentUser CurrentUser => LazyServiceProvider.LazyGetRequiredService<ICurrentUser>();
    public IDataFilter DataFilter => LazyServiceProvider.LazyGetRequiredService<IDataFilter>();
    public IDataAuthorizationService DataAuthorizationService => LazyServiceProvider.LazyGetRequiredService<IDataAuthorizationService>();

    public async override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, 
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (DataFilter.IsEnabled<IDataProtected>() && eventData.Context != null)
        {
            var updateEntites = eventData.Context.ChangeTracker.Entries()
                .Where(entry => entry.State.IsIn(EntityState.Modified) && (entry.Entity is not ISoftDelete || entry.Entity is ISoftDelete delete && delete.IsDeleted == false))
                .Select(entry => entry.Entity as IEntity);
            if (updateEntites.Any())
            {
                var updateGrant = await DataAuthorizationService.AuthorizeAsync(DataAccessOperation.Write, updateEntites);
                if (!updateGrant.Succeeded)
                {
                    var entityKeys = updateEntites
                        .Select(entity => (entity is IEntity abpEntity ? abpEntity.GetKeys() : new string[1] { entity.ToString() }).ToString())
                        .JoinAsString(";");
                    throw new AbpAuthorizationException(
                        $"Delete data permission not granted to entity {updateEntites.First().GetType()} for data {entityKeys}!");
                }
            }
            
            var deleteEntites = eventData.Context.ChangeTracker.Entries()
                .Where(entry => entry.State.IsIn(EntityState.Deleted) || entry.Entity is ISoftDelete delete && delete.IsDeleted == true)
                .Select(entry => entry.Entity as IEntity);
            if (deleteEntites.Any())
            {
                var deleteGrant = await DataAuthorizationService.AuthorizeAsync(DataAccessOperation.Delete, deleteEntites);
                if (!deleteGrant.Succeeded)
                {
                    var entityKeys = deleteEntites
                        .Select(entity => (entity is IEntity abpEntity ? abpEntity.GetKeys() : new string[1] { entity.ToString() }).ToString())
                        .JoinAsString(";");
                    throw new AbpAuthorizationException(
                        $"Delete data permission not granted to entity {deleteEntites.First().GetType()} for data {entityKeys}!");
                }
            }
        }
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
