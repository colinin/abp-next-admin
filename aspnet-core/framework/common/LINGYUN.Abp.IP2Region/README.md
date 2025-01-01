# LINGYUN.Abp.IP2Region

## 介绍

`LINGYUN.Abp.IP2Region` 是一个基于IP2Region的ABP框架模块，提供IP地址查询功能。本模块集成了IP2Region.Net库，提供了便捷的IP地址查询服务。

## 功能

* 提供IP地址查询服务
* 支持多种缓存策略
* 内置IP数据库文件
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

    public async Task<string> SearchIpInfo(string ip)
    {
        return await _searcher.SearchAsync(ip);
    }
}
```

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