# LINGYUN.Abp.WxPusher

集成WxPusher 

实现WxPusher相关Api文档,拥有WxPusher开放能力  

详情见WxPusher文档: https://wxpusher.dingliqc.com/docs/#/  

## 模块引用

```csharp
[DependsOn(typeof(AbpWxPusherModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 用户订阅

实现 [IWxPusherUserStore](./LINGYUN/Abp/WxPusher/User/IWxPusherUserStore) 接口获取用户订阅列表  

## Features

* WxPusher								WxPusher特性分组  
* WxPusher.Enable						全局启用WxPusher
* WxPusher.Message.Enable				全局启用WxPusher消息通道  
* WxPusher.Message						WxPusher消息推送	
* WxPusher.Message.Enable				启用WxPusher消息推送	
* WxPusher.Message.SendLimit			WxPusher消息推送限制次数	
* WxPusher.Message.SendLimitInterval	WxPusher消息推送限制周期(天)	

## Settings

* WxPusher.Security.AppToken			应用的身份标志,拥有APP_TOKEN，就可以给对应的应用的用户发送消息, 请严格保密.  
