# LINGYUN.Abp.Identity.Domain

Identity authentication domain module, providing core functionality implementation for identity authentication.

## Features

* Extends Volo.Abp.Identity.AbpIdentityDomainModule module
* Provides identity session management functionality
* Provides identity session cleanup functionality
* Supports distributed events

## Module Dependencies

```csharp
[DependsOn(
    typeof(AbpIdentityDomainModule),
    typeof(Volo.Abp.Identity.AbpIdentityDomainModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Configuration

### IdentitySessionCleanupOptions

```json
{
  "Identity": {
    "Session": {
      "Cleanup": {
        "IsEnabled": false,           // Enable session cleanup, default: false
        "CleanupPeriod": 3600000,    // Cleanup interval (milliseconds), default: 1 hour
        "InactiveTimeSpan": "30.00:00:00" // Inactive session retention period, default: 30 days
      }
    }
  }
}
```

### IdentitySessionSignInOptions

```json
{
  "Identity": {
    "Session": {
      "SignIn": {
        "AuthenticationSchemes": ["Identity.Application"], // Authentication schemes for processing
        "SignInSessionEnabled": false,     // Enable SignInManager login session, default: false
        "SignOutSessionEnabled": false     // Enable SignInManager logout session, default: false
      }
    }
  }
}
```

## Basic Usage

1. Configure identity session management
```csharp
Configure<IdentitySessionSignInOptions>(options =>
{
    options.SignInSessionEnabled = true;    // Enable login session
    options.SignOutSessionEnabled = true;   // Enable logout session
});
```

2. Configure session cleanup
```csharp
Configure<IdentitySessionCleanupOptions>(options =>
{
    options.IsCleanupEnabled = true;    // Enable session cleanup
    options.CleanupPeriod = 3600000;    // Set cleanup interval to 1 hour
    options.InactiveTimeSpan = TimeSpan.FromDays(7);  // Set inactive session retention period to 7 days
});
```

## More Information

* [ABP Identity Documentation](https://docs.abp.io/en/abp/latest/Identity)
