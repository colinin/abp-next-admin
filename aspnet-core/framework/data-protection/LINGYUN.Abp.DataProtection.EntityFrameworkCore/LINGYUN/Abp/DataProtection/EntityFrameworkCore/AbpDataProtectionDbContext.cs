using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Users;

namespace LINGYUN.Abp.DataProtection.EntityFrameworkCore;

public abstract class AbpDataProtectionDbContext<TDbContext> : AbpDbContext<TDbContext>
    where TDbContext : DbContext
{
    public IOptions<AbpDataProtectionOptions> DataProtectionOptions => LazyServiceProvider.LazyGetRequiredService<IOptions<AbpDataProtectionOptions>>();
    public ICurrentUser CurrentUser => LazyServiceProvider.LazyGetRequiredService<ICurrentUser>();

    public AbpDataProtectionDbContext(
        DbContextOptions<TDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        // TODO: 需要优化表达式树
        // optionsBuilder.AddInterceptors(LazyServiceProvider.GetRequiredService<AbpDataProtectedReadEntityInterceptor>());
        //optionsBuilder.AddInterceptors(LazyServiceProvider.GetRequiredService<AbpDataProtectedWriteEntityInterceptor>());
        optionsBuilder.AddInterceptors(LazyServiceProvider.GetRequiredService<AbpDataProtectedWritePropertiesInterceptor>());
    }

    protected override void ApplyAbpConceptsForAddedEntity(EntityEntry entry)
    {
        base.ApplyAbpConceptsForAddedEntity(entry);
        SetAuthorizationDataProperties(entry);
    }

    protected virtual void SetAuthorizationDataProperties(EntityEntry entry)
    {
        if (entry.Entity is IDataProtected data)
        {
            // TODO: 埋点, 以后可用EF.Functions查询
            if (data.GetProperty(DataAccessKeywords.AUTH_ROLES) == null)
            {
                data.SetProperty(DataAccessKeywords.AUTH_ROLES, CurrentUser.Roles.Select(role => $"[{role}]").JoinAsString(","));
            }
            if (data.GetProperty(DataAccessKeywords.AUTH_ORGS) == null)
            {
                data.SetProperty(DataAccessKeywords.AUTH_ORGS, CurrentUser.FindOrganizationUnits().Select(ou => $"[{ou}]").JoinAsString(","));
            }
        }
    }
}
