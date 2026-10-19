# LINGYUN.Abp.OpenObserve

简体中文 | [English](./README.EN.md)

OpenObserve HTTP API 客户端封装, 覆盖官方 API 参考文档中的全部接口

参考文档: https://openobserve.ai/docs/reference/api/

## 模块引用

```csharp
[DependsOn(typeof(AbpOpenObserveModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 配置项

| 配置项 | 说明 | 默认值 |
| --- | --- | --- |
| `Endpoint` | 服务地址 | `http://localhost:5080` |
| `Organization` | 默认组织名称, 调用接口未显式指定组织时使用 | `default` |
| `UserName` | 用户名(与 Password 一起构造 Basic 认证头) | - |
| `Password` | 密码 | - |
| `AccessToken` | 完整的授权头, 如 `Basic xxx` 或 `Bearer xxx`(服务账号令牌), 优先于 UserName/Password | - |
| `TimeoutSeconds` | 请求超时时间(秒) | `600` |

> 所有接口都需要授权头, 官方推荐 `Authorization: Basic base64(email:password)`。
> 注意: **摄取 token 只能用于摄取接口**, 不能用于 `_meta` / `_search` / `/mcp` 等 API, 否则返回 `401 Unauthorized`。
> `TimeoutSeconds` 默认 600 秒, 与搜索接口的 `ZO_QUERY_TIMEOUT` 保持一致, 避免长时间查询被 HttpClient 中断。

## appsettings.json

```json
{
  "OpenObserve": {
    "Endpoint": "http://localhost:5080",
    "Organization": "default",
    "UserName": "admin@abp.io",
    "Password": "your-password",
    "TimeoutSeconds": 600
  }
}
```

## 接口清单

### Stream

| 方法 | HTTP | 路径 |
| --- | --- | --- |
| `GetStreamsAsync` | GET | `/api/{organization}/streams` |
| `GetStreamSchemaAsync` | GET | `/api/{organization}/streams/{stream}/schema` |
| `CreateStreamSettingsAsync` | POST | `/api/{org_id}/streams/{stream_name}/settings` |
| `UpdateStreamSettingsAsync` | PUT | `/api/{org_id}/streams/{stream_name}/settings` |
| `DeleteStreamAsync` | DELETE | `/api/{org_id}/streams/{stream_name}` |
| `DeleteStreamDataByTimeRangeAsync` | DELETE | `/api/{org_id}/streams/{stream_name}/data_by_time_range` |
| `GetStreamDataByTimeRangeStatusAsync` | GET | `/api/{org_id}/streams/{stream_name}/data_by_time_range/status/{id}` |
| `DeleteStreamCacheAsync` | DELETE | `/api/{org_id}/streams/{stream_name}/cache/results` |

### Ingestion

| 方法 | HTTP | 路径 | 说明 |
| --- | --- | --- | --- |
| `JsonRequestAsync` | POST | `/api/{organization}/{stream}/_json` | JSON 数组 |
| `JsonRecordAsync` | POST | `/api/{organization}/{stream}/_json` | 单条记录(内部包装为数组) |
| `JsonBulkAsync` | POST | `/api/{organization}/_bulk` | NDJSON, 动作行 + 记录行, 兼容 Elasticsearch _bulk |
| `JsonMultiAsync` | POST | `/api/{organization}/{stream}/_multi` | NDJSON, 每行一条记录 |

> `_bulk` 支持 `index` / `create` / `update` 三种动作, 不支持 `delete`。
> 记录中的 `_timestamp`(微秒, 或 RFC 3339/RFC 2822 字符串) 可覆盖默认摄取时间; 不传时使用当前时间。
> 摄取响应中的 `status[].error` 会给出失败原因(如 `flatten value must be an object`), 可通过 `JsonStatus.Error` 读取。

### Search

| 方法 | HTTP | 路径 |
| --- | --- | --- |
| `SearchAsync` | POST | `/api/{organization}/_search` |
| `AroundAsync` | GET | `/api/{organization}/{stream}/_around` |
| `GetValuesAsync` | GET | `/api/{organization}/{stream}/_values` |

> 时间范围单位为**微秒**, 必须提供 `start_time` / `end_time`, 否则会全量扫描。
> 搜索错误码(20001-20013)与 `hint` / `suggestions` 由 `OpenObserveSearchException` 承载。

### Function

