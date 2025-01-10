# LINGYUN.Abp.Authorization.OrganizationUnits

组织单元权限验证模块，提供基于组织单元的权限验证功能。

## 功能特性

* 支持基于组织单元的权限验证
* 提供组织单元权限值提供者(OrganizationUnitPermissionValueProvider)
* 支持多组织单元权限验证
* 集成ABP权限系统
* 提供组织单元Claim类型扩展
* 支持当前用户组织单元查询扩展

## 模块引用

```csharp
[DependsOn(typeof(AbpAuthorizationOrganizationUnitsModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 基本用法

1. 配置权限提供者
   ```csharp
   public override void ConfigureServices(ServiceConfigurationContext context)
   {
       Configure<AbpPermissionOptions>(options =>
       {
           options.ValueProviders.Add<OrganizationUnitPermissionValueProvider>();
       });
   }
   ```

2. 获取当前用户的组织单元
   ```csharp
   public class YourService
   {
       private readonly ICurrentUser _currentUser;

       public YourService(ICurrentUser currentUser)
       {
           _currentUser = currentUser;
       }

       public void YourMethod()
       {
           var organizationUnits = _currentUser.FindOrganizationUnits();
           // 使用组织单元进行业务处理
       }
   }
   ```

3. 从ClaimsPrincipal获取组织单元
   ```csharp
   public class YourService
   {
       public void YourMethod(ClaimsPrincipal principal)
       {
           var organizationUnits = principal.FindOrganizationUnits();
           // 使用组织单元进行业务处理
       }
   }
   ```

## 更多资源

* [GitHub仓库](https://github.com/colinin/abp-next-admin)
* [示例应用程序](https://github.com/colinin/abp-next-admin/tree/master/aspnet-core/services/LY.MicroService.Applications.Single)

[English](./README.EN.md)
