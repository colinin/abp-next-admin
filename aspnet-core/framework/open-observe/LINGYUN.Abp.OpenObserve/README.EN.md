# LINGYUN.Abp.OpenObserve

English | [简体中文](./README.md)

HTTP API client for OpenObserve, covering all endpoints of the official API reference

Reference: https://openobserve.ai/docs/reference/api/

## Module reference

```csharp
[DependsOn(typeof(AbpOpenObserveModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Options

| Option | Description | Default |
| --- | --- | --- |
| `Endpoint` | Service address | `http://localhost:5080` |
| `Organization` | Default organization used when a call does not specify one | `default` |
| `UserName` | User name (used with Password to build the Basic header) | - |
| `Password` | Password | - |
| `AccessToken` | Full authorization header, e.g. `Basic xxx` or `Bearer xxx` (service account token); takes precedence over UserName/Password | - |
| `TimeoutSeconds` | Request timeout in seconds | `600` |

> Every API requires an authorization header; the official recommendation is `Authorization: Basic base64(email:password)`.
> Note: an **ingestion token only works for ingestion endpoints** and returns `401 Unauthorized` on `_meta` / `_search` / `/mcp`.
> `TimeoutSeconds` defaults to 600 seconds, matching the search API's `ZO_QUERY_TIMEOUT`, so long running queries are not aborted by HttpClient.

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

## Endpoints

### Stream

| Method | HTTP | Path |
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

| Method | HTTP | Path | Notes |
| --- | --- | --- | --- |
| `JsonRequestAsync` | POST | `/api/{organization}/{stream}/_json` | JSON array |
| `JsonRecordAsync` | POST | `/api/{organization}/{stream}/_json` | Single record (wrapped into an array internally) |
| `JsonBulkAsync` | POST | `/api/{organization}/_bulk` | NDJSON, action line + record line, Elasticsearch `_bulk` compatible |
| `JsonMultiAsync` | POST | `/api/{organization}/{stream}/_multi` | NDJSON, one record per line |

> `_bulk` supports `index` / `create` / `update`; `delete` is not supported.
> `_timestamp` (microseconds, or an RFC 3339/RFC 2822 string) in a record overrides the default ingestion time.
> A failed record reports the reason in `status[].error` (e.g. `flatten value must be an object`), available via `JsonStatus.Error`.

### Search

| Method | HTTP | Path |
| --- | --- | --- |
| `SearchAsync` | POST | `/api/{organization}/_search` |
| `AroundAsync` | GET | `/api/{organization}/{stream}/_around` |
| `GetValuesAsync` | GET | `/api/{organization}/{stream}/_values` |

> Time ranges are in **microseconds** and `start_time` / `end_time` are required, otherwise the whole dataset is scanned.
> Search error codes (20001-20013) plus `hint` / `suggestions` are surfaced through `OpenObserveSearchException`.

### Function

| Method | HTTP | Path |
| --- | --- | --- |
| `CreateFunctionAsync` | POST | `/api/{organization}/functions` |
| `UpdateFunctionAsync` | PUT | `/api/{organization}/functions/{name}` |
| `DeleteFunctionAsync` | DELETE | `/api/{organization}/functions/{name}` |
| `GetFunctionsAsync` | GET | `/api/{organization}/functions` |
| `TestFunctionAsync` | POST | `/api/{org_id}/functions/test` |

> The language is selected by the request body field `transType` (`FunctionTransType.Vrl` / `JavaScript`); there is no `type` query parameter.
> `TestFunctionAsync` is not part of the API docs and comes from the implementation sources.

### User

| Method | HTTP | Path |
| --- | --- | --- |
| `CreateUserAsync` | POST | `/api/{organization}/users` |
| `UpdateUserAsync` | PUT | `/api/{organization}/users/{user_email}` |
| `AddUserToOrganizationAsync` | POST | `/api/{organization}/users/{user_email}` |
| `RemoveUserFromOrganizationAsync` | DELETE | `/api/{organization}/users/{user_email}` |
| `GetUsersAsync` | GET | `/api/{organization}/users` |

### Metrics and Cluster

| Method | HTTP | Path |
| --- | --- | --- |
| `GetMetricsAsync` | GET | `/metrics` (Prometheus text format, requires `ZO_PROMETHEUS_ENABLE=true`) |
| `GetClusterInfoAsync` | GET | `/api/{org_id}/cluster/info` |

### Dashboard

| Method | HTTP | Path |
| --- | --- | --- |
| `AddDashboardPanelAsync` | POST | `/api/{org_id}/dashboards/{dashboard_id}/panels` |
| `UpdateDashboardPanelAsync` | PUT | `/api/{org_id}/dashboards/{dashboard_id}/panels/{panel_id}` |
| `DeleteDashboardPanelAsync` | DELETE | `/api/{org_id}/dashboards/{dashboard_id}/panels/{panel_id}` |

