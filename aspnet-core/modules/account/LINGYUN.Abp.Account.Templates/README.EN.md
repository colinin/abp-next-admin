# LINGYUN.Abp.Account.Templates

Email template definition module for the ABP account module, providing account-related email template functionality.

[简体中文](./README.md)

## Features

* Provides standard account email templates
* Supports email template localization
* Integrates with ABP Virtual File System
* Uses ABP Text Template System

## Module Dependencies

```csharp
[DependsOn(
    typeof(AbpEmailingModule),
    typeof(AbpAccountApplicationContractsModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Email Templates

### Template Definitions

The module defines the following email templates:

* `Abp.Account.MailConfirmLink` - Email Address Confirmation Template
  * Used for user email address verification
  * Template file: `/LINGYUN/Abp/Account/Emailing/Templates/MailConfirm.tpl`
  * Uses standard email layout

* `Abp.Account.MailSecurityVerifyLink` - Email Security Verification Template
  * Used for email security verification
  * Template file: `/LINGYUN/Abp/Account/Emailing/Templates/MailSecurityVerify.tpl`
  * Uses standard email layout

### Template System

* Uses ABP Text Template System (TextTemplating)
* Supports template localization
* Template definition provider: `AccountTemplateDefinitionProvider`

## Localization

The module includes localization resources:
* Resource type: `AccountResource`
* Resource path: `/LINGYUN/Abp/Account/Templates/Localization/Resources`

## Virtual File System

The module uses ABP Virtual File System to manage template files:
* All template files are embedded through the virtual file system
* Supports template file override and customization
