# LINGYUN.Abp.LocalizationManagement.Domain.Shared

本地化管理领域层共享模块，定义了错误代码、本地化资源、常量等共享内容。

## 功能特性

* 定义本地化管理相关的常量
* 定义本地化错误代码
* 提供本地化资源文件
* 支持多语言错误消息

## 模块引用

```csharp
[DependsOn(typeof(AbpLocalizationManagementDomainSharedModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 错误代码

* Localization:001100 - 语言 {CultureName} 已经存在
* Localization:001400 - 语言名称 {CultureName} 不存在或内置语言不允许操作
* Localization:002100 - 资源 {Name} 已经存在
* Localization:002400 - 资源名称 {Name} 不存在或内置资源不允许操作

## 本地化资源

模块定义了以下本地化资源：

* DisplayName:Enable - 启用
* DisplayName:CreationTime - 创建时间
* DisplayName:LastModificationTime - 修改时间
* DisplayName:SaveAndNext - 保存并下一步
* Permissions:LocalizationManagement - 本地化管理
* Permissions:Language - 语言管理
* Permissions:Resource - 资源管理
* Permissions:Text - 文档管理
* 等...

## 更多信息

* [English documentation](./README.EN.md)
