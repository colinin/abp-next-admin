# LINGYUN.Abp.IP2Region

## 介绍

`LINGYUN.Abp.IP2Region` 是一个基于IP2Region的ABP框架模块，提供IP地址查询功能。本模块集成了IP2Region.Net库，提供了便捷的IP地址查询服务。

## 功能

* 提供IP地址查询服务
* 同时支持 IPv4 与 IPv6 离线库，按地址族自动选择数据文件
* 支持多种缓存策略
* 内置 IPv4 与 IPv6 数据库文件
* 支持ABP虚拟文件系统

## 安装

```bash
dotnet add package LINGYUN.Abp.IP2Region
```

## 使用

1. 添加 `[DependsOn(typeof(AbpIP2RegionModule))]` 到你的模块类上。

```csharp
[DependsOn(typeof(AbpIP2RegionModule))]
public class YourModule : AbpModule
{
    // ...
}
```

2. 注入并使用IP查询服务：

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
        // 自动按地址族选择 IPv4 / IPv6 离线库
        return _searcher.Search(ip);
    }
}
```

## IPv4 / IPv6 支持

模块内置两个离线库文件(通过 ABP 虚拟文件系统加载)：

| 离线库 | 路径 |
|---|---|
| IPv4 | `/LINGYUN/Abp/IP2Region/Resources/ip2region_v4.xdb` |
| IPv6 | `/LINGYUN/Abp/IP2Region/Resources/ip2region_v6.xdb` |

`ISearcher` 注册的是 `AbpSearcher`，同时加载两个离线库并**按地址族自动路由**：

* IPv4 地址查询 IPv4 离线库，IPv6 地址查询 IPv6 离线库；
* IPv4 映射的 IPv6 地址(如 `::ffff:1.2.3.4`)会归一到 IPv4 查询；
* IPv6 离线库资源不存在时(例如打包时排除)，IPv6 查询返回 `null`，不会使用 IPv4 离线库解析出错误结果。

也可以直接使用 `AbpSearcher`：

```csharp
// 单文件模式：只加载一个离线库，由调用方保证查询地址与离线库版本一致
using var v6Searcher = new AbpSearcher(CachePolicy.File, ipv6XdbStream);

// 双文件模式：同时加载 IPv4 与 IPv6 离线库，按地址族自动路由
using var searcher = new AbpSearcher(CachePolicy.File, ipv4XdbStream, ipv6XdbStream);

var region = searcher.Search("2400:3200::1"); // 中国|浙江省|杭州市|阿里|CN
```

> IPv6 离线库(约 35MB)随包提供，可通过 `-p:IncludeIPv6Database=false` 在打包时排除以减小体积。

## 选项

```csharp
Configure<AbpIP2RegionOptions>(options =>
{
    // 离线库文件：物理文件优先，其次为虚拟文件系统路径(内置资源)
    options.IPv4DatabaseFile = "/data/ip2region_v4.xdb";
    options.IPv6DatabaseFile = "/data/ip2region_v6.xdb";
    // 默认值：CachePolicy.Content(并发安全)
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

> IPv4 / IPv6 离线库默认使用内置资源；配置为物理文件路径时**优先加载物理文件**，便于替换/更新离线库
> (离线库可从 [ip2region](https://github.com/lionsoul2014/ip2region) 下载或使用 maker 生成)；
> IPv6 离线库可选，文件不存在时 IPv6 查询返回 `null`。

### 缓存策略

| 缓存策略 | 说明 | 并发安全 |
|---|---|---|
| `Content`(默认) | 离线库全部加载到内存，单例查询器可并发使用 | 是 |
| `VectorIndex` | 只缓存向量索引(512KB)，其余读取数据流 | 否 |
| `File` | 完全基于文件流查询，内存占用最小 | 否 |

> `ISearcher` 注册为单例，而 `VectorIndex`/`File` 是"共享数据流 + Seek"的实现，多线程并发查询会互相打断数据流定位，
> 仅在能够保证串行查询时使用；内存占用参考：IPv4 库约 10MB、IPv6 库约 35MB。

## 备注(Remarks)规则

备注(Remarks)规则由 `LINGYUN.Abp.IP.Location` 统一处理(其它位置解析模块，例如 MaxMind，共用同一份配置)：

```csharp
Configure<AbpIPLocationResolveOptions>(options =>
{
    // 仅中国IP不显示国家
    options.UseCountry = location => !string.Equals("中国", location.Country);
    // 仅中国IP显示省份
    options.UseProvince = location => string.Equals("中国", location.Country);
});
```

默认(两者均为 `true`)时：有省份与城市则显示"省份+城市"(如 `浙江杭州`)，否则显示国家(如 `中国`、`日本`)。

## 特殊地址处理

* **非法地址、本地回环与未指定地址**(如 `not-an-ip`、`127.0.0.1`、`::1`、`0.0.0.0`、`::`)由 `IIPLocationResolver`
  统一处理，直接返回空的解析结果(`Location` 为 `null`)，不会进入位置解析模块；
* **内网、链路本地、组播、保留地址等**在 ip2region 离线库中返回的是英文 `Reserved`，
  模块返回一个 `Country`/`Province`/`City`/`Remarks` **均为空**的 `IPLocation`：

| 地址分类 | 示例 |
|---|---|
| 内网 | `10.0.0.1`、`172.16.0.1`、`192.168.1.1`、`fd00::1` |
| 链路本地 | `169.254.1.1`、`fe80::1` |
| 运营商级 NAT | `100.64.0.1` |
| 文档示例 / 基准测试 | `192.0.2.1`、`2001:db8::1`、`198.18.0.1` |
| 组播 / 广播 | `224.0.0.1`、`ff02::1`、`255.255.255.255` |
| 保留(离线库标记 `Reserved` 的其它地址段) | `64:ff9b::1` |

## IP2Region.Net 库说明

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

## ASP.NET Core Usage

```csharp
services.AddSingleton<ISearcher>(new Searcher(CachePolicy , "your xdb file path"));
```

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

## 链接

* [English document](./README.EN.md)