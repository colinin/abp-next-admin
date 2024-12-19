# LINGYUN.Abp.Aliyun.Features

阿里云服务功能管理模块。

## 功能特性

* 提供阿里云服务的功能定义和管理
* 支持启用/禁用阿里云服务功能
* 与ABP功能管理系统集成

## 模块引用

```csharp
[DependsOn(typeof(AbpAliyunFeaturesModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 功能项

* **Features:AlibabaCloud** - 阿里云服务功能组
  * **Features:AlibabaCloud:IsEnabled** - 是否启用阿里云服务
    * 默认值：false
    * 描述：启用使应用程序拥有阿里云服务的能力

## 配置项

此模块主要用于功能定义，不包含配置项。

## 注意

* 此模块需要与LINGYUN.Abp.Aliyun模块配合使用
* 启用阿里云服务功能后，还需要在LINGYUN.Abp.Aliyun模块中配置相应的服务参数

[点击查看英文文档](README.EN.md)
