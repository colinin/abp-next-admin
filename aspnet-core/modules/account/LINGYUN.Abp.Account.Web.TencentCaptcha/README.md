# LINGYUN.Abp.Account.Web.TencentCaptcha

腾讯云天御验证码组件集成模块,可使用腾讯云验证码能力  

## 模块引用

```csharp
[DependsOn(typeof(AbpAccountWebTencentCaptchaModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 注意事项

` 请在启动项目或系统设置中配置腾讯云相关参数,否则造成登录页验证码组件无法加载  

*	Abp.TencentCloud.SecretId      腾讯云账号SecretId, 需加密
*	Abp.TencentCloud.SecretKey            腾讯云账号SecretKey, 需加密
*	Abp.TencentCloud.Captcha.CaptchaAppId     验证码业务ID, 需加密
*	Abp.TencentCloud.Captcha.AppSecretKey     原始密钥（25位）, 需加密
*	Abp.TencentCloud.Captcha.CaptchaAppIdEncryptedType    加密方式，大小写不敏感："cbc"、 "gcm"，不指定时前端验证器组件不加密
*	Abp.TencentCloud.Captcha.CaptchaAppIdEncryptedExpireTime   过期时间（秒），最大值 86400 秒，即 24 小时

示例配置：

```json
{
  "Settings": {
    "Abp.TencentCloud.SecretId": "XXXXXXXXXXXXXX",
    "Abp.TencentCloud.SecretKey": "YYYYYYYYYYYYY",
    "Abp.TencentCloud.Captcha.CaptchaAppId": "XXXXXXXXXXXX",
    "Abp.TencentCloud.Captcha.AppSecretKey": "XXXXXXXXXXXXX",
    "Abp.TencentCloud.Captcha.CaptchaAppIdEncryptedType": "gcm",
    "Abp.TencentCloud.Captcha.CaptchaAppIdEncryptedExpireTime": 300
  }
}
```
