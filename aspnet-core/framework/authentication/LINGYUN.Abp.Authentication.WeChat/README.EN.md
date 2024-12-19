# LINGYUN.Abp.Authentication.WeChat

WeChat Official Account authentication module, integrating WeChat Official Account login functionality into ABP applications.

## Features

* WeChat Official Account OAuth2.0 authentication
* Retrieve WeChat user information (nickname, gender, region, avatar, etc.)
* Support for UnionId mechanism, connecting Official Account and Mini Program account systems
* Support for WeChat server message verification
* Integration with ABP identity system

## Module Dependencies

```csharp
[DependsOn(typeof(AbpAuthenticationWeChatModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Configuration

```json
{
  "Authentication": {
    "WeChat": {
      "AppId": "Your WeChat Official Account AppId",
      "AppSecret": "Your WeChat Official Account AppSecret",
      "ClaimsIssuer": "WeChat", // Optional, defaults to WeChat
      "CallbackPath": "/signin-wechat", // Optional, defaults to /signin-wechat
      "Scope": ["snsapi_login", "snsapi_userinfo"], // Optional, defaults to snsapi_login and snsapi_userinfo
      "QrConnect": {
        "Enabled": false, // Enable PC-side QR code login
        "Endpoint": "https://open.weixin.qq.com/connect/qrconnect" // PC-side QR code login endpoint
      }
    }
  }
}
```

## Basic Usage

1. Configure WeChat Official Account Parameters
   * Register an Official Account on WeChat Official Account Platform to get AppId and AppSecret
   * Configure AppId and AppSecret in appsettings.json

2. Add WeChat Login
   ```csharp
   public override void ConfigureServices(ServiceConfigurationContext context)
   {
       context.Services.AddAuthentication()
           .AddWeChat(); // Add WeChat login support
   }
   ```

3. Enable WeChat Server Message Verification (Optional)
   ```csharp
   public void Configure(IApplicationBuilder app)
   {
       app.UseWeChatSignature(); // Enable WeChat server message verification middleware
   }
   ```

## Retrieved User Information

* OpenId - Unique WeChat user identifier
* UnionId - WeChat Open Platform unique identifier (requires binding to Open Platform)
* NickName - User's nickname
* Sex - User's gender
* Country - User's country
* Province - User's province
* City - User's city
* AvatarUrl - User's avatar URL
* Privilege - User's privilege information

## More Information

* [WeChat Official Account Platform Documentation](https://developers.weixin.qq.com/doc/offiaccount/Getting_Started/Overview.html)
* [ABP Authentication Documentation](https://docs.abp.io/en/abp/latest/Authentication)
