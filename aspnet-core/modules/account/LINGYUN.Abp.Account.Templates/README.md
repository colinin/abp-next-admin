# LINGYUN.Abp.Account.Templates

ABP账户模块的邮件模板定义模块，提供账户相关的邮件模板功能。

[English](./README.EN.md)

## 功能特性

* 提供标准的账户邮件模板
* 支持邮件模板本地化
* 集成ABP虚拟文件系统
* 使用ABP文本模板系统

## 模块引用

```csharp
[DependsOn(
    typeof(AbpEmailingModule),
    typeof(AbpAccountApplicationContractsModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 邮件模板

### 模板定义

模块定义了以下邮件模板：

* `Abp.Account.MailConfirmLink` - 邮件地址确认模板
  * 用于用户邮箱地址验证
  * 模板文件：`/LINGYUN/Abp/Account/Emailing/Templates/MailConfirm.tpl`
  * 使用标准邮件布局

* `Abp.Account.MailSecurityVerifyLink` - 邮件安全验证模板
  * 用于邮箱安全验证
  * 模板文件：`/LINGYUN/Abp/Account/Emailing/Templates/MailSecurityVerify.tpl`
  * 使用标准邮件布局

### 模板系统

* 使用ABP文本模板系统（TextTemplating）
* 支持模板本地化
* 模板定义提供者：`AccountTemplateDefinitionProvider`

## 本地化

模块包含本地化资源：
* 资源类型：`AccountResource`
* 资源路径：`/LINGYUN/Abp/Account/Templates/Localization/Resources`

## 虚拟文件系统

模块使用ABP虚拟文件系统来管理模板文件：
* 所有模板文件都通过虚拟文件系统嵌入
* 支持模板文件的覆盖和自定义
