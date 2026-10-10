# LINGYUN.Abp.IP2Region

## Introduction

`LINGYUN.Abp.IP2Region` is an ABP framework module based on IP2Region, providing IP address query functionality. This module integrates the IP2Region.Net library and provides convenient IP address query services.

## Features

* Provides IP address query service
* Supports both IPv4 and IPv6 databases, routing by address family automatically
* Supports multiple caching strategies
* Built-in IPv4 and IPv6 database files
* Supports ABP virtual file system

## Installation

```bash
dotnet add package LINGYUN.Abp.IP2Region
```

## Usage

1. Add `[DependsOn(typeof(AbpIP2RegionModule))]` to your module class.

```csharp
[DependsOn(typeof(AbpIP2RegionModule))]
public class YourModule : AbpModule
{
    // ...
}
```

2. Inject and use the IP query service:

```csharp
public class YourService
{
    private readonly ISearcher _searcher;

    public YourService(ISearcher searcher)
    {
        _searcher = searcher;
    }

    public string? SearchIpInfo(string ip)
    {
        // picks the IPv4 / IPv6 database by address family automatically
        return _searcher.Search(ip);
    }
}
```

## IPv4 / IPv6 Support

The module embeds two databases (loaded through the ABP virtual file system):

| Database | Path |
|---|---|
| IPv4 | `/LINGYUN/Abp/IP2Region/Resources/ip2region_v4.xdb` |
| IPv6 | `/LINGYUN/Abp/IP2Region/Resources/ip2region_v6.xdb` |

`ISearcher` is registered as `AbpSearcher`, which loads both databases and **routes by address family automatically**:

* IPv4 addresses are looked up in the IPv4 database, IPv6 addresses in the IPv6 database;
* IPv4-mapped IPv6 addresses (such as `::ffff:1.2.3.4`) are normalized to IPv4;
* when the IPv6 database is missing (for example excluded while packing), IPv6 lookups return `null` instead of resolving wrong results from the IPv4 database.

`AbpSearcher` can also be used directly:

```csharp
// single database: the caller must query addresses matching the database version
using var v6Searcher = new AbpSearcher(CachePolicy.File, ipv6XdbStream);

// dual databases: routing by address family
using var searcher = new AbpSearcher(CachePolicy.File, ipv4XdbStream, ipv6XdbStream);

var region = searcher.Search("2400:3200::1"); // 中国|浙江省|杭州市|阿里|CN
```

> The IPv6 database (~35MB) ships with the package; pass `-p:IncludeIPv6Database=false` to exclude it while packing.

## Options

```csharp
Configure<AbpIP2RegionOptions>(options =>
{
    // database files: physical files take precedence, otherwise the embedded resources are used
    options.IPv4DatabaseFile = "/data/ip2region_v4.xdb";
    options.IPv6DatabaseFile = "/data/ip2region_v6.xdb";
    // default: CachePolicy.Content (concurrency safe)
    options.CachePolicy = CachePolicy.Content;
});
```

```json
{
  "IP2Region": {
    "IPv4DatabaseFile": "/data/ip2region_v4.xdb",
    "IPv6DatabaseFile": "/data/ip2region_v6.xdb",
    "CachePolicy": "Content"
  }
}
```

> The IPv4 / IPv6 databases default to the embedded resources; when a physical file path is configured it is
> **loaded first**, which makes it easy to replace or update the database
> (download from [ip2region](https://github.com/lionsoul2014/ip2region) or generate one with the maker);
> the IPv6 database is optional — IPv6 lookups return `null` when it is missing.

### Cache policies

| Cache policy | Description | Concurrency safe |
|---|---|---|
| `Content` (default) | Both databases are loaded into memory; the singleton searcher can be used concurrently | Yes |
| `VectorIndex` | Only the vector index (512KB) is cached, the rest is read from the stream | No |
| `File` | Fully stream based, lowest memory footprint | No |

> `ISearcher` is registered as a singleton, while `VectorIndex`/`File` are "shared stream + Seek" implementations:
> concurrent queries interrupt each other's stream positioning, so use them only when queries are serialized.
> Memory footprint reference: IPv4 database ~10MB, IPv6 database ~35MB.

## Special Addresses

* **Invalid, loopback and unspecified addresses** (such as `not-an-ip`, `127.0.0.1`, `::1`, `0.0.0.0`, `::`) are handled
  by `IIPLocationResolver` itself: the resolution result is empty (`Location` is `null`) and no contributor runs;
* **Private, link-local, multicast and reserved addresses** are reported as the English `Reserved` by the ip2region
  database; the module returns an `IPLocation` whose `Country`/`Province`/`City`/`Remarks` are all empty:

| Category | Examples |
|---|---|
| Private | `10.0.0.1`, `172.16.0.1`, `192.168.1.1`, `fd00::1` |
| Link-local | `169.254.1.1`, `fe80::1` |
| Carrier-grade NAT | `100.64.0.1` |
| Documentation / Benchmarking | `192.0.2.1`, `2001:db8::1`, `198.18.0.1` |
| Multicast / Broadcast | `224.0.0.1`, `ff02::1`, `255.255.255.255` |
| Reserved (other ranges marked `Reserved`) | `64:ff9b::1` |

## IP2Region.Net Library Description

### Installation

Install the package with [NuGet](https://www.nuget.org/packages/IP2Region.Net)

```bash
Install-Package IP2Region.Net
```

### Usage

```csharp
using IP2Region.Net.Abstractions;
using IP2Region.Net.XDB;

ISearcher searcher = new Searcher(CachePolicy , "your xdb file path");
```

### Cache Policy Description
| Cache Policy            | Description                                                                                                | Thread Safe |
|-------------------------|------------------------------------------------------------------------------------------------------------|-------------|
| CachePolicy.Content     | Cache the entire `xdb` data.                                                                               | Yes         |
| CachePolicy.VectorIndex | Cache `vecotorIndex` to speed up queries and reduce system io pressure by reducing one fixed IO operation. | Yes         |
| CachePolicy.File        | Completely file-based queries                                                                              | Yes         |

### XDB File Description
Generate using [maker](https://github.com/lionsoul2014/ip2region/tree/master/maker/csharp), or [download](https://github.com/lionsoul2014/ip2region/blob/master/data/ip2region.xdb) pre-generated xdb files

## Performance

``` ini
BenchmarkDotNet=v0.13.2, OS=macOS 13.4.1 (c) (22F770820d) [Darwin 22.5.0]
Apple M1, 1 CPU, 8 logical and 8 physical cores
.NET SDK=7.0.306
  [Host]     : .NET 6.0.20 (6.0.2023.32017), Arm64 RyuJIT AdvSIMD
  DefaultJob : .NET 6.0.20 (6.0.2023.32017), Arm64 RyuJIT AdvSIMD
```

| Method                  |       Mean |    Error |   StdDev |
|-------------------------|-----------:|---------:|---------:|
| CachePolicy_Content     |   155.7 ns |  0.46 ns |  0.39 ns |
| CachePolicy_File        | 2,186.8 ns | 34.27 ns | 32.06 ns |
| CachePolicy_VectorIndex | 1,570.3 ns | 27.53 ns | 22.99 ns |

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

Please make sure to update tests as appropriate.

## License
[Apache License 2.0](https://github.com/lionsoul2014/ip2region/blob/master/LICENSE.md)

## Links

* [中文文档](./README.md)
