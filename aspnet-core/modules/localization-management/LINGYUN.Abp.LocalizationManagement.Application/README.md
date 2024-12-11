# LINGYUN.Abp.LocalizationManagement.Application

本地化管理应用服务层实现模块，提供本地化资源管理的应用服务实现。

## 功能特性

* 实现语言管理应用服务
* 实现资源管理应用服务
* 实现文本管理应用服务
* 支持AutoMapper对象映射
* 提供标准化的CRUD操作

## 模块引用

```csharp
[DependsOn(typeof(AbpLocalizationManagementApplicationModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 应用服务

* `LanguageAppService`: 语言管理应用服务
  - 创建语言
  - 更新语言
  - 删除语言
  - 获取语言列表
  - 获取语言详情

* `ResourceAppService`: 资源管理应用服务
  - 创建资源
  - 更新资源
  - 删除资源
  - 获取资源列表
  - 获取资源详情

* `TextAppService`: 文本管理应用服务
  - 创建文本
  - 更新文本
  - 删除文本
  - 获取文本列表
  - 获取文本详情

## 权限

所有应用服务都遵循模块定义的权限要求，详见Domain.Shared模块的权限定义。

## 更多信息

* [English documentation](./README.EN.md)
