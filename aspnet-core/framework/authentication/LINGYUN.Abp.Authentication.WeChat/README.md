# LINGYUN.Abp.Authentication.WeChat

微信公众号认证模块，集成微信公众号登录功能到ABP应用程序。

## 功能特性

* 微信公众号OAuth2.0认证
* 支持获取微信用户基本信息（昵称、性别、地区、头像等）
* 支持UnionId机制，打通公众号与小程序账号体系
* 支持微信服务器消息验证
* 支持与ABP身份系统集成

## 模块引用

```csharp
[DependsOn(typeof(AbpAuthenticationWeChatModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 配置项

```json
{
  "Authentication": {
    "WeChat": {
      "AppId": "你的微信公众号AppId",
      "AppSecret": "你的微信公众号AppSecret",
      "ClaimsIssuer": "WeChat", // 可选，默认为 WeChat
      "CallbackPath": "/signin-wechat", // 可选，默认为 /signin-wechat
      "Scope": ["snsapi_login", "snsapi_userinfo"], // 可选，默认包含 snsapi_login 和 snsapi_userinfo
      "QrConnect": {
        "Enabled": false, // 是否启用PC端扫码登录
        "Endpoint": "https://open.weixin.qq.com/connect/qrconnect" // PC端扫码登录地址
      }
    }
  }
}
```

## 基本用法

1. 配置微信公众号参数
   * 在微信公众平台申请公众号，获取AppId和AppSecret
   * 在appsettings.json中配置AppId和AppSecret

2. 添加微信登录
   ```csharp
   public override void ConfigureServices(ServiceConfigurationContext context)
   {
       context.Services.AddAuthentication()
           .AddWeChat(); // 添加微信登录支持
   }
   ```

3. 启用微信服务器消息验证（可选）
   ```csharp
   public void Configure(IApplicationBuilder app)
   {
       app.UseWeChatSignature(); // 启用微信服务器消息验证中间件
   }
   ```

## 获取的用户信息

* OpenId - 微信用户唯一标识
* UnionId - 微信开放平台唯一标识（需要绑定开放平台）
* NickName - 用户昵称
* Sex - 用户性别
* Country - 国家
* Province - 省份
* City - 城市
* AvatarUrl - 用户头像URL
* Privilege - 用户特权信息

## 更多信息

* [微信公众平台开发文档](https://developers.weixin.qq.com/doc/offiaccount/Getting_Started/Overview.html)
* [ABP认证文档](https://docs.abp.io/en/abp/latest/Authentication)
