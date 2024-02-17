using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;
using Volo.Abp.Users;

namespace LINGYUN.Abp.DataProtection.EntityFrameworkCore
{
    public class AbpDataProtectionDbContext : AbpDbContext<AbpDataProtectionDbContext>
    {
        protected ICurrentUser CurrentUser => LazyServiceProvider.LazyGetService<ICurrentUser>();

        protected virtual bool IsDataAccessFilterEnabled => DataFilter?.IsEnabled<IHasDataAccess>() ?? false;

        public AbpDataProtectionDbContext(
            DbContextOptions<AbpDataProtectionDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(IHasDataAccess).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .OwnsOne(entityType.ClrType.FullName, nameof(IHasDataAccess.Owner), ownedNavigationBuilder =>
                        {
                            ownedNavigationBuilder.ToJson();
                        });
                }
            }
        }

        protected override void ApplyAbpConceptsForAddedEntity(EntityEntry entry)
        {
            base.ApplyAbpConceptsForAddedEntity(entry);
            if (CurrentUser.IsAuthenticated)
            {
                if (entry is IHasDataAccess entity)
                {
                    ProtectedEntityHelper.TrySetOwner(
                        entity,
                        () => new DataAccessOwner(
                                CurrentUser.Id,
                                CurrentUser.Roles,
                                CurrentUser.FindOrganizationUnits().Select(ou => ou.ToString()).ToArray()));
                }
            }
        }

        protected virtual DataAccessRuleInfo AccessRuleInfo => UnitOfWorkManager.Current.GetAccessRuleInfo();

        protected override void HandlePropertiesBeforeSave()
        {
            foreach (var item in ChangeTracker.Entries().ToList())
            {
                HandleExtraPropertiesOnSave(item);
                HandleCheckPropertiesOnSave(item);
                if (item.State.IsIn(EntityState.Modified, EntityState.Deleted))
                {
                    UpdateConcurrencyStamp(item);
                }
            }
        }

        protected virtual void HandleCheckPropertiesOnSave(EntityEntry entry)
        {
            // 仅当启用过滤器时检查
            if (IsDataAccessFilterEnabled)
            {
                var entityAccessRules = AccessRuleInfo?.Rules.Where(r => r.EntityTypeFullName == entry.Metadata.ClrType.FullName);
                if (entityAccessRules != null)
                {
                    if (entry.State.IsIn(EntityState.Modified, EntityState.Added, EntityState.Deleted))
                    {
                        var entityAccessRule = entityAccessRules.FirstOrDefault(r => r.Operation.IsIn(DataAccessOperation.Write, DataAccessOperation.Delete));
                        if (entityAccessRule != null)
                        {
                            if (entityAccessRule.Fileds.Count != 0)
                            {
                                var notAccessProps = entry.Properties.Where(p => !entityAccessRule.Fileds.Any(f => f.Field == p.Metadata.Name));
                                if (notAccessProps != null)
                                {
                                    foreach (var property in notAccessProps)
                                    {
                                        // 无字段权限不做变更
                                        property.CurrentValue = property.OriginalValue;
                                    }
                                }
                            }
                        }
                        else
                        {
                            // 无实体变更权限不做修改
                            entry.State = EntityState.Unchanged;
                        }
                    }
                }
            }
        }

        protected override Expression<Func<TEntity, bool>> CreateFilterExpression<TEntity>()
        {
            var expression = base.CreateFilterExpression<TEntity>();

            if (typeof(IHasDataAccess).IsAssignableFrom(typeof(TEntity)))
            {
                Expression<Func<TEntity, bool>> expression2 = (TEntity e) => !IsDataAccessFilterEnabled || CreateFilterExpression(e, AccessRuleInfo);
                expression = (Expression<Func<TEntity, bool>>)((expression == null) ? ((LambdaExpression)expression2) : ((LambdaExpression)QueryFilterExpressionHelper.CombineExpressions(expression, expression2)));
            }

            return expression;
        }

        protected static bool CreateFilterExpression<TEntity>(TEntity entity, DataAccessRuleInfo accessRuleInfo)
        {
            if (accessRuleInfo == null)
            {
                return true;
            }

            if (!accessRuleInfo.Rules.Any(r => r.EntityTypeFullName == typeof(TEntity).FullName))
            {
                return false;
            }

            if (entity is not IHasDataAccess accessEntity)
            {
                return true;
            }

            // TODO: 需要完成详细的过滤条件
            return false;
        }
    }
}
