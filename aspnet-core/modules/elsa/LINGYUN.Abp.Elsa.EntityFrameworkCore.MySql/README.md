# LINGYUN.Abp.Elsa.EntityFrameworkCore.MySql

Elsa工作流的MySql数据库提供程序模块

## 功能

* 提供Elsa工作流的MySql数据库支持
* 依赖于 `AbpElsaEntityFrameworkCoreModule` 和 `AbpEntityFrameworkCoreMySQLModule`
* 预配置以下功能:
  * 持久化存储
  * Webhooks存储
  * 工作流设置存储

## 配置使用

```csharp
[DependsOn(
    typeof(AbpElsaEntityFrameworkCoreMySqlModule)
    )]
public class YouProjectModule : AbpModule
{
}
```

## 配置项

```json
{
    "Elsa": {
        "Persistence": true,     // 启用持久化存储
        "Webhooks": true,       // 启用Webhooks存储
        "WorkflowSettings": true // 启用工作流设置存储
    }
}
```

[English](./README.EN.md)
