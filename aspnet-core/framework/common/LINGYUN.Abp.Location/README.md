# LINGYUN.Abp.Location

## 介绍

`LINGYUN.Abp.Location` 是一个位置服务基础模块，提供了地理位置相关的功能，包括地理编码（正向/反向）、距离计算等功能。

## 功能

* 地理编码（Geocoding）和反向地理编码（Reverse Geocoding）
* 位置距离计算（基于Google算法，误差<0.2米）
* 位置偏移量计算
* 支持POI（兴趣点）和道路信息
* 可扩展的位置解析提供程序

## 安装

```bash
dotnet add package LINGYUN.Abp.Location
```

## 使用

1. 添加 `[DependsOn(typeof(AbpLocationModule))]` 到你的模块类上。

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

    public async Task<GecodeLocation> GeocodeAsync(string address)
    {
        return await _locationProvider.GeocodeAsync(address);
    }

    public async Task<ReGeocodeLocation> ReGeocodeAsync(double lat, double lng)
    {
        return await _locationProvider.ReGeocodeAsync(lat, lng);
    }
}
```

## 高级用法

### 1. 距离计算

```csharp
// 计算两个位置之间的距离
var location1 = new Location { Latitude = 39.9042, Longitude = 116.4074 }; // 北京
var location2 = new Location { Latitude = 31.2304, Longitude = 121.4737 }; // 上海
double distance = Location.CalcDistance(location1, location2); // 返回单位：米
```

### 2. 计算位置偏移范围

```csharp
var location = new Location { Latitude = 39.9042, Longitude = 116.4074 };
// 计算指定距离（米）的偏移范围
var position = Location.CalcOffsetDistance(location, 1000); // 1公里范围
```

### 3. 自定义位置解析提供程序

```csharp
public class CustomLocationProvider : ILocationResolveProvider
{
    public async Task<GecodeLocation> GeocodeAsync(string address)
    {
        // 实现地理编码逻辑
    }

    public async Task<ReGeocodeLocation> ReGeocodeAsync(double lat, double lng)
    {
        // 实现反向地理编码逻辑
    }
}
```

## 链接

* [English document](./README.EN.md)
