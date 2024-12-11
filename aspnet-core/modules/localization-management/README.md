# Localization Management

本地化文档管理模块,因项目路径太长Windows系统不支持,项目目录取简称 lt  

## 功能特性

* 支持动态管理本地化资源
* 支持多语言管理（增删改查）
* 支持资源管理（增删改查）
* 支持文本管理（增删改查）
* 支持本地化资源的内存缓存
* 支持分布式缓存同步
* 提供标准的RESTful API接口
* 支持与ABP框架无缝集成

## 模块说明

### 基础模块

* [LINGYUN.Abp.Localization.Persistence](../localization/LINGYUN.Abp.Localization.Persistence)					本地化持久化模块,实现IStaticLocalizationSaver接口以将本地静态资源持久化到存储设施  
* [LINGYUN.Abp.LocalizationManagement.Domain.Shared](./LINGYUN.Abp.LocalizationManagement.Domain.Shared)					领域层公共模块，定义了错误代码、本地化、模块设置  
* [LINGYUN.Abp.LocalizationManagement.Domain](./LINGYUN.Abp.LocalizationManagement.Domain)								领域层模块，实现 ILocalizationStore 接口  
* [LINGYUN.Abp.LocalizationManagement.EntityFrameworkCore](./LINGYUN.Abp.LocalizationManagement.EntityFrameworkCore)								数据访问层模块,集成EfCore  
* [LINGYUN.Abp.LocalizationManagement.Application.Contracts](./LINGYUN.Abp.LocalizationManagement.Application.Contracts)	应用服务层公共模块，定义了管理本地化对象的外部接口、权限、功能限制策略  
* [LINGYUN.Abp.LocalizationManagement.Application](./LINGYUN.Abp.LocalizationManagement.Application)						应用服务层实现，实现了本地化对象管理接口  
* [LINGYUN.Abp.LocalizationManagement.HttpApi](./LINGYUN.Abp.LocalizationManagement.HttpApi)								RestApi实现，实现了独立的对外RestApi接口  

### 高阶模块

暂无高阶模块。

### 权限定义

* LocalizationManagement.Resource						授权对象是否允许访问资源  
* LocalizationManagement.Resource.Create		授权对象是否允许创建资源  
* LocalizationManagement.Resource.Update		授权对象是否允许修改资源  
* LocalizationManagement.Resource.Delete		授权对象是否允许删除资源  
* LocalizationManagement.Language						授权对象是否允许访问语言  
* LocalizationManagement.Language.Create		授权对象是否允许创建语言  
* LocalizationManagement.Language.Update		授权对象是否允许修改语言  
* LocalizationManagement.Language.Delete		授权对象是否允许删除语言  
* LocalizationManagement.Text						    授权对象是否允许访问文档  
* LocalizationManagement.Text.Create				授权对象是否允许创建文档  
* LocalizationManagement.Text.Update				授权对象是否允许修改文档  
* LocalizationManagement.Text.Delete				授权对象是否允许删除文档  

### 配置定义

```json
{
  "LocalizationManagement": {
    "LocalizationCacheStampTimeOut": "00:02:00",  // 本地化缓存时间戳超时时间，默认2分钟
    "LocalizationCacheStampExpiration": "00:30:00" // 本地化缓存过期时间，默认30分钟
  }
}
```

### 数据库表

本模块使用以下数据库表存储本地化数据：

* Languages - 语言表
* Resources - 资源表
* Texts - 文本表

可通过 `LocalizationModelBuilderConfigurationOptions` 配置表前缀和Schema：

```csharp
public class LocalizationModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
{
    public LocalizationModelBuilderConfigurationOptions(
       string tablePrefix = "",
       string schema = null)
       : base(tablePrefix, schema)
    {
    }
}
```

### API接口

本模块提供以下REST API接口：

* `/api/localization-management/languages` - 语言管理相关API
* `/api/localization-management/resources` - 资源管理相关API
* `/api/localization-management/texts` - 文本管理相关API

## 错误代码

* Localization:001100 - 语言 {CultureName} 已经存在
* Localization:001400 - 语言名称 {CultureName} 不存在或内置语言不允许操作
* Localization:002100 - 资源 {Name} 已经存在
* Localization:002400 - 资源名称 {Name} 不存在或内置资源不允许操作

## 更多信息

* [English documentation](./README.EN.md)

## 更新日志

### 2024.12
* 完善模块文档
* 添加英文文档支持
