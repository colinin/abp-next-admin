# LINGYUN.Abp.Aliyun.Features

Alibaba Cloud service feature management module.

## Features

* Provides feature definitions and management for Alibaba Cloud services
* Supports enabling/disabling Alibaba Cloud service features
* Integration with ABP feature management system

## Module Reference

```csharp
[DependsOn(typeof(AbpAliyunFeaturesModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Feature Items

* **Features:AlibabaCloud** - Alibaba Cloud service feature group
  * **Features:AlibabaCloud:IsEnabled** - Enable/Disable Alibaba Cloud services
    * Default value: false
    * Description: Enable to give the application Alibaba Cloud service capabilities

## Configuration Items

This module is mainly used for feature definition and does not contain configuration items.

## Notes

* This module needs to be used in conjunction with the LINGYUN.Abp.Aliyun module
* After enabling Alibaba Cloud service features, you still need to configure the corresponding service parameters in the LINGYUN.Abp.Aliyun module

[查看中文文档](README.md)
