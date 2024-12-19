# LINGYUN.Abp.Authentication.QQ

QQ互联认证模块，集成QQ登录功能到ABP应用程序。

## 功能特性

* QQ OAuth2.0认证
* 支持移动端和PC端登录
* 获取QQ用户基本信息（昵称、性别、头像等）
* 支持与ABP身份系统集成

## 模块引用

```csharp
[DependsOn(typeof(AbpAuthenticationQQModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 配置项

```json
{
  "Authentication": {
    "QQ": {
      "AppId": "你的QQ互联AppId",
      "AppKey": "你的QQ互联AppKey",
      "IsMobile": false, // 是否启用移动端登录页面
      "ClaimsIssuer": "connect.qq.com", // 可选，默认为 connect.qq.com
      "CallbackPath": "/signin-qq", // 可选，默认为 /signin-qq
      "Scope": ["get_user_info"] // 可选，默认为 get_user_info
    }
  }
}
```

## 基本用法

1. 配置QQ互联参数
   * 在QQ互联平台申请应用，获取AppId和AppKey
   * 在appsettings.json中配置AppId和AppKey

2. 添加QQ登录
   ```csharp
   public override void ConfigureServices(ServiceConfigurationContext context)
   {
       context.Services.AddAuthentication()
           .AddQQConnect(); // 添加QQ登录支持
   }
   ```

## 获取的用户信息

* OpenId - QQ用户唯一标识
* NickName - 用户昵称
* Gender - 用户性别
* AvatarUrl - 用户头像URL

## 更多信息

* [QQ互联文档](https://wiki.connect.qq.com/)
* [ABP认证文档](https://docs.abp.io/en/abp/latest/Authentication)
