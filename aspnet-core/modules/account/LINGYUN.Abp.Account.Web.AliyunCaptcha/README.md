# LINGYUN.Abp.Account.Web.AliyunCaptcha

阿里云验证码组件集成模块,可使用阿里云验证码能力  

## 模块引用

```csharp
[DependsOn(typeof(AbpAccountWebAliyunCaptchaModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 注意事项

` 请在启动项目或系统设置中配置阿里云相关参数,否则造成登录页验证码组件无法加载  

*	Abp.Aliyun.Authorization.RegionId      阿里云访问控制相关配置,具体参考AbpAliyunModule模块
*	Abp.Aliyun.Authorization.AccessKeyId      阿里云访问控制相关配置,具体参考AbpAliyunModule模块
*	Abp.Aliyun.Authorization.AccessKeySecret      阿里云访问控制相关配置,具体参考AbpAliyunModule模块
*	Abp.Aliyun.Authorization.UseSecurityTokenService      阿里云访问控制相关配置,具体参考AbpAliyunModule模块
*	Abp.Aliyun.Authorization.RamRoleArn      阿里云访问控制相关配置,具体参考AbpAliyunModule模块
*	Abp.Aliyun.Authorization.RoleSessionName      阿里云访问控制相关配置,具体参考AbpAliyunModule模块
*	Abp.Aliyun.Authorization.DurationSeconds      阿里云访问控制相关配置,具体参考AbpAliyunModule模块
*	Abp.Aliyun.Authorization.Policy      阿里云访问控制相关配置,具体参考AbpAliyunModule模块
*	Abp.Aliyun.Captcha.SceneId     验证码场景的唯一标识, 需加密
*	Abp.Aliyun.Captcha.Prefix     控制台获取的身份标, 需加密
*	Abp.Aliyun.Captcha.UseEncryptedSceneId    使用加密验证码场景标识
*	Abp.Aliyun.Captcha.EKey    控制台获取的ekey, 需加密
*	Abp.Aliyun.Captcha.EncryptedExpireTimeSec   密文过期时间，单位秒，范围 1~86400

示例配置：

```json
{
  "Settings": {
    "Abp.Aliyun.Authorization.RegionId": "XXXXXXXXXXXXXX",
    "Abp.Aliyun.Authorization.AccessKeyId": "XXXXXXXXXXXXXX",
    "Abp.Aliyun.Authorization.AccessKeySecret": "XXXXXXXXXXXXXX",
    "Abp.Aliyun.Authorization.UseSecurityTokenService": true,
    "Abp.Aliyun.Authorization.RamRoleArn": "XXXXXXXXXXXXXX",
    "Abp.Aliyun.Authorization.RoleSessionName": "XXXXXXXXXXXXXX",
    "Abp.Aliyun.Authorization.DurationSeconds": 3600,
    "Abp.Aliyun.Authorization.Policy": null,
    "Abp.Aliyun.Captcha.SceneId": "YYYYYYYYYYYYY",
    "Abp.Aliyun.Captcha.Prefix": "YYYYYYYYYYYYY",
    "Abp.Aliyun.Captcha.UseEncryptedSceneId": true,
    "Abp.Aliyun.Captcha.EKey": "YYYYYYYYYYYYY",
    "Abp.Aliyun.Captcha.EncryptedExpireTimeSec": 300
  }
}
```