> Panel operations are only supported for v8 dashboards and `hash` is mandatory (optimistic concurrency); a mismatch returns 409.

### Report

| Method | HTTP | Path |
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

> Report endpoints use the `/api/v2` prefix, unlike the other APIs.

## Calling style

* **Pass business parameters only** — there is no need to build `*Request` models yourself (the client assembles request bodies internally), for example:

```csharp
var client = ServiceProvider.GetRequiredService<OpenObserveClient>();

// create a user
await client.CreateUserAsync("user@abp.io", "complex#pass", firstName: "ming", role: "admin");

// ingest a single record
await client.JsonRecordAsync("default", "audit-log", new { action = "Login", user = "admin" });

// search
var result = await client.SearchAsync<JsonNode>(
    new SearchQuery("SELECT * FROM audit-log", DateTime.UtcNow.AddHours(-1), DateTime.UtcNow, size: 20));
```

* All serialization/deserialization goes through **`IOpenObserveSerializer`**; the default implementation (`DefaultOpenObserveSerializer`) is built on ABP's `IJsonSerializer`:
  * it honours the host's `AbpSystemTextJsonSerializerOptions` (converters, date formats, encoder, ...); ABP defaults the encoder to `UnsafeRelaxedJsonEscaping`, so Chinese text and quotes are not escaped as `\uXXXX`;
  * wire names come from `JsonPropertyName` on the models (note that the `camelCase` default of `IJsonSerializer.Serialize` only affects properties without that attribute);
  * the OpenObserve-specific "omit null fields when serializing" behaviour is implemented with an ABP **type-level serializer modifier**
    (`AbpSystemTextJsonSerializerModifiersOptions.Modifiers`: `List<Action<JsonTypeInfo>>` → `OpenObserveIgnoreNullPropertiesModifier`), scoped to the
    `LINGYUN.Abp.OpenObserve.Models` namespace: optional parameters that are not supplied produce no `null` field (for example the user password field, which the docs say "shouldn't pass if you don't want to change password");
  * note: the `IJsonSerializer` interface (ABP 10.x) only exposes `Serialize(object, camelCase, indented)` / `Deserialize<T>(string, camelCase)` /
    `Deserialize(Type, string, camelCase)` and does **not** accept a custom `JsonSerializerOptions`; use a modifier for per-type customisation, for example:

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

* To replace the serialization strategy entirely, implement `IOpenObserveSerializer` and mark it with `[Dependency(ReplaceServices = true)]`.
* The organization argument can be omitted; it defaults to `AbpOpenObserveOptions.Organization`. The cluster info API defaults to the `_meta` organization (server-side restriction).
* Data models are organised as **one type per file**, mirroring the `Models/Search` layout.

## Unit tests

Test project: `aspnet-core/tests/LINGYUN.Abp.OpenObserve.Tests` (added to the `LINGYUN.MicroService.All` solution)

```bash
dotnet test aspnet-core/tests/LINGYUN.Abp.OpenObserve.Tests/LINGYUN.Abp.OpenObserve.Tests.csproj
```

The tests intercept requests with an `HttpMessageHandler` and assert the **HTTP method / path / query parameters / request body** of every endpoint, plus response deserialization and exception types, without any real network traffic.

## Error handling

| Exception | Description |
| --- | --- |
| `OpenObserveException` | Base business exception carrying `Code` / `StatusCode` / `Error` / `ResponseBody` |
| `OpenObserveRequestException` | The response body is not an OpenObserve error envelope |
| `OpenObserveSearchException` | Search API business error (error code, `hint`, `suggestions`) |
| `OpenObserveIngestionException` | Ingestion API business error (includes the stream name) |

## Documentation vs implementation

Discrepancies found while implementing; the code follows the behaviour that actually works:

1. **Cluster**: the docs say `GET /api/{org_id}/cluster_info`, the implementation registers `GET /api/{org_id}/cluster/info`; the latter is used and can be changed by overriding `CreateClusterInfoRequestUri`.
2. **Metrics**: the page header says `GET /metrics` while the example says `PUT`; only GET is routed.
3. **Function list response**: the docs table lists `stream_name` / `order` / `num_args` / `trans_type`, the implementation returns camelCase (`numArgs` / `transType` / `streams[]`). The model follows the implementation and keeps unknown fields via extension data.
4. **Function `order`**: present in the docs example but not accepted by the current implementation (only `streams[].order` applies).
5. **Dashboard**: only panel level endpoints are documented; dashboard CRUD/move/folders are not (see the bundled OpenAPI/Swagger of your instance).
6. **Ingestion content type**: not documented; `_json` uses `application/json` and `_bulk` / `_multi` use `application/x-ndjson`.
7. **Search `sort_by`**: not a field of `_search` (multi-stream search only); use SQL `ORDER BY`.
8. **`use_cache` / `clear_cache`**: the query string value overrides the body, and `use_cache` defaults to `true`.
