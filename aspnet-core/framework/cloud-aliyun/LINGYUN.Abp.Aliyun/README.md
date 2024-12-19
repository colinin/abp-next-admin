# LINGYUN.Abp.Aliyun

阿里云 SDK 集成模块。

参照：[阿里云 API 文档](https://help.aliyun.com/document_detail/28763.html)

## 功能特性

- 提供阿里云服务的基础 SDK 集成
- 支持阿里云 RAM（访问控制）认证
- 支持 STS Token 访问
- 支持阿里云短信服务（SMS）
- 支持阿里云对象存储（OSS）
- 提供分布式缓存支持，优化高并发场景

## 模块引用

```csharp
[DependsOn(typeof(AbpAliyunModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 配置项说明

### 认证配置

- **AliyunSettingNames.Authorization.RegionId**

  - 说明：阿里云服务区域
  - 类型：可选
  - 默认值：default
  - 示例：oss-cn-hangzhou

- **AliyunSettingNames.Authorization.AccessKeyId**

  - 说明：阿里云 RAM 账号的 AccessKey ID
  - 类型：必须
  - 获取方式：访问阿里云控制台-访问控制

- **AliyunSettingNames.Authorization.AccessKeySecret**
  - 说明：RAM 账号的 AccessKey Secret
  - 类型：必须
  - 获取方式：访问阿里云控制台-访问控制

### STS Token 配置

- **AliyunSettingNames.Authorization.UseSecurityTokenService**

  - 说明：是否使用 STS Token 访问
  - 类型：可选
  - 默认值：false
  - 建议：建议开启，提高安全性

- **AliyunSettingNames.Authorization.RamRoleArn**

  - 说明：阿里云 RAM 角色 ARN
  - 类型：启用 STS Token 时必须
  - 格式：acs:ram::$accountID:role/$roleName

- **AliyunSettingNames.Authorization.RoleSessionName**

  - 说明：用户自定义令牌名称
  - 类型：可选
  - 用途：用于访问审计

- **AliyunSettingNames.Authorization.DurationSeconds**

  - 说明：用户令牌的过期时间
  - 类型：可选
  - 默认值：3000
  - 单位：秒

- **AliyunSettingNames.Authorization.Policy**
  - 说明：权限策略
  - 类型：可选
  - 格式：JSON 字符串

### 短信服务配置

```json
{
  "Settings": {
    "Abp.Aliyun.Sms": {
      "Domain": "dysmsapi.aliyuncs.com", // API域名，默认为 dysmsapi.aliyuncs.com
      "Version": "2017-05-25", // API版本，默认为 2017-05-25
      "ActionName": "SendSms", // API方法名，默认为 SendSms
      "DefaultSignName": "", // 默认短信签名
      "DefaultTemplateCode": "", // 默认短信模板代码
      "DefaultPhoneNumber": "", // 默认接收短信的手机号码
      "VisableErrorToClient": "false" // 是否向客户端显示错误信息
    }
  }
}
```

## 可用区域

本模块支持以下阿里云区域：

- 中国区域

  - 华北 1（青岛）- cn-qingdao
  - 华北 2（北京）- cn-beijing
  - 华北 3（张家口）- cn-zhangjiakou
  - 华北 5（呼和浩特）- cn-huhehaote
  - 华东 1（杭州）- cn-hangzhou
  - 华东 2（上海）- cn-shanghai
  - 华南 1（深圳）- cn-shenzhen
  - 华南 2（河源）- cn-heyuan
  - 华南 3（广州）- cn-guangzhou
  - 西南 1（成都）- cn-chengdu

- 香港及海外区域
  - 香港 - cn-hongkong
  - 美国（硅谷）- us-west-1
  - 美国（弗吉尼亚）- us-east-1
  - 日本（东京）- ap-northeast-1
  - 韩国（首尔）- ap-northeast-2
  - 新加坡 - ap-southeast-1
  - 澳大利亚（悉尼）- ap-southeast-2
  - 马来西亚（吉隆坡）- ap-southeast-3
  - 印度尼西亚（雅加达）- ap-southeast-5
  - 菲律宾（马尼拉）- ap-southeast-6
  - 泰国（曼谷）- ap-southeast-7
  - 印度（孟买）- ap-south-1
  - 德国（法兰克福）- eu-central-1
  - 英国（伦敦）- eu-west-1
  - 阿联酋（迪拜）- me-east-1

## 性能优化

- 在高并发场景下，建议开启分布式缓存以提高性能
- 使用 STS Token 时，Token 会自动缓存到过期前
- 建议合理设置 DurationSeconds，避免过于频繁的 Token 刷新

## 相关模块

- [LINGYUN.Abp.Aliyun.SettingManagement](../LINGYUN.Abp.Aliyun.SettingManagement/README.md) - 提供配置管理功能
- [LINGYUN.Abp.Aliyun.Features](../LINGYUN.Abp.Aliyun.Features/README.md) - 提供功能管理功能

[点击查看英文文档](README.EN.md)
