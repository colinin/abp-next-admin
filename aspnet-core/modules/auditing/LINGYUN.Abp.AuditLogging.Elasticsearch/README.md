# LINGYUN.Abp.AuditLogging.Elasticsearch

审计模块 Elasticsearch 实现

ElasticsearchAuditLogManager    实现了 IAuditLogManager, 审计日志由ES管理  
ElasticsearchSecurityLogManager 实现了 ISecurityLogManager, 安全日志由ES管理  

## 模块引用


```csharp
[DependsOn(typeof(AbpAuditLoggingElasticsearchModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 配置项

*	AbpAuditLoggingElasticsearchOptions.IndexPrefix      索引前缀, 默认 auditlogging

## 注意事项

与租户模块集成, 跨租户时将会切换索引

## appsettings.json

```json
{
  "AuditLogging": {
    "Elasticsearch": {
      "IndexPrefix": "auditlogging"
    }
  }
}

```