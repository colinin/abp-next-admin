# LINGYUN.Abp.AuditLogging.OpenObserve

[English](./README.EN.md) | 简体中文

## 简介

将审计日志、安全日志写入 [OpenObserve](https://openobserve.ai/) 并通过 OpenObserve SQL 查询，
可作为 `LINGYUN.Abp.AuditLogging.Elasticsearch` 的替代实现（`IAuditLogWriter` / `ISecurityLogWriter` /
`IAuditLogManager` / `ISecurityLogManager` / `IEntityChangeStore` 全部被替换）。

## 配置

`OpenObserve` 为连接配置（由 `LINGYUN.Abp.OpenObserve` 提供），审计相关配置节为 `AuditLogging:OpenObserve`：

```json
{
  "OpenObserve": {
    "Endpoint": "http://localhost:5080",
    "Organization": "default",
    "AccessToken": "Basic YWRtaW5AYWJwLmlvOnd3MVo1TCU2"
  },
  "AuditLogging": {
    "OpenObserve": {
      "Organization": null,
      "AuditLogStream": "audit_log",
      "SecurityLogStream": "security_log",
      "WriteEntityChanges": true,
      "WriteActions": true,
      "DefaultQueryDays": 365,
      "UseLogTimeAsTimestamp": true,
      "TimestampAllowedUptoHours": 5
    }
  }
}
```

| 配置项 | 默认值 | 说明 |
| --- | --- | --- |
| `Organization` | 空 | 数据流所属组织, 为空时使用 `OpenObserve:Organization` |
| `AuditLogStream` | `audit_log` | 审计日志主数据流（根），子数据流由它计算 |
| `SecurityLogStream` | `security_log` | 安全日志数据流 |
| `WriteEntityChanges` | `true` | 是否展平写入实体变更（关闭后 `IEntityChangeStore` 查不到数据） |
| `WriteActions` | `true` | 是否展平写入操作（关闭后 `Actions.*` 条件无法匹配） |
| `DefaultQueryDays` | `365` | 未指定时间范围时的默认查询跨度(天) |
| `UseLogTimeAsTimestamp` | `true` | 是否用日志业务时间覆盖 `_timestamp`（见注意事项 2） |
| `TimestampAllowedUptoHours` | `5` | 允许覆盖 `_timestamp` 的最大回溯小时数，需与服务端 `ZO_INGEST_ALLOWED_UPTO` 一致 |

> 子数据流名称**由 `AuditLogStream` 计算**（`AbpAuditLoggingOpenObserveOptions.GetEntityChangeStream()` /
> `GetActionStream()`），不单独配置：实体变更 = `{AuditLogStream}_entity_changes`、操作 = `{AuditLogStream}_actions`
> （默认即 `audit_log_entity_changes`、`audit_log_actions`）。

## 使用方式

```csharp
[DependsOn(typeof(AbpAuditLoggingOpenObserveModule))]
public class MyModule : AbpModule
{
}
```

模块**不对数据流做任何处理**（不探测、不创建、不修改设置）：OpenObserve 会在首次写入时自动创建数据流，
保留策略等数据流设置请在 OpenObserve 侧配置（或自行调用 `UpdateStreamSettingsAsync`）。

## 多租户

数据流按租户隔离（与 Elasticsearch 实现一致，参考其 `IIndexNameNormalizer`）：租户数据流形如
`audit_log_{tenantId:N}`（实体变更 / 操作分别为 `audit_log_entity_changes_{tenantId:N}`、`audit_log_actions_{tenantId:N}`），
宿主（未指定租户）使用基础名称。

* **写入**：按**日志自身的 `TenantId`** 选择数据流（审计日志写入发生在后台队列，当前租户可能已不是产生日志的租户），
  批量写入按租户分组；
* **查询**：按**当前租户**（`ICurrentTenant`）选择数据流，因此租户之间天然隔离；
* 命名规则可通过 `IStreamNameNormalizer`（默认 `StreamNameNormalizer`）覆写；
  实测 OpenObserve 会把流名中的 `-` 规范化为 `_`，因此默认分隔符使用 `_`；
* 模块不对数据流做任何处理：数据流在首次写入时由 OpenObserve 自动创建，
  保留策略等设置请在 OpenObserve 侧配置。

## 数据流结构

审计日志侧共三个数据流（后两个名称由 `AuditLogStream` 计算）：

| 数据流 | 内容 |
| --- | --- |
| `audit_log` | 审计日志头部 |
| `audit_log_entity_changes` | 展平的实体变更 |
| `audit_log_actions` | 展平的操作（`parameters` 可能很大，因此独立存放） |

```sql
select * from audit_log where httpmethod = 'POST' and url like '%/api/identity%' order by executiontime desc
```

实体变更数据流字段：

| 列 | 说明 |
| --- | --- |
| `id` / `auditlogid` | 实体变更 Id / 所属审计日志 Id |
| `tenantid` / `entitytenantid` | 租户 |
| `changetime` / `changetype` | 变更时间 / `Created`、`Updated`、`Deleted` |
| `entityid` / `entitytypefullname` | 实体标识 |
| `userid` / `username` | 操作人（冗余自审计日志，免关联） |
| `propertychanges` | 属性变更明细（JSON 字符串） |
| `_timestamp` | 文档时间（默认由 `changetime` 覆盖） |

操作数据流字段：

| 列 | 说明 |
| --- | --- |
| `id` / `auditlogid` | 操作 Id / 所属审计日志 Id |
| `servicename` / `methodname` | 服务与方法 |
| `parameters` | 操作参数（可能很大，用 `like` 或 `str_match` 检索） |
| `executiontime` / `executionduration` | 执行时间 / 耗时 |
| `userid` / `username` | 操作人（冗余自审计日志） |
| `_timestamp` | 文档时间（默认由 `executiontime` 覆盖） |

`Actions.*` 条件采用**两段式查询**：

```sql
-- 1) 先在操作数据流匹配
select distinct auditlogid from audit_log_actions where parameters like '%关键字%'
-- 2) 再回主数据流按 id 过滤
select * from audit_log where id in (...)
```

## 注意事项

1. **数据流列名一律小写**：OpenObserve 会把字段名小写化（`httpMethod` → `httpmethod`），
   因此 SQL 条件使用小写列名，参见各管理器中的 `MapedFields`；
2. **`_timestamp` 按业务时间覆盖**：写入时用 `AuditLog.ExecutionTime` / `SecurityLog.CreationTime`
   （实体变更用 `ChangeTime`、操作用 `ExecutionTime`）覆盖 `_timestamp`（`UseLogTimeAsTimestamp`，默认开启），
   这样查询的 `start_time` / `end_time` 才对应业务时间。但 OpenObserve 默认只接受
   `ZO_INGEST_ALLOWED_UPTO`（默认 5）小时内的数据，**超出窗口的记录会被服务端直接丢弃**
   （`Too old data … Data discarded`），因此超出 `TimestampAllowedUptoHours` 的日志不再覆盖
   `_timestamp`（改用服务端写入时间）；如需写入更早的日志，请调大服务端环境变量并同步该配置；
3. **时间过滤**：查询默认使用宽窗口（`DefaultQueryDays`），业务时间过滤通过 `executiontime` /
   `creationtime` / `changetime` 列的 ISO 8601 字符串比较完成（覆盖受 5 小时窗口限制时的兜底）；
4. **不支持删除记录**：OpenObserve 是 append-only，既不支持 SQL `DELETE`（`DELETE not supported for Base table`）
   也不支持原地 `UPDATE`，因此 `IAuditLogManager.DeleteAsync` / `DeleteManyAsync`（安全日志同理）会直接抛出业务异常
   `AuditLoggingOpenObserveErrorCodes.DeleteNotSupported`（错误码 `AuditLoggingOpenObserve:010001`，
   本地化文案：**当前日志设施不支持删除记录!**）。数据清理请在 OpenObserve 侧配置保留策略，或使用其按时间范围删除接口；
5. **操作与实体变更写入独立数据流**：主文档不再保存 `actions` / `entitychanges` 嵌套数组（OpenObserve 只能把它们
   存成 JSON 字符串列，无法结构化检索）。`x => x.Actions.Any(a => a.Parameters.Contains("..."))` 走两段式查询；
   `includeDetails` 时按 `auditlogid in (…)` 从两个子数据流装配详情。`parameters` 很大时建议改用
   `str_match(parameters, '值')` 并给子数据流配置 `full_text_search_keys`，避免 `like` 全列扫描。
   实体变更明细（`propertychanges`）同样以 JSON 字符串存储：转换逻辑集中在 `OpenObserveEntityChangeConverter`，
   序列化通过 `IAuditLoggingOpenObserveSerializer` 完成（默认实现委托 ABP 的 `IJsonSerializer`，
   实体本身不直接使用 `JsonSerializer`）；需要为特定类型定制 JSON 行为时，使用
   `AbpSystemTextJsonSerializerModifiersOptions` 注册修饰器；
   数据流尚未创建（还没写入过数据）时查询按空结果处理，不会报错；
6. **字符串查询为模糊匹配**：`url` / `userName` / `applicationName` 等参数使用 `like '%值%'`，
   `correlationId` / `clientId` / `clientIpAddress` / `httpMethod` / `action` 为精确匹配；
7. **字段映射**：表达式成员名（PascalCase）映射为小写数据流列名；
   `LINGYUN.Abp.OpenObserve` 包提供 `ExpressionParser`（支持 `Any(集合成员)` 表达式）与
   `OpenObserveSqlBuilder` 供 SQL 条件构建复用。

## 相关项目

* [LINGYUN.Abp.AuditLogging](../LINGYUN.Abp.AuditLogging/README.md)
* [LINGYUN.Abp.OpenObserve](../../open-observe/LINGYUN.Abp.OpenObserve/README.md)
* [LINGYUN.Abp.Logging.Serilog.OpenObserve](../../logging/LINGYUN.Abp.Logging.Serilog.OpenObserve/README.md)
