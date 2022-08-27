# LINGYUN.Abp.BackgroundTasks.Notifications

后台作业执行后通知事件    

## 配置使用

模块按需引用  

```csharp
[DependsOn(typeof(AbpBackgroundTasksNotificationsModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Action参数

** 通知内容格式化参数,在作业后将发送实时通知  

* push-provider		可选, 用户指定消息推送提供程序;  
* use-template		可选, 用于格式化通知内容的模板名称  
* content			可选, 通知内容（未指定模板名称则为必须参数）  
* culture			可选, 使用模板时的模板区域性  
