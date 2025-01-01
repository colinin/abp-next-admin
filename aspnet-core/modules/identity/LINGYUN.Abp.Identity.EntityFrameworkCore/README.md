# LINGYUN.Abp.Identity.EntityFrameworkCore

身份认证EntityFrameworkCore模块，提供身份认证相关的数据访问实现。

## 功能特性

* 扩展Volo.Abp.Identity.EntityFrameworkCore.AbpIdentityEntityFrameworkCoreModule模块
* 提供身份认证相关的EF Core仓储实现
* 支持用户头像URL扩展属性的数据库映射
* 提供组织单元的EF Core仓储实现
* 提供会话管理的EF Core仓储实现

## 模块引用

```csharp
[DependsOn(
    typeof(AbpIdentityDomainModule),
    typeof(Volo.Abp.Identity.EntityFrameworkCore.AbpIdentityEntityFrameworkCoreModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 仓储实现

* EfCoreIdentityUserRepository - 用户仓储实现
* EfCoreIdentityRoleRepository - 角色仓储实现
* EfCoreIdentitySessionRepository - 会话仓储实现
* EfCoreOrganizationUnitRepository - 组织单元仓储实现

## 基本用法

1. 配置DbContext
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

2. 使用仓储
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

## 数据库迁移

1. 添加迁移
```bash
dotnet ef migrations add Added_Identity_Tables
```

2. 更新数据库
```bash
dotnet ef database update
```

## 更多信息

* [ABP身份认证文档](https://docs.abp.io/en/abp/latest/Identity)
* [EF Core文档](https://docs.microsoft.com/en-us/ef/core/)
