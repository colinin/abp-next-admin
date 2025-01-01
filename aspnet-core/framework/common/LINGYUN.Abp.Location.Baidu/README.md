# LINGYUN.Abp.Location.Baidu

## 介绍

`LINGYUN.Abp.Location.Baidu` 是基于百度地图API的位置服务实现模块，提供了地理编码、反向地理编码、IP定位等功能。

## 功能特性

* 地理编码：将详细的结构化地址转换为对应的经纬度坐标
* 反向地理编码：将经纬度坐标转换为对应的结构化地址
* IP定位：根据IP地址获取位置信息
* POI（兴趣点）信息：获取周边的商铺、餐厅等兴趣点信息
* 道路信息：获取附近的道路信息
* 行政区划信息：获取详细的行政区划层级信息

## 安装

```bash
dotnet add package LINGYUN.Abp.Location.Baidu
```

## 配置

1. 添加模块依赖：

```csharp
[DependsOn(typeof(AbpBaiduLocationModule))]
public class YourModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<BaiduLocationOptions>(options =>
        {
            // 设置百度地图API密钥
            options.AccessKey = "your-baidu-map-ak";
            // 可选：设置安全密钥（sn校验）
            options.SecurityKey = "your-baidu-map-sk";
            // 可选：设置坐标系类型（默认为bd09ll）
            options.CoordType = "bd09ll";
            // 可选：设置输出格式（默认为json）
            options.Output = "json";
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
    "precise": 1,            // 位置的附加信息，是否精确查找（1为精确，0为不精确）
    "confidence": 80,        // 可信度
    "comprehension": 100,    // 地址理解程度
    "level": "门址"          // 地址类型
}
```

### 反向地理编码返回数据

```json
{
    "location": {
        "lat": 39.915119,    // 纬度值
        "lng": 116.403963    // 经度值
    },
    "formatted_address": "北京市东城区东长安街",  // 结构化地址信息
    "business": "天安门",                      // 商圈信息
    "addressComponent": {
        "country": "中国",                     // 国家
        "province": "北京市",                  // 省份
        "city": "北京市",                      // 城市
        "district": "东城区",                  // 区县
        "street": "东长安街",                  // 街道
        "street_number": "1号"                // 门牌号
    },
    "pois": [                                // 周边POI信息
        {
            "name": "天安门",                  // POI名称
            "type": "旅游景点",                // POI类型
            "distance": "100"                 // 距离（米）
        }
    ],
    "roads": [                               // 周边道路信息
        {
            "name": "东长安街",                // 道路名称
            "distance": "50"                  // 距离（米）
        }
    ]
}
```

## 更多信息

* [English Documentation](./README.EN.md)
* [百度地图开放平台](https://lbsyun.baidu.com/)
