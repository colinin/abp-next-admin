# LINGYUN.Abp.MultiTenancy.DbFinder

abp 多租户数据库查询组件,引用此模块将首先从分布式缓存查询当前租户配置

如果缓存不存在,则调用租户仓储接口获取租户数据,并存储到分布式缓存中

## 配置使用

模块按需引用

启动项目需要引用**Volo.Abp.TenantManagement.EntityFrameworkCore**

``` shell
 // .NET CLI
 dotnet add package Volo.Abp.TenantManagement.EntityFrameworkCore
 
 // Package Manager
Install-Package Volo.Abp.TenantManagement.EntityFrameworkCore
 
```

事先定义**appsettings.json**文件

```json
{
  "ConnectionStrings": {
    "AbpTenantManagement": "Server=127.0.0.1;Database=TenantDb;User Id=root;Password=yourPassword"
  }
}

```

```csharp
[DependsOn(typeof(AbpDbFinderMultiTenancyModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```