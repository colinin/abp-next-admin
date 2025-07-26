# LINGYUN.Abp.AspNetCore.Auditing

审计日期扩展模块, 用于在审计日志中加入特定的Http请求头记录  

## 模块引用


```csharp
[DependsOn(typeof(AbpAspNetCoreAuditingModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 配置项

*	AbpAspNetCoreAuditingHeaderOptions.IsEnabled      是否在审计日志中记录Http请求头,默认: true
*	AbpAspNetCoreAuditingHeaderOptions.HttpHeaders    需要在审计日志中记录的Http请求头列表
