# LINGYUN.Abp.DataProtection.EntityFrameworkCore

数据权限EF实现模块

## 接口描述  

* DisableDataProtectedAttribute: 通过拦截器自动实现DataFilter.Disable<IDataProtected>(),数据过滤器在当前范围将被禁用  

## 注意事项

* 使用仓储接口尽量避免直接使用 *await GetDbSetAsync()*, 而是使用 *await GetQueryableAsync()*, 因为 **DbSet** 设计模式原因,暂无法对其进行处理  
* 您的仓库接口应继承自 **EfCoreDataProtectionRepository**, 您的 *DbContext* 应继承自 **AbpDataProtectionDbContext**  

## 配置使用

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
            // 受保护的实体列表持久化
            options.AddEntities(typeof(YouResource), new Type[]
            {
                typeof(YouProtectionObject),
            });

            // 如下格式
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

    // 获取受保护的数据列表
    public async virtual Task<List<YouProtectionObject>> GetProtectedListAsync()
    {
        return await (await GetQueryableAsync())
            .ToListAsync();
    }

    // 标注 DisableDataProtected 获取全部数据列表, 通过拦截器自动处理 DataFilter.Disable<IDataProtected>()
    [DisableDataProtected]
    public async virtual Task<List<YouProtectionObject>> GetUnProtectedListAsync()
    {
        return await (await GetQueryableAsync())
            .ToListAsync();
    }

    // 禁用 IDataProtected 过滤器获取全部数据列表(可在任意地方使用)
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

