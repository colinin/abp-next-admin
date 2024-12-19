# LINGYUN.Abp.Aliyun.SettingManagement

阿里云配置管理模块，引用此模块可管理阿里云相关的配置，可通过网关聚合对外公布的API接口。

## 功能特性

* 提供阿里云服务配置的查询接口
* 支持通过API接口获取阿里云配置信息
* 与ABP设置管理系统集成

## API接口

* **GET api/setting-management/aliyun** - 获取阿里云配置信息

## 模块引用

模块按需引用，建议在配置管理承载服务引用此模块。

```csharp
[DependsOn(typeof(AbpAliyunSettingManagementModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 注意

* 因后台管理模块负责管理所有配置，此模块仅提供查询接口
* 需要与LINGYUN.Abp.Aliyun模块配合使用
* 建议在配置管理承载服务中引用此模块

[点击查看英文文档](README.EN.md)
