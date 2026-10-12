# LINGYUN.Abp.AuditLogging.OpenObserve

English | [简体中文](./README.md)

## Introduction

Writes audit logs and security logs to [OpenObserve](https://openobserve.ai/) and queries them with
OpenObserve SQL. It can be used as an alternative to `LINGYUN.Abp.AuditLogging.Elasticsearch`
(`IAuditLogWriter`, `ISecurityLogWriter`, `IAuditLogManager`, `ISecurityLogManager` and
`IEntityChangeStore` are all replaced).

## Configuration

`OpenObserve` holds the connection settings (provided by `LINGYUN.Abp.OpenObserve`) and the audit
specific section is `AuditLogging:OpenObserve`:

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

| Option | Default | Description |
| --- | --- | --- |
| `Organization` | null | Stream organization, falls back to `OpenObserve:Organization` |
| `AuditLogStream` | `audit_log` | Root audit log stream; child streams are derived from it |
| `SecurityLogStream` | `security_log` | Security log stream |
| `WriteEntityChanges` | `true` | Flatten entity changes (disabling it makes `IEntityChangeStore` return nothing) |
| `WriteActions` | `true` | Flatten actions (disabling it makes `Actions.*` conditions unmatchable) |
| `DefaultQueryDays` | `365` | Default query range in days when no time range is given |
| `UseLogTimeAsTimestamp` | `true` | Overwrite `_timestamp` with the log time (see note 2) |
| `TimestampAllowedUptoHours` | `5` | Maximum age for the `_timestamp` override; must match `ZO_INGEST_ALLOWED_UPTO` |

> Child stream names are **derived from `AuditLogStream`**
> (`AbpAuditLoggingOpenObserveOptions.GetEntityChangeStream()` / `GetActionStream()`) and are not
> configurable: entity changes = `{AuditLogStream}_entity_changes`, actions = `{AuditLogStream}_actions`
> (defaults: `audit_log_entity_changes`, `audit_log_actions`).

## Usage

```csharp
[DependsOn(typeof(AbpAuditLoggingOpenObserveModule))]
public class MyModule : AbpModule
{
}
```

The module **does not touch the streams at all** (no probing, no creation, no settings): OpenObserve creates a
stream on the first ingestion, and retention or other stream settings must be configured on the OpenObserve
side (or by calling `UpdateStreamSettingsAsync` yourself).

## Multi-tenancy

Streams are isolated per tenant (mirroring the Elasticsearch implementation and its `IIndexNameNormalizer`):
tenant streams look like `audit_log_{tenantId:N}` (`audit_log_entity_changes_{tenantId:N}` and
`audit_log_actions_{tenantId:N}` for the child streams) while the host (no tenant) uses the base names.

* **Writes** resolve the stream from the **log's own `TenantId`** (audit logs are written by a background
  queue where the ambient tenant may no longer be the one that produced the log); bulk writes are grouped by
  tenant;
* **Queries** resolve the stream from the **current tenant** (`ICurrentTenant`), so tenants are isolated;
* The naming rule can be replaced by overriding `IStreamNameNormalizer` (default `StreamNameNormalizer`).
  OpenObserve normalizes `-` to `_` in stream names (verified empirically), hence `_` is used as the separator;
* The module does not touch the streams: they are created by OpenObserve on the first write, and retention
  settings must be configured on the OpenObserve side.

## Stream layout

There are three streams on the audit side (the last two names are derived from `AuditLogStream`):

| Stream | Content |
| --- | --- |
| `audit_log` | Audit log header |
| `audit_log_entity_changes` | Flattened entity changes |
| `audit_log_actions` | Flattened actions (`parameters` can be large, hence the separate stream) |

```sql
select * from audit_log where httpmethod = 'POST' and url like '%/api/identity%' order by executiontime desc
```

Entity change stream columns:

| Column | Description |
| --- | --- |
| `id` / `auditlogid` | Entity change id / owning audit log id |
| `tenantid` / `entitytenantid` | Tenant |
| `changetime` / `changetype` | Change time / `Created`, `Updated`, `Deleted` |
| `entityid` / `entitytypefullname` | Entity identity |
| `userid` / `username` | Operator (denormalized from the audit log) |
| `propertychanges` | Property change details (JSON string) |
| `_timestamp` | Document time (overridden by `changetime` by default) |

