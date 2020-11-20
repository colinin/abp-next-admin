# LINGYUN.Abp.IdentityServer.WeChatValidator

废弃模块,模块层次不清晰,微信有多端平台,不同平台授权规则不一致

#### 注意



## 配置使用


```csharp
[DependsOn(typeof(AbpIdentityServerWeChatValidatorModule))]
public class YouProjectModule : AbpModule
{
  // other
}
