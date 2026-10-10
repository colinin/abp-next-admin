# LINGYUN.Abp.MaxMindGeoIP2

## 介绍

`LINGYUN.Abp.MaxMindGeoIP2` 是一个基于 [MaxMind.GeoIP2](https://github.com/maxmind/GeoIP2-dotnet) 的 ABP 框架模块，
通过 mmdb 离线库(默认内置 GeoLite2-City)解析 IP 地理位置，并注册 `LINGYUN.Abp.IP.Location` 的位置解析贡献者，
可与 `LINGYUN.Abp.IP2Region` 互换使用(备注规则保持一致)。

## 功能

* 通过 mmdb 离线库解析 IP 地理位置(国家/省份/城市)
* 内置 `GeoLite2-City.mmdb`，也可指向物理文件以替换/更新离线库
* 同时支持 IPv4 与 IPv6(取决于离线库)
* 按当前文化解析地理名称(从 `Names` 获取，缺失时回退 `Name`)
* 本地回环、内网、保留地址等无记录地址不返回数据(备注为空)
* 支持 ABP 虚拟文件系统

## 安装

```bash
dotnet add package LINGYUN.Abp.MaxMindGeoIP2
```

## 使用

1. 添加 `[DependsOn(typeof(AbpMaxMindGeoIP2Module))]` 到你的模块类上。

```csharp
[DependsOn(typeof(AbpMaxMindGeoIP2Module))]
public class YourModule : AbpModule
{
    // ...
}
```

2. 注入 `IIPLocationResolver` 解析 IP 位置：

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

## 选项

```csharp
Configure<AbpMaxMindGeoIP2Options>(options =>
{
    // 物理文件优先(便于替换/更新离线库)，默认使用内置的 GeoLite2-City.mmdb 资源
    options.DatabaseFile = "/data/GeoLite2-City.mmdb";
    // 仅对物理文件生效，默认内存映射
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

### 地理名称与文化

查询器不指定 locales(即以离线库默认名称 `Name` 打开)，地理名称在解析时按**当前文化**
(`CultureInfo.CurrentUICulture`)从 MaxMind 的 `Names` 字典中获取：

* 先按完整文化名称匹配(如 `zh-CN`)，未命中再按语言名称匹配(如 `es-MX` → `es`)；
* 仍未命中时回退到 `Name`(离线库默认语言，通常为英语)；
* 内置 GeoLite2-City 提供 `de,en,es,fr,ja,pt-BR,ru,zh-CN` 语言，部分实体只提供少数语言(此时按上述规则回退)。

因此同一个单例查询器可为所有文化提供服务，无需按语言创建多个查询器。

| 当前文化 | `8.8.8.8` 的备注 | `223.5.5.5` 的备注 |
|---|---|---|
| `zh-CN` | 美国 | 浙江杭州 |
| `en-US` | United States | China |
| `ja-JP` | アメリカ | 中国 |
| `ko-KR`(离线库无该语言，回退 `Name`) | United States | China |

### 备注(Remarks)规则

备注(Remarks)规则由 `LINGYUN.Abp.IP.Location` 统一处理，与 `LINGYUN.Abp.IP2Region` 共用同一份配置：

```csharp
Configure<AbpIPLocationResolveOptions>(options =>
{
    // 仅中国IP不显示国家
    options.UseCountry = location => !string.Equals("中国", location.Country);
    // 仅中国IP显示省份
    options.UseProvince = location => string.Equals("中国", location.Country);
});
```

| 场景 | `Remarks` 示例 |
|---|---|
| 中国且有省份与城市 | `浙江杭州`(省市同名时仅显示市) |
| 中国且仅有国家 | `中国` |
| 其它国家 | `美国`、`日本`、`德国` |
| 本地回环 / 内网 / 保留 / 离线库未收录 | 空 |

## 性能基准

使用 [BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet)(`MemoryDiagnoser` + `InProcessEmitToolchain`)测得：

```ini
BenchmarkDotNet v0.15.8, Windows 10.0.26100
Intel Core i7-14700K (20C/28T), .NET 10.0.12 (SDK 10.0.401), X64 RyuJIT x86-64-v3
GC=Concurrent Workstation, Toolchain=InProcessEmitToolchain (IterationCount=15)
```

| 方法 | Mean | Error | StdDev | Ratio | Gen0 | Gen1 | Allocated |
|---|---:|---:|---:|---:|---:|---:|---:|
| `DatabaseReader_TryCity_IPv4`(基线) | 3.428 μs | 0.0415 μs | 0.0388 μs | 1.00 | 0.4807 | 0.0038 | 8.15 KB |
| `DatabaseReader_TryCity_IPv6` | 3.496 μs | 0.0273 μs | 0.0228 μs | 1.02 | 0.4807 | 0.0038 | 8.15 KB |
| `DatabaseReader_TryCity_String_IPv4` | 3.428 μs | 0.0611 μs | 0.0542 μs | 1.00 | 0.4845 | 0.0038 | 8.19 KB |
| `ResolveAsync_IPv4`(模块全链路) | 3.753 μs | 0.0426 μs | 0.0399 μs | 1.09 | 0.5112 | 0.0076 | 8.73 KB |
| `ResolveAsync_IPv6`(模块全链路) | 3.812 μs | 0.0364 μs | 0.0323 μs | 1.11 | 0.5188 | 0.0076 | 8.78 KB |

* **单次查询约 3.4 μs**（单线程理论吞吐约 29 万次/秒），IPv6 与 IPv4 基本一致（`Ratio` 1.02）；
* `TryCity(IPAddress)` 与 `TryCity(string)` 无差异，字符串解析开销可忽略；
* 经 `IIPLocationResolver` 的**完整链路**（DI Scope + 贡献者 + 按当前文化解析名称）仅增加约 **9%~11%**；
* 每次查询分配约 **8.15 KB**（`CityResponse` 模型对象图），完整链路约 8.73 KB —— 高并发场景建议关注 GC（例如对热点 IP 做结果缓存）；
* 测量场景：`TryCity` 使用**物理 mmdb + 内存映射**（`FileAccessMode.MemoryMapped`，推荐的部署方式）；`ResolveAsync` 使用模块默认配置（内置资源 + `IIPLocationResolver`）；
* 基准使用 in-process 工具链（当前环境无法派生独立基准进程），数值会略优于独立进程模式，仅用于横向对比。

## mmdb 离线库说明

* 内置的 `GeoLite2-City.mmdb` 为打包时的快照数据；GeoLite2 需要 [MaxMind 账号](https://www.maxmind.com/en/geolite2/signup)
  与 License Key 才能定期更新，生产环境建议下载最新 mmdb 并通过 `DatabaseFile` 指向物理文件；
* 离线 mmdb 文件可从 [wp-statistics/GeoLite2-City](https://github.com/wp-statistics/GeoLite2-City) 仓库下载：
  仓库中提供的是压缩包 `GeoLite2-City.mmdb.gz`，解压后得到 `GeoLite2-City.mmdb`，
  再通过 `MaxMindGeoIP2:DatabaseFile` 指向该文件；
  直接下载地址：<https://raw.githubusercontent.com/wp-statistics/GeoLite2-City/master/GeoLite2-City.mmdb.gz>
* GeoLite2-City 同时支持 IPv4 与 IPv6，并内置
  `de,en,es,fr,ja,pt-BR,ru,zh-CN` 语言；
* 非法地址、本地回环与未指定地址由 `IIPLocationResolver` 统一处理(返回空结果，不进入位置解析模块)；
* 离线库没有记录的地址(内网、保留、未收录)返回空的 `IPLocation`(国家/省份/城市与备注均为空)；
* 查询异常(例如离线库文件损坏)不会阻断业务流程：记录警告日志并按无数据处理(返回空位置)。

## 链接

* [English document](./README.EN.md)
* [MaxMind GeoIP2 .NET](https://github.com/maxmind/GeoIP2-dotnet)
* [GeoLite2 离线库](https://dev.maxmind.com/geoip/geolite2-free-geolocation-data)
* [离线 mmdb 下载(wp-statistics/GeoLite2-City)](https://github.com/wp-statistics/GeoLite2-City)
