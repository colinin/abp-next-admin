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

## Action参数

** 在定义作业时在参数中指定如下参数,在作业执行失败时将发送通知  

* to		必须, 接收者邮件地址  
* from		可选, 邮件抬头发送者名称  
* body		可选, 邮件内容（未指定模板名称则为必须参数）  
* subject	可选, 邮件标题
* template	可选, 邮件模板  
* context	可选, 使用模板时的上下文参数  
* culture	可选, 使用模板时的模板区域性  
