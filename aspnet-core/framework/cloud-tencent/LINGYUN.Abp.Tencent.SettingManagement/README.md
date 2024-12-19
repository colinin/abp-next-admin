# LINGYUN.Abp.Tencent.SettingManagement

腾讯云服务设置管理模块，提供腾讯云服务的配置管理界面和API。

## 功能特性

* 提供腾讯云服务的配置管理界面
* 支持全局和租户级别的配置管理
* 支持以下腾讯云服务的配置：
  * 基础配置（密钥、地域等）
  * 连接配置（HTTP方法、超时时间、代理等）
  * 短信服务配置（应用ID、默认签名、默认模板等）
  * QQ互联配置（应用ID、应用密钥等）
* 内置权限管理
* 支持多语言本地化
* 支持所有腾讯云可用区域的配置

## 权限

* `TencentCloud` - 腾讯云服务权限组
  * `TencentCloud.Settings` - 配置腾讯云服务权限

## API接口

### 获取全局配置

```http
GET /api/setting-management/tencent/by-global
```

### 获取当前租户配置

```http
GET /api/setting-management/tencent/by-current-tenant
```

## 基本用法

1. 添加模块依赖
```csharp
[DependsOn(typeof(AbpTencentCloudSettingManagementModule))]
public class YourModule : AbpModule
{
    // ...
}
```

2. 授权配置
```csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    Configure<AbpPermissionOptions>(options =>
    {
        options.GrantByDefault(TencentCloudSettingPermissionNames.Settings);
    });
}
```

## 配置项说明

### 基础配置

```json
{
  "Settings": {
    "Abp.TencentCloud": {
      "EndPoint": "ap-guangzhou", // 资源所在地域，默认为广州
      "SecretId": "您的腾讯云SecretId", // 从腾讯云控制台获取
      "SecretKey": "您的腾讯云SecretKey", // 从腾讯云控制台获取
      "DurationSecond": "600" // 会话持续时长，单位秒
    }
  }
}
```

### 连接配置

```json
{
  "Settings": {
    "Abp.TencentCloud.Connection": {
      "HttpMethod": "POST", // 请求方法，默认POST
      "Timeout": "60", // 连接超时时间，单位秒
      "WebProxy": "", // 代理服务器地址，可选
      "EndPoint": "" // 特定服务的域名，如金融区服务需要指定
    }
  }
}
```

### 短信服务配置

```json
{
  "Settings": {
    "Abp.TencentCloud.Sms": {
      "AppId": "", // 短信应用ID，在短信控制台添加应用后生成
      "DefaultSignName": "", // 默认短信签名
      "DefaultTemplateId": "" // 默认短信模板ID
    }
  }
}
```

### QQ互联配置

```json
{
  "Settings": {
    "Abp.TencentCloud.QQConnect": {
      "AppId": "", // QQ互联应用ID，从QQ互联管理中心获取
      "AppKey": "", // QQ互联应用密钥，从QQ互联管理中心获取
      "IsMobile": "false" // 是否使用移动端样式，默认为PC端样式
    }
  }
}
```

## 可用区域

模块支持以下腾讯云区域：

* 中国区域
  * 华北地区（北京）- ap-beijing
  * 西南地区（成都）- ap-chengdu
  * 西南地区（重庆）- ap-chongqing
  * 华南地区（广州）- ap-guangzhou
  * 港澳台地区（中国香港）- ap-hongkong
  * 华东地区（南京）- ap-nanjing
  * 华东地区（上海）- ap-shanghai
  * 华东地区（上海金融）- ap-shanghai-fsi
  * 华南地区（深圳金融）- ap-shenzhen-fsi

* 亚太地区
  * 曼谷 - ap-bangkok
  * 雅加达 - ap-jakarta
  * 孟买 - ap-mumbai
  * 首尔 - ap-seoul
  * 新加坡 - ap-singapore
  * 东京 - ap-tokyo

* 欧洲地区
  * 法兰克福 - eu-frankfurt
  * 莫斯科 - eu-moscow

* 北美地区
  * 弗吉尼亚 - na-ashburn
  * 硅谷 - na-siliconvalley
  * 多伦多 - na-toronto

## 更多文档

* [腾讯云API密钥管理](https://console.cloud.tencent.com/cam/capi)
* [腾讯云地域和可用区](https://cloud.tencent.com/document/product/213/6091)

[English](./README.EN.md)
