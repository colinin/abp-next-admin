# LINGYUN.Abp.Location

## 介绍

`LINGYUN.Abp.Location` 是一个位置服务基础模块，提供了地理位置相关的功能，包括地理编码（正向/反向）、距离计算等功能。

## 功能特性

* 地理编码（Geocoding）和反向地理编码（Reverse Geocoding）
* IP地理位置解析
* 位置距离计算（基于Google算法，误差<0.2米）
* 位置偏移量计算
* 支持POI（兴趣点）和道路信息
* 可扩展的位置解析提供程序

## 安装

```bash
dotnet add package LINGYUN.Abp.Location
```

## 使用方法

1. 添加模块依赖：

```csharp
[DependsOn(typeof(AbpLocationModule))]
public class YourModule : AbpModule
{
    // ...
}
```

2. 注入并使用位置解析服务：

```csharp
public class YourLocationService
{
    private readonly ILocationResolveProvider _locationProvider;

    public YourLocationService(ILocationResolveProvider locationProvider)
    {
        _locationProvider = locationProvider;
    }

    // 地理编码：地址转坐标
    public async Task<GecodeLocation> GeocodeAsync(string address)
    {
        return await _locationProvider.GeocodeAsync(address);
    }

    // 反向地理编码：坐标转地址
    public async Task<ReGeocodeLocation> ReGeocodeAsync(double lat, double lng)
    {
        return await _locationProvider.ReGeocodeAsync(lat, lng);
    }

    // IP地理位置解析
    public async Task<IPGecodeLocation> IPGeocodeAsync(string ipAddress)
    {
        return await _locationProvider.IPGeocodeAsync(ipAddress);
    }
}
```

## 位置计算

模块提供了强大的位置计算功能：

```csharp
// 创建位置对象
var location1 = new Location { Latitude = 39.9042, Longitude = 116.4074 }; // 北京
var location2 = new Location { Latitude = 31.2304, Longitude = 121.4737 }; // 上海

// 计算两点之间的距离（米）
double distance = location1.CalcDistance(location2);

// 计算位置的偏移
var offset = location1.CalcOffset(1000, 45); // 向东北方向偏移1000米
```

## 自定义位置解析提供程序

要实现自定义的位置解析提供程序，需要：

1. 实现 `ILocationResolveProvider` 接口：

```csharp
public class CustomLocationProvider : ILocationResolveProvider
{
    public async Task<IPGecodeLocation> IPGeocodeAsync(string ipAddress)
    {
        // 实现IP地理位置解析
    }

    public async Task<GecodeLocation> GeocodeAsync(string address, string city = null)
    {
        // 实现地理编码
    }

    public async Task<ReGeocodeLocation> ReGeocodeAsync(double lat, double lng, int radius = 50)
    {
        // 实现反向地理编码
    }
}
```

2. 在模块中注册你的实现：

```csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    context.Services.AddTransient<ILocationResolveProvider, CustomLocationProvider>();
}
```

## 更多信息

* [English Documentation](./README.EN.md)
* [百度地图定位服务](./LINGYUN.Abp.Location.Baidu/README.md)
* [腾讯地图定位服务](./LINGYUN.Abp.Location.Tencent/README.md)
