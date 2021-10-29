# LINGYUN.Abp.AuditLogging.EntityFrameworkCore

审计模块 EntityFrameworkCore 实现, 此模块仅作为桥梁, 具体实现交给abp官方模块  

AuditLogManager    实现了 IAuditLogManager, 审计日志由Volo.Abp.AuditLogging模块管理  
SecurityLogManager 实现了 ISecurityLogManager, 安全日志由Volo.Abp.Identity模块管理  

## 模块引用

```csharp
[DependsOn(typeof(AbpAuditLoggingEntityFrameworkCoreModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 配置项

请遵循 Volo.Abp.AuditLogging、Volo.Abp.Identity模块中的配置  

## appsettings.json

```json
{
  "ConnectionStrings": {
    "AbpIdentity": "Server=127.0.0.1;Database=Identity;User Id=root;Password=*",
    "AbpAuditLogging": "Server=127.0.0.1;Database=AuditLogging;User Id=root;Password=*",
  }
}

```