# LINGYUN.Abp.Location.Tencent

## 介绍

`LINGYUN.Abp.Location.Tencent` 是基于腾讯地图API的位置服务实现模块，提供了地理编码、反向地理编码、IP定位等功能。

## 功能特性

* 地理编码：将详细的结构化地址转换为对应的经纬度坐标
* 反向地理编码：将经纬度坐标转换为对应的结构化地址
* IP定位：根据IP地址获取位置信息
* POI（兴趣点）信息：获取周边的商铺、餐厅等兴趣点信息
* 行政区划信息：获取详细的行政区划层级信息
* 地址解析：智能解析地址信息，支持多种格式

## 安装

```bash
dotnet add package LINGYUN.Abp.Location.Tencent
```

## 配置

1. 添加模块依赖：

```csharp
[DependsOn(typeof(AbpTencentLocationModule))]
public class YourModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<TencentLocationOptions>(options =>
        {
            // 设置腾讯地图API密钥
            options.Key = "your-tencent-map-key";
            // 可选：设置安全密钥（SK校验）
            options.SecretKey = "your-tencent-map-sk";
        });
    }
}
```

## 使用方法

1. 注入并使用位置解析服务：

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
        // city参数可选，用于指定地址所在城市
        return await _locationProvider.GeocodeAsync(address, "北京市");
    }

    // 反向地理编码：坐标转地址
    public async Task<ReGeocodeLocation> ReGeocodeAsync(double lat, double lng)
    {
        // radius参数可选，指定搜索半径（米）
        return await _locationProvider.ReGeocodeAsync(lat, lng, 1000);
    }

    // IP地理位置解析
    public async Task<IPGecodeLocation> IPGeocodeAsync(string ipAddress)
    {
        return await _locationProvider.IPGeocodeAsync(ipAddress);
    }
}
```

## 返回数据说明

### 地理编码返回数据

```json
{
    "location": {
        "lat": 39.915119,    // 纬度值
        "lng": 116.403963    // 经度值
    },
    "title": "天安门",       // 地点名称
    "address": "北京市东城区东长安街",  // 地址
    "category": "旅游景点",   // 类别
    "adcode": "110101",      // 行政区划代码
    "similarity": 0.8,       // 相似度（0-1）
    "reliability": 7,        // 可信度（1-10）
    "level": 11             // 地址类型
}
```

### 反向地理编码返回数据

```json
{
    "location": {
        "lat": 39.915119,    // 纬度值
        "lng": 116.403963    // 经度值
    },
    "address": "北京市东城区东长安街",  // 完整地址
    "formatted_addresses": {
        "recommend": "东城区天安门",    // 推荐地址
        "rough": "北京市东城区"         // 粗略地址
    },
    "address_component": {
        "nation": "中国",              // 国家
        "province": "北京市",          // 省份
        "city": "北京市",              // 城市
        "district": "东城区",          // 区县
        "street": "东长安街",          // 街道
        "street_number": "1号"         // 门牌号
    },
    "pois": [                         // 周边POI信息
        {
            "title": "天安门",         // POI名称
            "address": "北京市东城区东长安街", // POI地址
            "category": "旅游景点",     // POI类型
            "distance": 100,          // 距离（米）
            "_distance": 100.0,       // 距离（米，浮点数）
            "tel": "",               // 电话
            "ad_info": {             // 行政区划信息
                "adcode": "110101",  // 行政区划代码
                "name": "东城区",     // 行政区划名称
                "location": {        // 行政区划中心点
                    "lat": 39.915119,
                    "lng": 116.403963
                }
            }
        }
    ]
}
```

## 更多信息

* [English Documentation](./README.EN.md)
* [腾讯位置服务](https://lbs.qq.com/)
