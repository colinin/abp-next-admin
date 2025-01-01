# LINGYUN.Abp.MultiTenancy.Editions

Multi-tenancy edition management module, providing basic functionality support for tenant editions.

## Features

- Edition Management
  - Support for assigning different editions to tenants
  - Edition information includes ID and display name
  - Support for edition information storage and retrieval
- Global Feature Toggles
  - Control edition functionality through global feature switches
  - Flexible configuration for enabling/disabling edition features
- Authentication Integration
  - Automatically adds edition information to user Claims
  - Support for accessing current tenant's edition information in applications
- Extensibility
  - Provides IEditionStore interface for custom edition storage
  - Support for custom edition configuration providers

## Module Reference

```csharp
[DependsOn(typeof(AbpMultiTenancyEditionsModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Configuration

### 1. Global Feature Configuration

```csharp
GlobalFeatureManager.Instance.Modules.Editions(editions =>
{
    // Configure edition-related global features here
});
```

### 2. Implementing Edition Storage

```csharp
public class YourEditionStore : IEditionStore
{
    public async Task<EditionInfo> FindByTenantAsync(Guid tenantId)
    {
        // Implement logic to retrieve tenant edition information from storage
        return new EditionInfo(
            id: Guid.NewGuid(),
            displayName: "Standard Edition"
        );
    }
}
```

### 3. Retrieving Edition Information

```csharp
public class YourService
{
    private readonly IEditionConfigurationProvider _editionProvider;

    public YourService(IEditionConfigurationProvider editionProvider)
    {
        _editionProvider = editionProvider;
    }

    public async Task<EditionConfiguration> GetEditionAsync(Guid? tenantId)
    {
        return await _editionProvider.GetAsync(tenantId);
    }
}
```

## Using Edition Information in Claims

When edition functionality is enabled, the module automatically adds edition information to user Claims:

```csharp
public class YourController
{
    public async Task<IActionResult> GetEditionInfo()
    {
        var editionId = User.FindFirstValue(AbpClaimTypes.EditionId);
        // Perform operations using the edition ID
    }
}
```

## Important Notes

1. Ensure global feature toggle is enabled before using edition functionality.
2. Edition storage implementation should consider performance and concurrent access issues.
3. Changes to edition information may affect tenant feature access permissions.
4. Edition information in Claims is automatically updated during user authentication.

## More Documentation

- [Chinese Documentation](README.md)
