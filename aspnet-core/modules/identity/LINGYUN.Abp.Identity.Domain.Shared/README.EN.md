# LINGYUN.Abp.Identity.Domain.Shared

Identity authentication domain shared module, providing basic definitions for identity authentication.

## Features

* Provides localization resources for identity authentication
* Provides virtual file system configuration for identity authentication
* Extends Volo.Abp.Identity.AbpIdentityDomainSharedModule module

## Module Dependencies

```csharp
[DependsOn(typeof(AbpIdentityDomainSharedModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Configuration

No additional configuration required.

## Basic Usage

1. Reference the module
```csharp
[DependsOn(typeof(AbpIdentityDomainSharedModule))]
public class YouProjectModule : AbpModule
{
}
```

2. Use localization resources
```csharp
public class YourClass
{
    private readonly IStringLocalizer<IdentityResource> _localizer;

    public YourClass(IStringLocalizer<IdentityResource> localizer)
    {
        _localizer = localizer;
    }

    public string GetLocalizedString(string key)
    {
        return _localizer[key];
    }
}
```

## More Information

* [ABP Identity Documentation](https://docs.abp.io/en/abp/latest/Identity)
