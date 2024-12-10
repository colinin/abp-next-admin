# LINGYUN.Abp.Aliyun

阿里云SDK集成模块。

参照：[阿里云API文档](https://help.aliyun.com/document_detail/28763.html)

## 功能特性

* 提供阿里云服务的基础SDK集成
* 支持阿里云RAM（访问控制）认证
* 支持STS Token访问
* 支持阿里云短信服务（SMS）
* 支持阿里云对象存储（OSS）
* 提供分布式缓存支持，优化高并发场景

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

* **AliyunSettingNames.Authorization.RegionId**
  * 说明：阿里云服务区域
  * 类型：可选
  * 默认值：default
  * 示例：oss-cn-hangzhou

* **AliyunSettingNames.Authorization.AccessKeyId**
  * 说明：阿里云RAM账号的AccessKey ID
  * 类型：必须
  * 获取方式：访问阿里云控制台-访问控制

* **AliyunSettingNames.Authorization.AccessKeySecret**
  * 说明：RAM账号的AccessKey Secret
  * 类型：必须
  * 获取方式：访问阿里云控制台-访问控制

### STS Token配置

* **AliyunSettingNames.Authorization.UseSecurityTokenService**
  * 说明：是否使用STS Token访问
  * 类型：可选
  * 默认值：false
  * 建议：建议开启，提高安全性

* **AliyunSettingNames.Authorization.RamRoleArn**
  * 说明：阿里云RAM角色ARN
  * 类型：启用STS Token时必须
  * 格式：acs:ram::$accountID:role/$roleName

* **AliyunSettingNames.Authorization.RoleSessionName**
  * 说明：用户自定义令牌名称
  * 类型：可选
  * 用途：用于访问审计

* **AliyunSettingNames.Authorization.DurationSeconds**
  * 说明：用户令牌的过期时间
  * 类型：可选
  * 默认值：3000
  * 单位：秒

* **AliyunSettingNames.Authorization.Policy**
  * 说明：权限策略
  * 类型：可选
  * 格式：JSON字符串

## 性能优化

* 在高并发场景下，建议开启分布式缓存以提高性能
* 使用STS Token时，Token会自动缓存到过期前
* 建议合理设置DurationSeconds，避免过于频繁的Token刷新

## 相关模块

* [LINGYUN.Abp.Aliyun.SettingManagement](../LINGYUN.Abp.Aliyun.SettingManagement/README.md) - 提供配置管理功能
* [LINGYUN.Abp.Aliyun.Features](../LINGYUN.Abp.Aliyun.Features/README.md) - 提供功能管理功能

[点击查看英文文档](README.EN.md)