| 方法 | HTTP | 路径 |
| --- | --- | --- |
| `CreateFunctionAsync` | POST | `/api/{organization}/functions` |
| `UpdateFunctionAsync` | PUT | `/api/{organization}/functions/{name}` |
| `DeleteFunctionAsync` | DELETE | `/api/{organization}/functions/{name}` |
| `GetFunctionsAsync` | GET | `/api/{organization}/functions` |
| `TestFunctionAsync` | POST | `/api/{org_id}/functions/test` |

> 函数语言由请求体 `transType` 决定(`FunctionTransType.Vrl` / `JavaScript`), 没有 `type` 查询参数。
> `TestFunctionAsync` 未出现在 API 文档中, 来自实现源码。

### User

| 方法 | HTTP | 路径 |
| --- | --- | --- |
| `CreateUserAsync` | POST | `/api/{organization}/users` |
| `UpdateUserAsync` | PUT | `/api/{organization}/users/{user_email}` |
| `AddUserToOrganizationAsync` | POST | `/api/{organization}/users/{user_email}` |
| `RemoveUserFromOrganizationAsync` | DELETE | `/api/{organization}/users/{user_email}` |
| `GetUsersAsync` | GET | `/api/{organization}/users` |

### Metrics 与 Cluster

| 方法 | HTTP | 路径 |
| --- | --- | --- |
| `GetMetricsAsync` | GET | `/metrics` (Prometheus 文本格式, 需 `ZO_PROMETHEUS_ENABLE=true`) |
| `GetClusterInfoAsync` | GET | `/api/{org_id}/cluster/info` |

### Dashboard

| 方法 | HTTP | 路径 |
| --- | --- | --- |
| `AddDashboardPanelAsync` | POST | `/api/{org_id}/dashboards/{dashboard_id}/panels` |
| `UpdateDashboardPanelAsync` | PUT | `/api/{org_id}/dashboards/{dashboard_id}/panels/{panel_id}` |
| `DeleteDashboardPanelAsync` | DELETE | `/api/{org_id}/dashboards/{dashboard_id}/panels/{panel_id}` |

> 面板操作仅支持 v8 仪表盘, 且 `hash` 参数必填(乐观并发控制), 不匹配时返回 409。

### Report

| 方法 | HTTP | 路径 |
| --- | --- | --- |
| `GetReportsAsync` | GET | `/api/v2/{org_id}/reports` |
| `CreateReportAsync` | POST | `/api/v2/{org_id}/reports` |
| `GetReportAsync` | GET | `/api/v2/{org_id}/reports/{report_id}` |
| `UpdateReportAsync` | PUT | `/api/v2/{org_id}/reports/{report_id}` |
| `DeleteReportAsync` | DELETE | `/api/v2/{org_id}/reports/{report_id}` |
| `BulkDeleteReportsAsync` | DELETE | `/api/v2/{org_id}/reports/bulk` |
| `MoveReportsAsync` | PATCH | `/api/v2/{org_id}/reports/move` |
| `EnableReportAsync` | PATCH | `/api/v2/{org_id}/reports/{report_id}/enable` |
| `TriggerReportAsync` | PUT | `/api/v2/{org_id}/reports/{report_id}/trigger` |

> 报表接口使用 `/api/v2` 前缀, 与其它接口不同。

## 调用风格

* **只需传递业务参数**, 无需自行组装 `*Request` 模型(请求体由客户端内部组装), 例如:

```csharp
var client = ServiceProvider.GetRequiredService<OpenObserveClient>();

// 新建用户
await client.CreateUserAsync("user@abp.io", "complex#pass", firstName: "ming", role: "admin");

// 写入一条审计数据
await client.JsonRecordAsync("default", "audit-log", new { action = "Login", user = "admin" });

// 搜索
var result = await client.SearchAsync<JsonNode>(
    new SearchQuery("SELECT * FROM audit-log", DateTime.UtcNow.AddHours(-1), DateTime.UtcNow, size: 20));
```

