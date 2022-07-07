# LINGYUN.Abp.BackgroundTasks.Jobs

后台任务（队列）常用作业模块  

## 作业列表  

* [ConsoleJob](./LINGYUN/Abp/BackgroundTasks/Jobs/ConsoleJob):							控制台输出  
* [HttpRequestJob](./LINGYUN/Abp/BackgroundTasks/Jobs/HttpRequestJob):					Http请求  
* [SendEmailJob](./LINGYUN/Abp/BackgroundTasks/Jobs/SendEmailJob):						发送邮件  
* [SendSmsJob](./LINGYUN/Abp/BackgroundTasks/Jobs/SendSmsJob):							发送短信  
* [ServiceInvocationJob](./LINGYUN/Abp/BackgroundTasks/Jobs/ServiceInvocationJob):		服务间调用（Http请求的扩展）  
* [SleepJob](./LINGYUN/Abp/BackgroundTasks/Jobs/SleepJob):								休眠,使作业延期执行  

## 配置使用

模块按需引用

```csharp
[DependsOn(typeof(AbpBackgroundTasksJobsModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```
