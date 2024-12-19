# LINGYUN.Abp.LocalizationManagement.EntityFrameworkCore

Entity Framework Core integration implementation for the localization management module, providing data access and persistence functionality.

## Features

* Implements database mapping for localization management
* Supports custom table prefix and schema
* Provides EF Core implementation of repository interfaces
* Supports database operations for languages, resources, and texts

## Module Dependencies

```csharp
[DependsOn(typeof(AbpLocalizationManagementEntityFrameworkCoreModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Configuration

```csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    context.Services.AddAbpDbContext<YourDbContext>(options =>
    {
        options.AddRepository<Language, EfCoreLanguageRepository>();
        options.AddRepository<Resource, EfCoreResourceRepository>();
        options.AddRepository<Text, EfCoreTextRepository>();
    });

    Configure<AbpDbContextOptions>(options =>
    {
        options.UseMySQL(); // or other databases
    });
}
```

Database table configuration options:
```csharp
public class LocalizationModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
{
    public LocalizationModelBuilderConfigurationOptions(
       string tablePrefix = "",
       string schema = null)
       : base(tablePrefix, schema)
    {
    }
}
```

## Database Tables

* Languages - Language table
* Resources - Resource table
* Texts - Text table

## More Information

* [中文文档](./README.md)
