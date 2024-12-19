# LINGYUN.Abp.Identity.WeChat.Work

企业微信用户身份集成模块，提供企业微信用户与ABP Identity系统的集成功能。

## 功能特性

* 企业微信用户身份集成
* 支持企业微信用户信息同步到Identity系统
* 支持企业微信组织架构同步
* 支持自动创建用户

## 模块引用

```csharp
[DependsOn(typeof(AbpIdentityWeChatWorkModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 配置项

```json
{
  "Identity": {
    "WeChat": {
      "Work": {
        "CreateUserIfNotExist": true,    // 用户不存在时是否自动创建
        "UpdateUserIfExist": true,       // 用户存在时是否更新信息
        "SyncOrganizationUnit": true     // 是否同步组织架构
      }
    }
  }
}
```

## 更多文档

* [企业微信用户身份集成文档](README.EN.md)
