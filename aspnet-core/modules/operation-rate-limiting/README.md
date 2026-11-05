[English](./README.EN.md) | 简体中文

# 操作限流策略管理（modules/operation-rate-limiting）

操作限流基础库位于 `framework/operation-rate-limiting`，本目录是策略管理模块，负责把动态策略持久化到数据库，并通过应用服务 / REST API 管理。

## 分层

| 项目 | 说明 |
| --- | --- |
| `LINGYUN.Abp.OperationRateLimiting.Domain.Shared` | 常量、错误码、`OperationRateLimitingPolicyChangedEto` 等 ETO、前端所需本地化资源 |
| `LINGYUN.Abp.OperationRateLimiting.Domain` | `OperationRateLimitingPolicyRecord`、`OperationRateLimitingRuleRecord` 实体，`IOperationRateLimitingPolicyRecordRepository` 仓储接口，实体与运行时定义的映射扩展 |
| `LINGYUN.Abp.OperationRateLimiting.EntityFrameworkCore` | EF Core 仓储、`OperationRateLimitingDbContext`、`ConfigureOperationRateLimiting()`、带缓存的动态策略存储 |
| `LINGYUN.Abp.OperationRateLimiting.Application.Contracts` | DTO、权限定义、`IOperationRateLimitingPolicyAppService`、本地化 |
| `LINGYUN.Abp.OperationRateLimiting.Application` | 策略 CRUD 应用服务；保存/删除后调用 `IDynamicOperationRateLimitingPolicyStore.InvalidateCacheAsync()` |
| `LINGYUN.Abp.OperationRateLimiting.HttpApi` | `api/operation-rate-limiting/policies` REST API |

## 依赖

```csharp
[DependsOn(
    typeof(AbpOperationRateLimitingEntityFrameworkCoreModule),
    typeof(AbpOperationRateLimitingManagementApplicationModule),
    typeof(AbpOperationRateLimitingManagementHttpApiModule))]
public class YouProjectModule : AbpModule { }
```

数据库配置：

```csharp
Configure<AbpDbContextOptions>(options =>
{
    options.Configure<OperationRateLimitingDbContext>(context =>
    {
        context.DbContextOptions.UseSqlServer(connectionString);
    });
});
```

或者把 `OperationRateLimitingPolicyRecord` / `OperationRateLimitingRuleRecord` 加入现有 DbContext，并在 `OnModelCreating` 中调用 `builder.ConfigureOperationRateLimiting()`；策略表为 `OperationRateLimitingPolicies`，规则子表为 `OperationRateLimitingPolicyRules`（`Properties` 为 JSON 列），请为所用数据库生成迁移。

## 权限

| 权限 | 说明 |
| --- | --- |
| `OperationRateLimitingManagement.Policies` | 查看策略 |
| `OperationRateLimitingManagement.Policies.Create` | 创建策略 |
| `OperationRateLimitingManagement.Policies.Update` | 更新策略 |
| `OperationRateLimitingManagement.Policies.Delete` | 删除策略 |

## REST API

| 方法 | 路由 | 说明 |
| --- | --- | --- |
| GET | `api/operation-rate-limiting/policies/{id}` | 按 Id 查询 |
| GET | `api/operation-rate-limiting/policies/by-name/{name}` | 按名称查询 |
| GET | `api/operation-rate-limiting/policies` | 分页查询（`Filter` / `IsEnabled` / `Algorithm`） |
| POST | `api/operation-rate-limiting/policies` | 创建 |
| PUT | `api/operation-rate-limiting/policies/{id}` | 更新 |
| DELETE | `api/operation-rate-limiting/policies/{id}` | 删除 |

创建示例：

```json
{
  "name": "SendSmsCode",
  "displayName": "发送短信验证码",
  "isEnabled": true,
  "rules": [
    {
      "algorithm": "FixedWindow",
      "durationSeconds": 60,
      "maxCount": 1,
      "partitionType": "Parameter"
    }
  ]
}
```

规则字段：`algorithm`（内置为 `FixedWindow` / `SlidingWindow` / `TokenBucket` / `Concurrency`，也可使用应用启动时注册的自定义算法名）、`durationSeconds`、`maxCount`、`segments`、`tokensPerPeriod`、`partitionType`、`partitionResolverName`、`isMultiTenancyEnabled`、`errorCode`、`name`（稳定计数键）、`properties`（算法特有参数，如 `SlidingWindow:Segments`、`TokenBucket:TokensPerPeriod`）。`durationSeconds` 和 `maxCount` 为公共属性，直接存储在规则定义上。

* `partitionType` 取值：`Parameter` / `CurrentUser` / `CurrentTenant` / `ClientIp` / `Email` / `PhoneNumber` / `Custom`。
* 自定义算法与自定义分区解析器必须已在 `AbpOperationRateLimitingOptions` 中注册，否则创建/更新会返回 `LINGYUN.Abp.OperationRateLimiting.Management:010002`。
* 同名策略返回 `LINGYUN.Abp.OperationRateLimiting.Management:010001`。

## 与静态策略的合并

写操作（创建/更新/删除）成功后会发布 `OperationRateLimitingPolicyChangedEto`（包含策略与规则快照，删除时为空），可用于其他服务做缓存刷新或审计订阅；同时应用服务会立即失效当前租户的动态策略缓存。

管理接口写入的是动态策略；读取时由基础库的 `OperationRateLimitingPolicyManager` 与静态策略按 `DynamicDefinitionStrategy.Merge` 合并：

* 同名时数据库的 `DisplayName`、`ErrorCode`、`IsEnabled` 覆盖静态值，规则列表非空时整体替换。
* 仅数据库存在的策略直接生效。
* 动态策略按租户缓存 5 分钟；应用服务在写入后自动失效当前租户缓存。
