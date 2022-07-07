# LINGYUN.Abp.BackgroundTasks.EventBus

后台任务（队列）模块分布式事件模块,集成模块使应用程序具备处理作业事件能力  

## 配置使用

模块按需引用

```csharp
[DependsOn(typeof(AbpBackgroundTasksEventBusModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```
