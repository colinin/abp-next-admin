# LINGYUN.Abp.Identity.WeChat

WeChat user identity integration module, providing integration between WeChat users and the ABP Identity system.

## Features

* WeChat user identity integration
* Support synchronizing WeChat user information to Identity system
* Support for WeChat UnionId mechanism
* Support automatic user creation

## Module Reference

```csharp
[DependsOn(typeof(AbpIdentityWeChatModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Configuration

```json
{
  "Identity": {
    "WeChat": {
      "CreateUserIfNotExist": true,  // Whether to automatically create user if not exists
      "UpdateUserIfExist": true      // Whether to update user information if exists
    }
  }
}
```

## More Documentation

* [Chinese Documentation](README.md)
