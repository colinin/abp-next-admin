# LINGYUN.Abp.Authentication.QQ

QQ Connect authentication module, integrating QQ login functionality into ABP applications.

## Features

* QQ OAuth2.0 authentication
* Support for both mobile and PC login
* Retrieve basic QQ user information (nickname, gender, avatar, etc.)
* Integration with ABP identity system

## Module Dependencies

```csharp
[DependsOn(typeof(AbpAuthenticationQQModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Configuration

```json
{
  "Authentication": {
    "QQ": {
      "AppId": "Your QQ Connect AppId",
      "AppKey": "Your QQ Connect AppKey",
      "IsMobile": false, // Enable mobile login page
      "ClaimsIssuer": "connect.qq.com", // Optional, defaults to connect.qq.com
      "CallbackPath": "/signin-qq", // Optional, defaults to /signin-qq
      "Scope": ["get_user_info"] // Optional, defaults to get_user_info
    }
  }
}
```

## Basic Usage

1. Configure QQ Connect Parameters
   * Apply for an application on QQ Connect platform to get AppId and AppKey
   * Configure AppId and AppKey in appsettings.json

2. Add QQ Login
   ```csharp
   public override void ConfigureServices(ServiceConfigurationContext context)
   {
       context.Services.AddAuthentication()
           .AddQQConnect(); // Add QQ login support
   }
   ```

## Retrieved User Information

* OpenId - Unique QQ user identifier
* NickName - User's nickname
* Gender - User's gender
* AvatarUrl - User's avatar URL

## More Information

* [QQ Connect Documentation](https://wiki.connect.qq.com/)
* [ABP Authentication Documentation](https://docs.abp.io/en/abp/latest/Authentication)