Action stream columns:

| Column | Description |
| --- | --- |
| `id` / `auditlogid` | Action id / owning audit log id |
| `servicename` / `methodname` | Service and method |
| `parameters` | Action parameters (can be large; search with `like` or `str_match`) |
| `executiontime` / `executionduration` | Execution time / duration |
| `userid` / `username` | Operator (denormalized from the audit log) |
| `_timestamp` | Document time (overridden by `executiontime` by default) |

`Actions.*` conditions use a **two-step query**:

```sql
-- 1) match in the actions stream first
select distinct auditlogid from audit_log_actions where parameters like '%keyword%'
-- 2) then filter the main stream by id
select * from audit_log where id in (...)
```

## Notes

1. **Stream columns are always lowercase**: OpenObserve lowercases field names (`httpMethod` becomes
   `httpmethod`), so SQL conditions use lowercase column names (see `MapedFields` in each manager);
2. **`_timestamp` is overwritten with the log time**: writes use `AuditLog.ExecutionTime` /
   `SecurityLog.CreationTime` (entity changes use `ChangeTime`, actions use `ExecutionTime`) so that the
   query `start_time` / `end_time` correspond to business time (`UseLogTimeAsTimestamp`, enabled by default).
   However OpenObserve only accepts data within `ZO_INGEST_ALLOWED_UPTO` (5 by default) hours and **discards**
   anything older (`Too old data … Data discarded`), so logs older than `TimestampAllowedUptoHours` keep the
   server ingestion time instead. Raise the server environment variable and this option together if you need
   to write older logs;
3. **Time filtering**: queries use a wide window (`DefaultQueryDays`) and filter business time with ISO 8601
   string comparisons on the `executiontime` / `creationtime` / `changetime` columns (the fallback when the
   `_timestamp` override is outside the allowed window);
4. **Deleting records is not supported**: OpenObserve is append-only and supports neither SQL `DELETE`
   (`DELETE not supported for Base table`) nor in-place `UPDATE`, so `IAuditLogManager.DeleteAsync` /
   `DeleteManyAsync` (and the security log equivalents) throw the business exception
   `AuditLoggingOpenObserveErrorCodes.DeleteNotSupported` (code `AuditLoggingOpenObserve:010001`,
   localized text: **The current logging facility does not support deleting records!**). Configure retention
   on the OpenObserve side or use its time-range delete API to purge data;
5. **Actions and entity changes live in dedicated streams**: the main document no longer carries the
   `actions` / `entitychanges` nested arrays (OpenObserve can only store them as JSON string columns that
   cannot be queried structurally). `x => x.Actions.Any(a => a.Parameters.Contains("..."))` uses the two-step
   query above, and `includeDetails` assembles the details from both child streams with `auditlogid in (...)`.
   For large `parameters`, prefer `str_match(parameters, 'value')` with `full_text_search_keys` configured on
   the child stream instead of a full-column `like`. Entity change details (`propertychanges`) are stored as a
   JSON string as well: the conversion lives in `OpenObserveEntityChangeConverter` and serialization goes
   through `IAuditLoggingOpenObserveSerializer` (the default implementation delegates to ABP's
   `IJsonSerializer`, so entities never use `JsonSerializer` directly); use
   `AbpSystemTextJsonSerializerModifiersOptions` to customize JSON behaviour for specific types. Queries
   against a stream that has not been created yet return an empty result instead of failing;
6. **String filters are fuzzy**: `url`, `userName`, `applicationName` use `like '%value%'` while
   `correlationId`, `clientId`, `clientIpAddress`, `httpMethod` and `action` match exactly;
7. **Field mapping**: expression member names (PascalCase) map to lowercase stream columns.
   `ExpressionParser` (which supports `Any(member)` expressions) and `OpenObserveSqlBuilder` live in
   `LINGYUN.Abp.OpenObserve` so that any consumer can build SQL conditions.

## Related projects

* [LINGYUN.Abp.AuditLogging](../LINGYUN.Abp.AuditLogging/README.EN.md)
* [LINGYUN.Abp.OpenObserve](../../open-observe/LINGYUN.Abp.OpenObserve/README.EN.md)
* [LINGYUN.Abp.Logging.Serilog.OpenObserve](../../logging/LINGYUN.Abp.Logging.Serilog.OpenObserve/README.EN.md)
