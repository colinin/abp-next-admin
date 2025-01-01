# LINGYUN.Abp.DataProtection

数据权限实现模块，提供数据权限的核心实现。

## 功能

* 数据权限拦截器 - 自动拦截标记了数据权限特性的方法
* 数据权限验证服务 - 提供数据权限验证功能
* 数据权限资源存储 - 提供数据权限资源的内存存储实现

## 配置项

```csharp
public class AbpDataProtectionOptions
{
    /// <summary>
    /// 是否启用数据权限
    /// 默认: true
    /// </summary>
    public bool IsEnabled { get; set; }

    /// <summary>
    /// 数据权限主体提供者列表
    /// </summary>
    public IList<IDataAccessSubjectContributor> SubjectContributors { get; }

    /// <summary>
    /// 数据权限关键字提供者字典
    /// </summary>
    public IDictionary<string, IDataAccessKeywordContributor> KeywordContributors { get; }

    /// <summary>
    /// 数据权限操作提供者字典
    /// </summary>
    public IDictionary<DataAccessFilterOperate, IDataAccessOperateContributor> OperateContributors { get; }

    /// <summary>
    /// 忽略审计属性列表
    /// 默认包含：Id, LastModifierId, LastModificationTime, CreatorId, CreationTime, 
    /// IsDeleted, DeleterId, DeletionTime, TenantId, EntityVersion, 
    /// ConcurrencyStamp, ExtraProperties
    /// </summary>
    public IList<string> IgnoreAuditedProperties { get; }
}
```

## 使用方式

1. 配置模块

```csharp
[DependsOn(typeof(AbpDataProtectionModule))]
public class YourModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpDataProtectionOptions>(options =>
        {
            // 配置数据权限选项
            options.IsEnabled = true;
            
            // 添加自定义主体提供者
            options.SubjectContributors.Add(new YourSubjectContributor());
            
            // 添加自定义关键字提供者
            options.KeywordContributors.Add("your-keyword", new YourKeywordContributor());
            
            // 添加自定义操作提供者
            options.OperateContributors.Add(DataAccessFilterOperate.Equal, new YourOperateContributor());
            
            // 添加忽略的审计属性
            options.IgnoreAuditedProperties.Add("YourProperty");
        });
    }
}
```

2. 使用数据权限特性

```csharp
// 类级别的数据权限控制
[DataProtected]
public class YourService
{
    // 方法级别的数据权限控制
    [DataProtected]
    public virtual Task<YourEntity> GetAsync(Guid id)
    {
        // ...
    }

    // 禁用数据权限控制
    [DisableDataProtected]
    public virtual Task<YourEntity> GetWithoutProtectionAsync(Guid id)
    {
        // ...
    }
}
```

## 相关链接

* [English document](./README.EN.md)
