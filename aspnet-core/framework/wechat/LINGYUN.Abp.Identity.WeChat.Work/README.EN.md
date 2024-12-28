# LINGYUN.Abp.Identity.WeChat.Work

WeChat Work (Enterprise WeChat) user identity integration module, providing integration between WeChat Work users and the ABP Identity system.

## Features

* WeChat Work user identity integration
* Support synchronizing WeChat Work user information to Identity system
* Support WeChat Work organizational structure synchronization
* Support automatic user creation

## Module Reference

```csharp
[DependsOn(typeof(AbpIdentityWeChatWorkModule))]
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
      "Work": {
        "CreateUserIfNotExist": true,    // Whether to automatically create user if not exists
        "UpdateUserIfExist": true,       // Whether to update user information if exists
        "SyncOrganizationUnit": true     // Whether to sync organizational structure
      }
    }
  }
}
```

## More Documentation

* [Chinese Documentation](README.md)