* 所有序列化/反序列化统一通过 **`IOpenObserveSerializer`** 完成, 默认实现 `DefaultOpenObserveSerializer` 基于 ABP 的 `IJsonSerializer`:
  * 遵循宿主配置的 `AbpSystemTextJsonSerializerOptions`(转换器、日期格式、编码器等); ABP 默认将编码器设为 `UnsafeRelaxedJsonEscaping`, 中文与引号不会被转义为 `\uXXXX`;
  * 字段名以模型上的 `JsonPropertyName` 为准(注意 `IJsonSerializer.Serialize` 的 `camelCase` 默认值只影响未标注特性的属性);
  * OpenObserve 特有的"序列化时忽略 null 字段"通过 ABP 的**类型级序列化修饰器**实现
    (`AbpSystemTextJsonSerializerModifiersOptions.Modifiers`: `List<Action<JsonTypeInfo>>` → `OpenObserveIgnoreNullPropertiesModifier`),
    仅作用于 `LINGYUN.Abp.OpenObserve.Models` 命名空间: 可选参数不传时不会产生 `null` 字段(例如用户密码字段, 文档要求"不修改时不要传值");
  * 说明: `IJsonSerializer` 接口(ABP 10.x)只有 `Serialize(object, camelCase, indented)` / `Deserialize<T>(string, camelCase)` /
    `Deserialize(Type, string, camelCase)`, **不接受自定义 `JsonSerializerOptions`**; 需要按类型定制序列化时应使用上述修饰器, 例如:

```csharp
Configure<AbpSystemTextJsonSerializerModifiersOptions>(options =>
{
    options.Modifiers.Add(jsonTypeInfo =>
    {
        if (jsonTypeInfo.Type == typeof(MyType))
        {
            foreach (var property in jsonTypeInfo.Properties)
            {
                property.ShouldSerialize = (_, value) => value != null;
            }
        }
    });
});
```

* 如需整体替换序列化方式, 实现 `IOpenObserveSerializer` 并加上 `[Dependency(ReplaceServices = true)]` 覆盖默认实现。
* 组织名称参数可省略, 默认取 `AbpOpenObserveOptions.Organization`; 集群信息接口默认使用 `_meta` 组织(服务端限制)。
* 数据模型按**一个类型一个文件**组织, 目录结构与 `Models/Search` 保持一致。

## 单元测试

测试项目: `aspnet-core/tests/LINGYUN.Abp.OpenObserve.Tests`(已加入 `LINGYUN.MicroService.All` 解决方案)

```bash
dotnet test aspnet-core/tests/LINGYUN.Abp.OpenObserve.Tests/LINGYUN.Abp.OpenObserve.Tests.csproj
```

测试通过 `HttpMessageHandler` 拦截请求, 校验每个接口的 **HTTP 方法 / 路径 / 查询参数 / 请求体**, 以及响应反序列化与异常类型, 不产生真实网络请求。

## 异常处理

| 异常 | 说明 |
| --- | --- |
| `OpenObserveException` | 业务异常基类, 携带 `Code` / `StatusCode` / `Error` / `ResponseBody` |
| `OpenObserveRequestException` | 响应体无法解析为 OpenObserve 标准错误结构 |
| `OpenObserveSearchException` | 搜索接口业务错误(含错误码、`hint`、`suggestions`) |
| `OpenObserveIngestionException` | 摄取接口业务错误(含数据流名称) |

## 文档与实现的差异

实现过程中发现的文档与实现不一致之处, 代码以**实际可用的行为**为准:

1. **Cluster**: 文档为 `GET /api/{org_id}/cluster_info`, 实现为 `GET /api/{org_id}/cluster/info`, 默认使用后者, 可通过重写 `CreateClusterInfoRequestUri` 调整。
2. **Metrics**: 文档正文为 `GET /metrics`, 但示例写成 `PUT`, 实际只注册了 GET。
3. **Function 列表响应**: 文档字段表使用 `stream_name` / `order` / `num_args` / `trans_type`, 实现返回 camelCase(`numArgs` / `transType` / `streams[]`), 模型以实现为准并用扩展属性保留未声明字段。
4. **函数 `order` 字段**: 文档示例包含, 当前实现不接受(仅 `streams[].order` 生效)。
5. **Dashboard**: 文档只描述了面板级接口, 仪表盘本身的增删改查/移动/文件夹未在文档中给出(官方说明见实例内置的 OpenAPI/Swagger)。
6. **Ingestion 的 Content-Type**: 文档未声明, `_json` 使用 `application/json`, `_bulk` / `_multi` 使用 `application/x-ndjson`。
7. **搜索 `sort_by`**: 不是 `_search` 的字段(仅多流搜索支持), 排序请使用 SQL `ORDER BY`。
8. **`use_cache` / `clear_cache`**: 查询字符串中的值会覆盖请求体, 默认 `use_cache=true`。
