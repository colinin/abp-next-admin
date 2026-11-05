[English](./README.en.md) | 简体中文

# LINGYUN.Abp.OperationRateLimiting

操作级限流模块，实现 ABP [Operation Rate Limiting](https://github.com/abpframework/abp/blob/dev/docs/en/modules/operation-rate-limiting.md) 设计文档中的能力。

与 [ASP.NET Core 限流中间件](https://learn.microsoft.com/en-us/aspnet/core/performance/rate-limit) 不同，本模块工作在**应用/领域代码**层：开发者显式调用 `IOperationRateLimitingChecker`，或在方法/控制器上使用 `[OperationRateLimiting]` 特性，从而对“发送短信验证码”“生成报表”“登录尝试”等具体业务操作限流。

## 功能

* 内置四种算法：固定窗口、滑动窗口、令牌桶、并发限制（与 ASP.NET Core 的四种内置算法对应）。
* 可扩展算法：实现 `IOperationRateLimitingRule` 即可注册新的算法。
* 分区策略：参数、当前用户、当前租户、客户端 IP、邮箱（自动大写归一化）、手机号（自动去格式化字符）、自定义命名解析器。
* 多规则策略（AND 语义），支持“先全部预检查、通过后再递增”，避免已阻塞的请求继续消耗其他规则配额。
* 策略覆盖：`AddPolicy` 全量替换、`ConfigurePolicy` 局部修改、`ClearRules`。
* 声明式限流：`[OperationRateLimiting]` 支持应用服务（拦截器）与 MVC 控制器（ActionFilter）。
* 永久封禁策略（`maxCount: 0`）、自定义错误码、`Properties`。
* 动态策略仓储：策略持久化到数据库，与静态策略按 `DynamicDefinitionStrategy` 合并（复用 `LINGYUN.Abp.Dynamic.Definitions` 的合并机制）。
* 可替换的存储（分布式缓存 + 分布式锁）、格式化器、策略提供者。

## 项目

| 项目 | 说明 |
| --- | --- |
| `LINGYUN.Abp.OperationRateLimiting` | 基础库：选项、策略、算法、检查器、异常、特性、本地化、默认分布式缓存存储 |
| `LINGYUN.Abp.OperationRateLimiting.AspNetCore` | 基础库：MVC 控制器 `[OperationRateLimiting]` 过滤器、客户端 IP 解析 |
| `LINGYUN.Abp.OperationRateLimiting.StackExchangeRedis` | 基础库：基于 Redis + Lua 的原子存储实现 |

策略管理（Application / HttpApi / EF Core 仓储）位于 `modules/operation-rate-limiting`：

| 项目 | 说明 |
| --- | --- |
| `LINGYUN.Abp.OperationRateLimiting.Domain.Shared` | 常量、错误码、`OperationRateLimitingPolicyChangedEto` 等 ETO、前端本地化资源 |
| `LINGYUN.Abp.OperationRateLimiting.Domain` | 动态策略/规则实体与仓储接口 |
| `LINGYUN.Abp.OperationRateLimiting.EntityFrameworkCore` | 动态策略 EF Core 仓储与缓存存储、`DbContext` 配置扩展 |
| `LINGYUN.Abp.OperationRateLimiting.Application.Contracts` | DTO、权限、应用服务接口、本地化 |
| `LINGYUN.Abp.OperationRateLimiting.Application` | 策略 CRUD 应用服务（保存后自动失效动态策略缓存） |
| `LINGYUN.Abp.OperationRateLimiting.HttpApi` | 策略管理 REST API |

## 快速开始

在模块中依赖 `AbpOperationRateLimitingModule`，并在 `ConfigureServices` 中定义策略：

```csharp
[DependsOn(typeof(AbpOperationRateLimitingModule))]
public class MyModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpOperationRateLimitingOptions>(options =>
        {
            options.AddPolicy("SendSmsCode", policy =>
            {
                policy.WithFixedWindow(TimeSpan.FromMinutes(1), maxCount: 1)
                      .PartitionByParameter();
            });
        });
    }
}
```

注入 `IOperationRateLimitingChecker` 并调用：

```csharp
public class SmsAppService : ApplicationService
{
    private readonly IOperationRateLimitingChecker _rateLimitChecker;

    public SmsAppService(IOperationRateLimitingChecker rateLimitChecker)
    {
        _rateLimitChecker = rateLimitChecker;
    }

    public virtual async Task SendCodeAsync(string phoneNumber)
    {
        // 超限时抛出 AbpOperationRateLimitingException（HTTP 429）
        await _rateLimitChecker.CheckAsync("SendSmsCode", phoneNumber);

        // 发送验证码...
    }
}
```

检查器提供四个方法（均提供仅传 `parameter` 字符串的扩展方法重载）：

| 方法 | 说明 |
| --- | --- |
| `CheckAsync` | 检查并递增计数；超限抛出 `AbpOperationRateLimitingException` |
| `IsAllowedAsync` | 只读检查，不递增 |
| `GetStatusAsync` | 返回详细状态，不递增 |
| `ResetAsync` | 重置该策略/分区计数 |

并发限制额外提供 `AcquireAsync`（返回 `IOperationRateLimitingLease`，释放时归还许可）与 `ReleaseAsync`。

## 四种内置算法

| 构建方法 | 参数 | 语义 | 适用场景 |
| --- | --- | --- | --- |
| `WithFixedWindow(duration, maxCount)` | 窗口、上限 | 固定时间窗口内最多 `maxCount` 次 | 每分钟 N 次短信 |
| `WithSlidingWindow(duration, maxCount, segments = 10)` | 窗口、上限、分段数 | 最近一个窗口内最多 `maxCount` 次，按 `segments` 分段近似统计 | 更平滑的 N 次/周期 |
| `WithTokenBucket(period, tokenLimit, tokensPerPeriod = 1)` | 补充周期、桶容量、每次补充令牌数 | 初始满桶，允许突发；每周期补充 `tokensPerPeriod` 个令牌 | 允许突发的 API 调用 |
| `WithConcurrency(permitLimit, leaseDuration)` | 并发许可数、租约时长 | 同时最多 `permitLimit` 个在途操作；许可显式释放或租约到期后自动释放 | 报表生成、导出等重操作 |

滑动窗口按对齐分段统计，属于有界内存的近似算法（与 ASP.NET Core `SlidingWindowRateLimiter` 一致）。

`Duration` 和 `MaxCount` 是所有算法共有的属性，直接定义在 `OperationRateLimitingRuleDefinition` 上；其余算法特有参数（如 `SlidingWindow:Segments`、`TokenBucket:TokensPerPeriod`）存储在 `OperationRateLimitingRuleDefinition.Properties` 中，键名以算法名作前缀。各算法在对应命名空间提供扩展方法读写这些特有参数，例如 `OperationRateLimitingRuleDefinitionTokenBucketExtensions.GetTokensPerPeriod` / `SetTokensPerPeriod`；自定义算法可通过 `rule.WithProperty("MyAlgorithm:Key", "value")` 扩展参数，并通过 `rule.GetPropertyOrNull("MyAlgorithm:Key")` 读取。

### Redis 原子实现（Lua）

默认存储基于 `IDistributedCache`，用 `IAbpDistributedLock` 包住“读-改-写”来保证一致性；锁本身也依赖分布式锁实现。对 Redis 部署，引用 `LINGYUN.Abp.OperationRateLimiting.StackExchangeRedis` 并依赖 `AbpOperationRateLimitingStackExchangeRedisModule` 即可替换为 **Lua 原子实现**，无需分布式锁：

```csharp
[DependsOn(typeof(AbpOperationRateLimitingStackExchangeRedisModule))]
public class MyModule : AbpModule { }
```

```json
{
  "OperationRateLimiting": {
    "Redis": {
      "Configuration": "127.0.0.1:6379",
      "InstanceName": "my-app:"
    }
  }
}
```

* 四种内置算法各自一条 Lua 脚本，在 Redis 内完成读改写：固定窗口 `GET/SET`、滑动窗口 `ZSET`、令牌桶 `HASH`、并发 `ZSET`，返回值包含放行、当前值、剩余值与重试毫秒数。
* 自定义算法的状态通过 `UpdateStateAsync` 的 Lua CAS（比较并交换）循环更新，同样具备原子性。
* 连接串也可通过 `ConfigurationOptions` 提供更细粒度配置；`InstanceName` 为所有键的统一前缀。
* 行为与默认存储一致；`IOperationRateLimitingStore` 的替换对上层完全透明。

## 分区策略

| 方法 | 分区键 |
| --- | --- |
| `PartitionByParameter()` | `OperationRateLimitingContext.Parameter` 原值（不做归一化） |
| `PartitionByCurrentUser()` | `ICurrentUser.Id`（未认证抛出异常） |
| `PartitionByCurrentTenant()` | `ICurrentTenant.Id`，Host 侧为 `host` |
| `PartitionByClientIp()` | `IWebClientInfoProvider.ClientIpAddress`，需要 ASP.NET Core 环境 |
| `PartitionByEmail()` | 参数优先，其次当前用户邮箱；统一转换为大写 |
| `PartitionByPhoneNumber()` | 参数优先，其次当前用户手机号；去除空格、`-`、`.`、`()`，保留 `+` 与数字 |
| `PartitionBy("Name")` | 命名自定义解析器（配置期校验是否存在） |
| `PartitionBy("Name", resolver)` | 内联注册（或替换）并引用自定义解析器 |

自定义分区解析器：

```csharp
options.AddPartitionKeyResolver("ByDevice", ctx =>
    Task.FromResult($"{ctx.Parameter}:{ctx.Properties["DeviceId"]}"));

options.AddPolicy("ApiCall", policy =>
    policy.WithFixedWindow(TimeSpan.FromHours(1), 100).PartitionBy("ByDevice"));
```

`ReplacePartitionKeyResolver` 用于替换已注册的解析器。

## 多规则与策略覆盖

```csharp
options.AddPolicy("Login", policy =>
{
    // 每个用户名 5 分钟最多 5 次
    policy.AddRule(rule => rule
        .WithFixedWindow(TimeSpan.FromMinutes(5), 5)
        .PartitionByParameter());

    // 每个 IP 每小时最多 20 次
    policy.AddRule(rule => rule
        .WithFixedWindow(TimeSpan.FromHours(1), 20)
        .PartitionByClientIp());
});
```

* 多规则为 **AND** 关系：所有规则都允许才放行。
* 先对所有规则做只读预检查，全部通过后才逐一递增；并发竞争导致的失败不会回滚已递增的计数（与文档一致）。

覆盖已存在的策略：

```csharp
// 全量替换
options.AddPolicy("Account.SendPasswordResetCode", policy => { /* ... */ });

// 局部修改（保留原有规则）
options.ConfigurePolicy("Account.SendPasswordResetCode", policy =>
{
    policy.WithErrorCode("MyApp:SmsCodeLimit");
    // 或 policy.ClearRules(); ...
});
```

命名规则用于在参数变化时保持计数键稳定：

```csharp
policy.AddRule(rule => rule
    .WithName("HourlyLimit")
    .WithFixedWindow(TimeSpan.FromHours(1), 100)
    .PartitionByCurrentUser());
```

规则名在策略内必须唯一；未命名规则按算法/时长/上限/分区类型去重。

## 声明式限流

应用服务（拦截器，方法需 `virtual`）：

```csharp
public class SmsAppService : ApplicationService
{
    [OperationRateLimiting("SendSmsCode")]
    public virtual async Task SendCodeAsync([RateLimitingParameter] string phoneNumber)
    {
        await _smsSender.SendAsync(phoneNumber);
    }
}
```

MVC 控制器（引用 `LINGYUN.Abp.OperationRateLimiting.AspNetCore` 后自动注册 ActionFilter）：

```csharp
[HttpPost("send-sms-code")]
[OperationRateLimiting("SendSmsCode")]
public async Task<IActionResult> SendSmsCodeAsync([RateLimitingParameter] string phoneNumber)
{
    await _smsSender.SendAsync(phoneNumber);
    return Ok();
}
```

参数解析优先级：

1. 标记了 `[RateLimitingParameter]` 的方法参数（其 `ToString()` 作为分区值）；
2. 参数值实现 `IHasOperationRateLimitingParameter`（使用 `GetPartitionParameter()`）；
3. `null`（适用于 `PartitionByCurrentUser`、`PartitionByClientIp` 等）。

一个方法只允许标记一个 `[RateLimitingParameter]`，否则抛出 `AbpException`。特性可放在类上（作用于所有公开方法），方法级特性优先于类级特性。

## 异常与错误码

超限抛出 `AbpOperationRateLimitingException`：

* 继承 `BusinessException`，实现 `IHasHttpStatusCode`，HTTP 状态码 **429**。
* 错误码：`Volo.Abp.OperationRateLimiting:010001`（普通超限）、`Volo.Abp.OperationRateLimiting:010002`（永久封禁）。
* 策略/规则上的 `WithErrorCode()` 优先于默认错误码。
* `Data` 包含：`PolicyName`、`MaxCount`、`CurrentCount`、`RemainingCount`、`RetryAfterSeconds`、`RetryAfterMinutes`、`RetryAfter`（本地化文案，封禁时不存在）、`WindowDurationSeconds`、`WindowDescription`、`RuleDetails`，以及 `OperationRateLimitingContext.Properties` 的全部条目。

永久封禁（`maxCount: 0`）：

```csharp
options.AddPolicy("BlockedUser", policy =>
    policy.WithFixedWindow(TimeSpan.FromHours(24), 0).PartitionByParameter());
```

## 多租户

规则默认不隔离租户；调用 `WithMultiTenancy()` 后缓存键包含当前租户：

* 全局键：`orl:{PolicyName}:{RuleKey}:{PartitionKey}`
* 租户键：`orl:t:{TenantId}:{PolicyName}:{RuleKey}:{PartitionKey}`

`PartitionByCurrentTenant()` 本身就以租户为分区，无需再调用 `WithMultiTenancy()`。

> 分区值会原样出现在缓存键中。请勿使用密码等敏感值作为分区键；敏感标识请使用自定义解析器返回不可逆的稳定值。

## 动态策略仓储（数据库，位于 modules/operation-rate-limiting）

动态策略的实体、EF Core 仓储、应用服务与 REST API 位于 `modules/operation-rate-limiting`，基础库只保留 `IDynamicOperationRateLimitingPolicyStore` 抽象与合并逻辑。引用 `LINGYUN.Abp.OperationRateLimiting.EntityFrameworkCore` 并依赖 `AbpOperationRateLimitingEntityFrameworkCoreModule`。

使用模块自带的 `OperationRateLimitingDbContext`：

```csharp
Configure<AbpDbContextOptions>(options =>
{
    options.Configure<OperationRateLimitingDbContext>(context =>
    {
        context.DbContextOptions.UseSqlServer(connectionString);
    });
});
```

或加入现有 `DbContext`：

```csharp
public class MyDbContext : AbpDbContext<MyDbContext>, IOperationRateLimitingDbContext
{
    public DbSet<OperationRateLimitingPolicyRecord> OperationRateLimitingPolicies { get; set; }
    public DbSet<OperationRateLimitingRuleRecord> OperationRateLimitingPolicyRules { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ConfigureOperationRateLimiting();
    }
}
```

写入/更新策略：

```csharp
var record = new OperationRateLimitingPolicyRecord(GuidGenerator.Create(), "DynamicPolicy", CurrentTenant.Id);
record.SetRules(new[]
{
    new OperationRateLimitingRuleRecord(GuidGenerator.Create(), record.Id, OperationRateLimitingAlgorithms.FixedWindow)
    {
        Duration = TimeSpan.FromMinutes(1),
        MaxCount = 5,
        PartitionType = OperationRateLimitingPartitionType.Parameter
    }
});
await _policyRecordRepository.InsertAsync(record);

// 使缓存失效，下一次检查立即加载新策略
await _dynamicPolicyStore.InvalidateCacheAsync();
```

静态策略与数据库策略分别由 `IStaticOperationRateLimitingPolicyStore`、`IDynamicOperationRateLimitingPolicyStore` 提供，再由 `IOperationRateLimitingPolicyManager`（默认实现 `OperationRateLimitingPolicyManager`）通过 `LINGYUN.Abp.Dynamic.Definitions` 的 `DynamicDefinitionManager` 合并，默认策略为 `Merge`：

* `IStaticOperationRateLimitingPolicyStore` 默认实现为 `OptionsOperationRateLimitingPolicyStore`，从 `AbpOperationRateLimitingOptions.Policies` 读取；可替换为配置中心、文件等来源。
* `IDynamicOperationRateLimitingPolicyStore` 默认是空实现，引用管理模块的 EF Core 工程后由 `EfCoreOperationRateLimitingPolicyStore` 替换。

* 同名策略：数据库中的 `DisplayName`、`ErrorCode`、`IsEnabled` 覆盖静态值；数据库规则列表非空时整体替换规则。
* 仅数据库存在的策略直接生效。
* 合并策略可通过 `Configure<AbpDynamicDefinitionsOptions>(o => o.MapStrategy<OperationRateLimitingPolicy>(DynamicDefinitionStrategy.Dynamic))` 调整。

策略读取带 5 分钟绝对过期缓存（按租户隔离）。管理端修改数据库后请调用 `InvalidateCacheAsync`，或等待缓存过期。

## 扩展

### 自定义算法

```csharp
public class MyBurstRule : OperationRateLimitingRuleBase
{
    public override async Task<OperationRateLimitingStoreResult> ExecuteAsync(OperationRateLimitingRuleContext context)
    {
        // Duration 和 MaxCount 是公共属性，自定义算法可直接使用
        var maxCount = context.Rule.MaxCount;

        // 使用 context.Store 原子读写自定义状态；context.Consume 表示是否递增
        var state = await context.Store.UpdateStateAsync(
            context.Key,
            (MyState? current) => { /* ... */ return current; },
            TimeSpan.FromMinutes(10),
            context.CancellationToken);

        return OperationRateLimitingStoreResult.Allow(maxCount, 0);
    }
}
```

注册与使用：

```csharp
// 按名称注册（可持久化到数据库后按名称引用）
options.AddAlgorithm<MyBurstRule>("MyBurst");
options.AddPolicy("Policy", policy =>
    policy.AddRule(rule => rule
        .WithAlgorithm("MyBurst")
        .WithMaxCount(10)
        .WithDuration(TimeSpan.FromMinutes(1))
        .PartitionByParameter()));

// 或直接按类型添加到策略
policy.AddRule<MyBurstRule>(rule => rule
    .WithMaxCount(10)
    .WithDuration(TimeSpan.FromMinutes(1))
    .PartitionByParameter());
```

内置算法的参数请使用对应算法命名空间的扩展方法读写，例如：

```csharp
using LINGYUN.Abp.OperationRateLimiting.Algorithms.TokenBucket;

var tokens = rule.GetTokensPerPeriod();
rule.SetTokensPerPeriod(2);
```

自定义规则从 DI 按检查解析，因此可以使用 Scoped 服务。实现 `IOperationRateLimitingRuleReleaser` 可支持“许可释放”语义（参考并发算法）。

### 自定义存储

实现 `IOperationRateLimitingStore` 并注册替换默认实现。为让底层能够把整个算法作为一次原子操作执行，接口按“算法级操作”设计：`ExecuteAsync(OperationRateLimitingStoreRequest)` 执行四种内置算法，`UpdateStateAsync<TState>` 供自定义算法使用，`ReleaseAsync` 释放并发许可，`ResetAsync` 删除计数：

```csharp
[Dependency(ReplaceServices = true)]
public class MyStore : IOperationRateLimitingStore, ITransientDependency
{
    public Task<OperationRateLimitingStoreResult> ExecuteAsync(OperationRateLimitingStoreRequest request, CancellationToken ct = default) => ...;
    public Task ReleaseAsync(OperationRateLimitingStoreRequest request, CancellationToken ct = default) => ...;
    public Task<TState?> GetStateAsync<TState>(string key, CancellationToken ct = default) where TState : class, new() => ...;
    public Task<TState?> UpdateStateAsync<TState>(string key, Func<TState?, TState?> updater, TimeSpan expiration, CancellationToken ct = default) where TState : class, new() => ...;
    public Task ResetAsync(string key, CancellationToken ct = default) => ...;
}
```

自定义存储必须保证 `ExecuteAsync` 与 `UpdateStateAsync` 的原子性（分布式锁、Redis Lua 或数据库原子语句）；可参考 `StackExchangeRedisOperationRateLimitingStore`。

### 自定义格式化器

实现 `IOperationRateLimitingFormatter` 并注册替换，控制异常信息/`Data` 中时长文案（如“5 分钟”“2 小时 30 分钟”）。

### 自定义策略提供者

实现 `IOperationRateLimitingPolicyProvider` 并注册替换，可完全接管策略来源（默认实现基于静态/动态策略存储合并）。

### 自定义策略存储

* 静态策略：实现 `IStaticOperationRateLimitingPolicyStore` 并注册替换，可从配置中心、文件等来源加载；默认实现 `OptionsOperationRateLimitingPolicyStore` 读取 `AbpOperationRateLimitingOptions`。
* 动态策略：实现 `IDynamicOperationRateLimitingPolicyStore`（数据库、远程配置等），由 `AbpDynamicDefinitionsOptions.MapStrategy<OperationRateLimitingPolicy>()` 控制与静态策略的合并方式。

## 配置

```csharp
Configure<AbpOperationRateLimitingOptions>(options =>
{
    options.IsEnabled = true;                          // 全局开关
    options.LockTimeout = TimeSpan.FromSeconds(5);     // 计数更新分布式锁超时
});
```

全局关闭时：`CheckAsync`/`ResetAsync` 不执行任何操作，`IsAllowedAsync` 返回 `true`，`GetStatusAsync` 返回 `MaxCount = RemainingCount = int.MaxValue`、`CurrentCount = 0` 的放行结果。

开发环境关闭：

```csharp
var hostEnvironment = context.Services.GetHostingEnvironment();
Configure<AbpOperationRateLimitingOptions>(options =>
{
    if (hostEnvironment.IsDevelopment())
    {
        options.IsEnabled = false;
    }
});
```

## 部署注意事项

* **单机默认可用**：默认使用 ABP 的 `LocalAbpDistributedLock`（进程内键控锁），单实例无需额外配置。
* **多实例必须替换为真正的分布式锁**（Redis / Medallion / 数据库锁等），否则跨实例的原子性无法保证。
* **多实例请使用分布式缓存**（如 Redis）存储计数，`AbpCachingModule` 默认的内存缓存只在单实例内一致。
* 数据库表：`OperationRateLimitingPolicies`（策略）与 `OperationRateLimitingPolicyRules`（规则，`Properties` 为 JSON 列）；请按项目使用的数据库生成迁移。
* 策略缓存按租户隔离；`InvalidateCacheAsync` 只清理当前租户的缓存。

## 与设计文档的差异

* 错误码常量值、HTTP 429、异常数据键、四种算法、分区类型、多规则与声明式用法均与文档一致。
* 为让存储层能够原子执行四种算法，`IOperationRateLimitingStore` 采用算法级 `ExecuteAsync(OperationRateLimitingStoreRequest)` + 自定义算法 `UpdateStateAsync` 的接口（文档中的 `IncrementAsync(key, duration, maxCount)` 仅能表达固定窗口）。默认实现用分布式锁，Redis 实现用 Lua，均不需要调用方加锁。
* 文档中的“动态策略仓储”只描述了 `AddPolicy(OperationRateLimitingPolicy)` 与自定义 `IOperationRateLimitingPolicyProvider`；本模块把实体、EF Core 仓储、应用服务、REST API 拆到 `modules/operation-rate-limiting`，并复用 `LINGYUN.Abp.Dynamic.Definitions` 的合并策略。
