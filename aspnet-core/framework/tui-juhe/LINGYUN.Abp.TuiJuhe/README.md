# LINGYUN.Abp.TuiJuhe

集成TuiJuhe（仅适用于特定消息与人员通知, 不支持群发）

实现TuiJuhe相关Api文档,拥有TuiJuhe开放能力  

详情见TuiJuhe文档: https://tui.juhe.cn/docs 

## 模块引用

```csharp
[DependsOn(typeof(AbpTuiJuheModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Features

* TuiJuhe								TuiJuhe特性分组  
* TuiJuhe.Enable						全局启用TuiJuhe
* TuiJuhe.Message.Enable				全局启用TuiJuhe消息通道  
* TuiJuhe.Message						TuiJuhe消息推送	
* TuiJuhe.Message.Enable				启用TuiJuhe消息推送	
* TuiJuhe.Message.SendLimit				TuiJuhe消息推送限制次数	
* TuiJuhe.Message.SendLimitInterval		TuiJuhe消息推送限制周期(时)	

## Settings

* TuiJuhe.Security.Token			用户令牌.  
