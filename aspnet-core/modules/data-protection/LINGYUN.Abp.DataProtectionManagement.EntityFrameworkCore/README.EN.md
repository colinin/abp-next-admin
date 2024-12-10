# LINGYUN.Abp.DataProtectionManagement.EntityFrameworkCore

Data protection management EntityFrameworkCore module, providing data access implementation for data protection management.

## Features

* Data Protection Management Repository Implementation
* Database Mapping Configuration
* Database Migration

## Repository Implementation

* `EfCoreDataProtectionRepository` - Data Protection Repository EF Core Implementation
  * Implements `IDataProtectionRepository` interface
  * Provides CRUD operations for data protection
  * Supports data filtering

## Database Mapping Configuration

```csharp
public static class DataProtectionDbContextModelCreatingExtensions
{
    public static void ConfigureDataProtectionManagement(
        this ModelBuilder builder,
        Action<DataProtectionModelBuilderConfigurationOptions> optionsAction = null)
    {
        builder.Entity<DataProtection>(b =>
        {
            b.ToTable(options.TablePrefix + "DataProtections", options.Schema);

            b.ConfigureByConvention();

            b.Property(x => x.Name).IsRequired().HasMaxLength(DataProtectionConsts.MaxNameLength);
            b.Property(x => x.DisplayName).HasMaxLength(DataProtectionConsts.MaxDisplayNameLength);
            b.Property(x => x.Description).HasMaxLength(DataProtectionConsts.MaxDescriptionLength);

            b.HasIndex(x => x.Name);
        });
    }
}
```

## Configuration and Usage

1. Add Module Dependency

```csharp
[DependsOn(typeof(AbpDataProtectionManagementEntityFrameworkCoreModule))]
public class YourModule : AbpModule
{
}
```

2. Configure DbContext

```csharp
public class YourDbContext : AbpDbContext<YourDbContext>, IDataProtectionManagementDbContext
{
    public DbSet<DataProtection> DataProtections { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureDataProtectionManagement();
    }
}
```

## Related Links

* [中文文档](./README.md)
