# LINGYUN.Abp.DataProtection.EntityFrameworkCore

Data protection EntityFramework Core implementation module

## Interface Description

* DisableDataProtectedAttribute: Automatically implements DataFilter.Disable<IDataProtected>() through interceptor, data filter will be disabled in the current scope

## Important Notes

* When using repository interfaces, try to avoid using *await GetDbSetAsync()* directly, use *await GetQueryableAsync()* instead, because due to the **DbSet** design pattern, it cannot be processed at the moment
* Your repository interface should inherit from **EfCoreDataProtectionRepository**, and your *DbContext* should inherit from **AbpDataProtectionDbContext**

## Configuration and Usage

```csharp
[DependsOn(
    typeof(AbpDataProtectionEntityFrameworkCoreModule)
    )]
public class YouProjectModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<DataProtectionManagementOptions>(options =>
        {
            // Persist protected entity list
            options.AddEntities(typeof(YouResource), new Type[]
            {
                typeof(YouProtectionObject),
            });

            // Format as follows
            // options.AddEntities(typeof(IdentityResource), new Type[]
            // {
            //     typeof(IdentityUser),
            //     typeof(IdentityRole),
            //     typeof(OrganizationUnit),
            // });
        });
    }
}

public class YouDbContext : AbpDataProtectionDbContext<YouDbContext>
{
    public DbSet<YouProtectionObject> ProtectionObjects { get; set; }
    public YouDbContext(
        DbContextOptions<YouDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<YouProtectionObject>(b =>
        {
            // ...
        });
    }
}

public class EfCoreYouProtectionObjectRepository :
    EfCoreDataProtectionRepository<YouDbContext, YouProtectionObject, int>,
    IYouProtectionObjectRepository
{
    protected IDataFilter DataFilter { get; }
    public EfCoreYouProtectionObjectRepository(
        [NotNull] IDbContextProvider<YouDbContext> dbContextProvider, 
        [NotNull] IDataAuthorizationService dataAuthorizationService, 
        [NotNull] IEntityTypeFilterBuilder entityTypeFilterBuilder,
        IDataFilter dataFilter) 
        : base(dbContextProvider, dataAuthorizationService, entityTypeFilterBuilder)
    {
        DataFilter = dataFilter;
    }

    // Get protected data list
    public async virtual Task<List<YouProtectionObject>> GetProtectedListAsync()
    {
        return await (await GetQueryableAsync())
            .ToListAsync();
    }

    // Mark with DisableDataProtected to get all data list, automatically handle DataFilter.Disable<IDataProtected>() through interceptor
    [DisableDataProtected]
    public async virtual Task<List<YouProtectionObject>> GetUnProtectedListAsync()
    {
        return await (await GetQueryableAsync())
            .ToListAsync();
    }

    // Disable IDataProtected filter to get all data list (can be used anywhere)
    public async virtual Task<List<YouProtectionObject>> GetUnProtectedByFilterListAsync()
    {
        using (DataFilter.Disable<IDataProtected>())
        {
            return await (await GetQueryableAsync())
                .ToListAsync();
        }
    }
}
```

## Related Links

* [中文文档](./README.md)
