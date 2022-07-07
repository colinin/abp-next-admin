# LINGYUN.Abp.BackgroundTasks.Activities

后台任务（队列）模块行为处理模块    

## 接口参数  

* IJobActionStore				实现此接口获取作业管理行为  
* JobActionDefinitionProvider	实现此接口自定义作业行为  
* JobExecutedProvider			实现此接口扩展作业行为  

## 配置使用

模块按需引用

```csharp
[DependsOn(typeof(AbpBackgroundTasksActivitiesModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```
