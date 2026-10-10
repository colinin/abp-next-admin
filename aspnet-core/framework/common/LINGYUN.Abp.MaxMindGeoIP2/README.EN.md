# LINGYUN.Abp.MaxMindGeoIP2

## Introduction

`LINGYUN.Abp.MaxMindGeoIP2` is an ABP framework module based on [MaxMind.GeoIP2](https://github.com/maxmind/GeoIP2-dotnet).
It resolves IP geolocation from an mmdb database (the embedded GeoLite2-City by default) and registers an IP location
resolve contributor for `LINGYUN.Abp.IP.Location`, so it can be used interchangeably with `LINGYUN.Abp.IP2Region`
(the remarks rules are identical).

## Features

* Resolves IP geolocation (country / province / city) from an mmdb database
* Ships `GeoLite2-City.mmdb`, and can point to a physical file to replace or update the database
* Supports both IPv4 and IPv6 (depending on the database)
* Resolves geo names for the current culture (from `Names`, falling back to `Name`)
* Loopback, private and reserved addresses return no data (empty remarks)
* Supports the ABP virtual file system

## Installation

```bash
dotnet add package LINGYUN.Abp.MaxMindGeoIP2
```

## Usage

1. Add `[DependsOn(typeof(AbpMaxMindGeoIP2Module))]` to your module class.

```csharp
[DependsOn(typeof(AbpMaxMindGeoIP2Module))]
public class YourModule : AbpModule
{
    // ...
}
```

2. Inject `IIPLocationResolver` to resolve an IP location:

```csharp
public class YourService
{
    private readonly IIPLocationResolver _ipLocationResolver;

    public YourService(IIPLocationResolver ipLocationResolver)
    {
        _ipLocationResolver = ipLocationResolver;
    }

    public async Task<string?> ResolveAsync(string ip)
    {
        var result = await _ipLocationResolver.ResolveAsync(ip);

        // 223.5.5.5 => 浙江杭州
        return result.Location?.Remarks;
    }
}
```

## Options

```csharp
Configure<AbpMaxMindGeoIP2Options>(options =>
{
    // physical files take precedence (easy to replace/update); the embedded GeoLite2-City.mmdb is used by default
    options.DatabaseFile = "/data/GeoLite2-City.mmdb";
    // applies to physical files only, memory mapped by default
    options.FileAccessMode = FileAccessMode.MemoryMapped;
});
```

```json
{
  "MaxMindGeoIP2": {
    "DatabaseFile": "/data/GeoLite2-City.mmdb",
    "FileAccessMode": "MemoryMapped"
  }
}
```

### Geo names and cultures

The reader is opened without locales (i.e. with the database default `Name`), and geo names are resolved per request from
MaxMind's `Names` dictionary using the **current culture** (`CultureInfo.CurrentUICulture`):

* the full culture name is tried first (e.g. `zh-CN`), then the language name (e.g. `es-MX` → `es`);
* if neither matches, `Name` (the database default language, usually English) is used;
* the embedded GeoLite2-City provides `de,en,es,fr,ja,pt-BR,ru,zh-CN`; some entities only provide a few languages and
  fall back as described above.

That means a single singleton reader serves every culture — no need for one reader per language.

| Current culture | Remarks for `8.8.8.8` | Remarks for `223.5.5.5` |
|---|---|---|
| `zh-CN` | 美国 | 浙江杭州 |
| `en-US` | United States | China |
| `ja-JP` | アメリカ | 中国 |
| `ko-KR` (not in database, falls back to `Name`) | United States | China |

### Remarks rules

The remarks rules are handled by `LINGYUN.Abp.IP.Location` and shared with `LINGYUN.Abp.IP2Region`:

```csharp
Configure<AbpIPLocationResolveOptions>(options =>
{
    // hide the country for Chinese IPs
    options.UseCountry = location => !string.Equals("中国", location.Country);
    // show the province for Chinese IPs
    options.UseProvince = location => string.Equals("中国", location.Country);
});
```

| Scenario | `Remarks` example |
|---|---|
| China with province and city | `浙江杭州` (city only when province equals city) |
| China with country only | `中国` |
| Other countries | `美国`, `日本`, `德国` |
| Loopback / private / reserved / not in database | empty |

## Performance

Measured with [BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet) (`MemoryDiagnoser` + `InProcessEmitToolchain`):

```ini
BenchmarkDotNet v0.15.8, Windows 10.0.26100
Intel Core i7-14700K (20C/28T), .NET 10.0.12 (SDK 10.0.401), X64 RyuJIT x86-64-v3
GC=Concurrent Workstation, Toolchain=InProcessEmitToolchain (IterationCount=15)
```

| Method | Mean | Error | StdDev | Ratio | Gen0 | Gen1 | Allocated |
|---|---:|---:|---:|---:|---:|---:|---:|
| `DatabaseReader_TryCity_IPv4` (baseline) | 3.428 μs | 0.0415 μs | 0.0388 μs | 1.00 | 0.4807 | 0.0038 | 8.15 KB |
| `DatabaseReader_TryCity_IPv6` | 3.496 μs | 0.0273 μs | 0.0228 μs | 1.02 | 0.4807 | 0.0038 | 8.15 KB |
| `DatabaseReader_TryCity_String_IPv4` | 3.428 μs | 0.0611 μs | 0.0542 μs | 1.00 | 0.4845 | 0.0038 | 8.19 KB |
| `ResolveAsync_IPv4` (module pipeline) | 3.753 μs | 0.0426 μs | 0.0399 μs | 1.09 | 0.5112 | 0.0076 | 8.73 KB |
| `ResolveAsync_IPv6` (module pipeline) | 3.812 μs | 0.0364 μs | 0.0323 μs | 1.11 | 0.5188 | 0.0076 | 8.78 KB |

* **~3.4 μs per lookup** (roughly 290k lookups/second on a single thread); IPv6 performs like IPv4 (`Ratio` 1.02);
* `TryCity(IPAddress)` and `TryCity(string)` are equivalent — string parsing overhead is negligible;
* the **full pipeline** through `IIPLocationResolver` (DI scope + contributor + culture-aware name resolution) only adds
  about **9%~11%**;
* each lookup allocates ~**8.15 KB** (the `CityResponse` object graph), ~8.73 KB through the full pipeline — watch GC
  under high concurrency (e.g. cache results for hot IPs);
* measurement setup: `TryCity` uses a **physical mmdb with memory mapping** (`FileAccessMode.MemoryMapped`, the
  recommended deployment); `ResolveAsync` uses the module defaults (embedded resource + `IIPLocationResolver`);
* benchmarks ran with the in-process toolchain (this environment cannot spawn dedicated benchmark processes), which is
  slightly faster than out-of-process mode — use the numbers for relative comparison only.

> The benchmark project is not part of this repository; reproduce it with the setup above
> (`new DatabaseReader(mmdbPath)` / module defaults + `[MemoryDiagnoser]`, `[InProcess]`).

## mmdb Database

* The embedded `GeoLite2-City.mmdb` is a snapshot taken at packaging time. Updating GeoLite2 requires a
  [MaxMind account](https://www.maxmind.com/en/geolite2/signup) and license key, so production deployments should
  download a fresh mmdb and point `DatabaseFile` to the physical file;
* Offline mmdb files can be downloaded from the [wp-statistics/GeoLite2-City](https://github.com/wp-statistics/GeoLite2-City)
  repository: it ships a compressed `GeoLite2-City.mmdb.gz`, which you extract to `GeoLite2-City.mmdb` and point
  `MaxMindGeoIP2:DatabaseFile` at;
  direct download: <https://raw.githubusercontent.com/wp-statistics/GeoLite2-City/master/GeoLite2-City.mmdb.gz>
* GeoLite2-City supports both IPv4 and IPv6 and bundles the
  `de,en,es,fr,ja,pt-BR,ru,zh-CN` locales;
* invalid, loopback and unspecified addresses are handled by `IIPLocationResolver` (empty result, no contributor runs);
* addresses missing from the database (private, reserved, not covered) return an empty `IPLocation`
  (country/province/city and remarks are all empty);
* query failures (e.g. a corrupted database) never break the request flow: a warning is logged and the address is treated
  as having no data (empty location).

## Links

* [中文文档](./README.md)
* [MaxMind GeoIP2 .NET](https://github.com/maxmind/GeoIP2-dotnet)
* [GeoLite2 free databases](https://dev.maxmind.com/geoip/geolite2-free-geolocation-data)
* [Offline mmdb download (wp-statistics/GeoLite2-City)](https://github.com/wp-statistics/GeoLite2-City)
