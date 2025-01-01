# LINGYUN.Abp.Identity.EntityFrameworkCore

Identity authentication EntityFrameworkCore module, providing data access implementation for identity authentication.

## Features

* Extends Volo.Abp.Identity.EntityFrameworkCore.AbpIdentityEntityFrameworkCoreModule module
* Provides EF Core repository implementations for identity authentication
* Supports database mapping for user avatar URL extension property
* Provides EF Core repository implementation for organization units
* Provides EF Core repository implementation for session management

## Module Dependencies

```csharp
[DependsOn(
    typeof(AbpIdentityDomainModule),
    typeof(Volo.Abp.Identity.EntityFrameworkCore.AbpIdentityEntityFrameworkCoreModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Repository Implementations

* EfCoreIdentityUserRepository - User repository implementation
* EfCoreIdentityRoleRepository - Role repository implementation
* EfCoreIdentitySessionRepository - Session repository implementation
* EfCoreOrganizationUnitRepository - Organization unit repository implementation

## Basic Usage

1. Configure DbContext
```csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    context.Services.AddAbpDbContext<IdentityDbContext>(options =>
    {
        options.AddRepository<IdentityRole, EfCoreIdentityRoleRepository>();
        options.AddRepository<IdentityUser, EfCoreIdentityUserRepository>();
        options.AddRepository<IdentitySession, EfCoreIdentitySessionRepository>();
        options.AddRepository<OrganizationUnit, EfCoreOrganizationUnitRepository>();
    });
}
```

2. Use repositories
```csharp
public class YourService
{
    private readonly IRepository<IdentityUser, Guid> _userRepository;
    private readonly IRepository<IdentityRole, Guid> _roleRepository;

    public YourService(
        IRepository<IdentityUser, Guid> userRepository,
        IRepository<IdentityRole, Guid> roleRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
    }

    public async Task<IdentityUser> GetUserAsync(Guid id)
    {
        return await _userRepository.GetAsync(id);
    }

    public async Task<IdentityRole> GetRoleAsync(Guid id)
    {
        return await _roleRepository.GetAsync(id);
    }
}
```

## Database Migrations

1. Add migration
```bash
dotnet ef migrations add Added_Identity_Tables
```

2. Update database
```bash
dotnet ef database update
```

## More Information

* [ABP Identity Documentation](https://docs.abp.io/en/abp/latest/Identity)
* [EF Core Documentation](https://docs.microsoft.com/en-us/ef/core/)
