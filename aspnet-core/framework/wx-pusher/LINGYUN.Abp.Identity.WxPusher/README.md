# LINGYUN.Abp.Identity.WxPusher

IWxPusherUserStore 接口的Identity模块实现, 通过用户Claims来获取关注的topic列表  

## 模块引用

```csharp
[DependsOn(typeof(AbpIdentityWxPusherModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```
