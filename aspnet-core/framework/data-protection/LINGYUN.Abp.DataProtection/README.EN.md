# LINGYUN.Abp.DataProtection

Data protection implementation module, providing core implementation of data protection.

## Features

* Data Protection Interceptor - Automatically intercepts methods marked with data protection attributes
* Data Authorization Service - Provides data protection validation functionality
* Data Protection Resource Store - Provides in-memory storage implementation for data protection resources

## Configuration

```csharp
public class AbpDataProtectionOptions
{
    /// <summary>
    /// Whether to enable data protection
    /// Default: true
    /// </summary>
    public bool IsEnabled { get; set; }

    /// <summary>
    /// List of data access subject contributors
    /// </summary>
    public IList<IDataAccessSubjectContributor> SubjectContributors { get; }

    /// <summary>
    /// Dictionary of data access keyword contributors
    /// </summary>
    public IDictionary<string, IDataAccessKeywordContributor> KeywordContributors { get; }

    /// <summary>
    /// Dictionary of data access operation contributors
    /// </summary>
    public IDictionary<DataAccessFilterOperate, IDataAccessOperateContributor> OperateContributors { get; }

    /// <summary>
    /// List of ignored audit properties
    /// Default includes: Id, LastModifierId, LastModificationTime, CreatorId, CreationTime, 
    /// IsDeleted, DeleterId, DeletionTime, TenantId, EntityVersion, 
    /// ConcurrencyStamp, ExtraProperties
    /// </summary>
    public IList<string> IgnoreAuditedProperties { get; }
}
```

## Usage

1. Configure Module

```csharp
[DependsOn(typeof(AbpDataProtectionModule))]
public class YourModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpDataProtectionOptions>(options =>
        {
            // Configure data protection options
            options.IsEnabled = true;
            
            // Add custom subject contributor
            options.SubjectContributors.Add(new YourSubjectContributor());
            
            // Add custom keyword contributor
            options.KeywordContributors.Add("your-keyword", new YourKeywordContributor());
            
            // Add custom operation contributor
            options.OperateContributors.Add(DataAccessFilterOperate.Equal, new YourOperateContributor());
            
            // Add ignored audit property
            options.IgnoreAuditedProperties.Add("YourProperty");
        });
    }
}
```

2. Use Data Protection Attributes

```csharp
// Class level data protection control
[DataProtected]
public class YourService
{
    // Method level data protection control
    [DataProtected]
    public virtual Task<YourEntity> GetAsync(Guid id)
    {
        // ...
    }

    // Disable data protection control
    [DisableDataProtected]
    public virtual Task<YourEntity> GetWithoutProtectionAsync(Guid id)
    {
        // ...
    }
}
```

## Related Links

* [中文文档](./README.md)
