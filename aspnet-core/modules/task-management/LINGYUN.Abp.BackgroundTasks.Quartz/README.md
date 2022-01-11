# LINGYUN.Abp.BackgroundTasks.Quartz

后台任务（队列）模块的Quartz实现.  
并添加一个监听器用于通知管理者任务状态  

## 配置使用

模块按需引用,具体配置参考Volo.Abp.Quartz模块  

```csharp
[DependsOn(typeof(AbpBackgroundTasksQuartzModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```
