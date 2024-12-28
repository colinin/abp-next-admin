# LINGYUN.Abp.LocalizationManagement.Application.Contracts

本地化管理应用服务层契约模块，定义了本地化管理的应用服务接口、DTO对象和权限定义。

## 功能特性

* 定义语言管理应用服务接口
* 定义资源管理应用服务接口
* 定义文本管理应用服务接口
* 定义权限和授权
* 提供DTO对象定义

## 模块引用

```csharp
[DependsOn(typeof(AbpLocalizationManagementApplicationContractsModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 权限定义

* LocalizationManagement.Resource - 授权对象是否允许访问资源
* LocalizationManagement.Resource.Create - 授权对象是否允许创建资源
* LocalizationManagement.Resource.Update - 授权对象是否允许修改资源
* LocalizationManagement.Resource.Delete - 授权对象是否允许删除资源
* LocalizationManagement.Language - 授权对象是否允许访问语言
* LocalizationManagement.Language.Create - 授权对象是否允许创建语言
* LocalizationManagement.Language.Update - 授权对象是否允许修改语言
* LocalizationManagement.Language.Delete - 授权对象是否允许删除语言
* LocalizationManagement.Text - 授权对象是否允许访问文档
* LocalizationManagement.Text.Create - 授权对象是否允许创建文档
* LocalizationManagement.Text.Update - 授权对象是否允许修改文档
* LocalizationManagement.Text.Delete - 授权对象是否允许删除文档

## 应用服务接口

* `ILanguageAppService`: 语言管理应用服务接口
* `IResourceAppService`: 资源管理应用服务接口
* `ITextAppService`: 文本管理应用服务接口

## 更多信息

* [English documentation](./README.EN.md)
