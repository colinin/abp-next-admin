# LINGYUN.Abp.Identity.WeChat

微信用户身份集成模块，提供微信用户与ABP Identity系统的集成功能。

## 功能特性

* 微信用户身份集成
* 支持微信用户信息同步到Identity系统
* 支持微信UnionId机制
* 支持自动创建用户

## 模块引用

```csharp
[DependsOn(typeof(AbpIdentityWeChatModule))]
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
      "CreateUserIfNotExist": true,  // 用户不存在时是否自动创建
      "UpdateUserIfExist": true      // 用户存在时是否更新信息
    }
  }
}
```

## 更多文档

* [微信用户身份集成文档](README.EN.md)
