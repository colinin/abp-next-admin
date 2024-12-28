# LINGYUN.Abp.Identity.Session.AspNetCore

身份服务用户会话扩展模块

## 接口描述

### AbpSessionMiddleware 在请求管道中从用户令牌提取 *sessionId* 作为全局会话标识, 可用于注销会话  
> 注意: 当匿名用户访问时, 以请求 *CorrelationId* 作为标识;
        当 *CorrelationId* 不存在时, 使用随机 *Guid.NewGuid()*.

### HttpContextDeviceInfoProvider 从请求参数中提取设备标识  

> 出于模块职责分离原则, 请勿与 *LINGYUN.Abp.Identity.AspNetCore.Session* 模块混淆  


### HttpContextDeviceInfoProvider 用于处理会话IP的地址位置解析

> IsParseIpLocation 启用后将对会话IP进行地理位置解析
> IgnoreProvinces 解析地理位置时需要忽略的省份,通常情况下中国的直辖市需要
> LocationParser 自定义需要处理的地理位置数据

## 功能特性

* 提供AspNetCore环境下的会话管理功能
* 支持从请求中提取设备标识信息
* 支持IP地理位置解析功能
* 依赖AbpAspNetCoreModule、AbpIP2RegionModule和AbpIdentitySessionModule模块

## 配置项

### AbpIdentitySessionAspNetCoreOptions

```json
{
  "Identity": {
    "Session": {
      "AspNetCore": {
        "IsParseIpLocation": false,    // 是否解析IP地理信息，默认：false
        "IgnoreProvinces": [          // 不做处理的省份，默认包含中国直辖市
          "北京",
          "上海",
          "天津",
          "重庆"
        ]
      }
    }
  }
}
```

## 基本用法

1. 配置IP地理位置解析
```csharp
Configure<AbpIdentitySessionAspNetCoreOptions>(options =>
{
    options.IsParseIpLocation = true;    // 启用IP地理位置解析
    options.IgnoreProvinces.Add("香港"); // 添加需要忽略的省份
    options.LocationParser = (locationInfo) => 
    {
        // 自定义地理位置解析逻辑
        return $"{locationInfo.Country}{locationInfo.Province}{locationInfo.City}";
    };
});
```

2. 使用设备信息提供者
```csharp
public class YourService
{
    private readonly IDeviceInfoProvider _deviceInfoProvider;

    public YourService(IDeviceInfoProvider deviceInfoProvider)
    {
        _deviceInfoProvider = deviceInfoProvider;
    }

    public async Task<DeviceInfo> GetDeviceInfoAsync()
    {
        return await _deviceInfoProvider.GetDeviceInfoAsync();
    }
}
```

## 配置使用

```csharp
[DependsOn(typeof(AbpIdentitySessionAspNetCoreModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 更多信息

* [ABP身份认证文档](https://docs.abp.io/en/abp/latest/Identity)
* [ABP AspNetCore文档](https://docs.abp.io/en/abp/latest/AspNetCore)
