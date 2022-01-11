# LINGYUN.Abp.BackgroundTasks.ExceptionHandling

后台作业执行异常通知实现, 默认使用Email发送通知    

## 配置使用

模块按需引用  

```csharp
[DependsOn(typeof(AbpBackgroundTasksExceptionHandlingModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 先决条件

** 在定义作业时在参数中指定如下参数,在作业执行失败时将发送通知  

* exception.to			必须, 接收者邮件地址  
* exception.from		必须, 邮件抬头发送者名称  
* exception.body		可选, 邮件内容（未指定模板名称则为必须参数）  
* exception.subject		必须, 邮件标题
* exception.template	可选, 邮件模板  
* exception.context		可选, 使用模板时的上下文参数  
* exception.culture		可选, 使用模板时的模板区域性  
