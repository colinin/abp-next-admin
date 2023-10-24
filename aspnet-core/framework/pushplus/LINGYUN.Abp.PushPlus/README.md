# LINGYUN.Abp.PushPlus

集成PushPlus  

实现PushPlus相关Api文档,拥有PushPlus开放能力  

详情见PushPlus文档: https://www.pushplus.plus/doc/guide/openApi.html#%E6%96%87%E6%A1%A3%E8%AF%B4%E6%98%8E  

## 模块引用

```csharp
[DependsOn(typeof(AbpPushPlusModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Features

* PushPlus										PushPlus特性分组  
* PushPlus.Message.Enable						全局启用PushPlus消息通道  
* PushPlus.Channel.WeChat						微信公众号消息通道	
* PushPlus.Channel.WeChat.Enable				启用微信公众号消息通道	
* PushPlus.Channel.WeChat.SendLimit				微信公众号消息通道限制次数	
* PushPlus.Channel.WeChat.SendLimitInterval		微信公众号消息通道限制周期(天)	
* PushPlus.Channel.WeWork						企业微信消息通道	
* PushPlus.Channel.WeWork.Enable				启用企业微信消息通道	
* PushPlus.Channel.WeWork.SendLimit				企业微信消息通道限制次数	
* PushPlus.Channel.WeWork.SendLimitInterval		企业微信消息通道限制周期(天)	
* PushPlus.Channel.Webhook						Webhook消息通道	
* PushPlus.Channel.Webhook.Enable				启用Webhook消息通道	
* PushPlus.Channel.Webhook.SendLimit			Webhook消息通道限制次数	
* PushPlus.Channel.Webhook.SendLimitInterval	Webhook消息通道限制周期(天)	
* PushPlus.Channel.Email						Email消息通道	
* PushPlus.Channel.Email.Enable					启用Email消息通道	
* PushPlus.Channel.Email.SendLimit				Email消息通道限制次数	
* PushPlus.Channel.Email.SendLimitInterval		Email消息通道限制周期(天)	
* PushPlus.Channel.Sms							短信消息通道	
* PushPlus.Channel.Sms.Enable					启用短信消息通道	
* PushPlus.Channel.Sms.SendLimit				短信消息通道限制次数	
* PushPlus.Channel.Sms.SendLimitInterval		短信消息通道限制周期(天)	
