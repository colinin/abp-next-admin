# LINGYUN.Abp.BackgroundTasks.Abstractions

后台任务（队列）模块抽象层，定义一些基本构造与接口  

## 特性参数  

* DisableJobActionAttribute		标记此特性不处理作业触发后行为  

## 配置使用

模块按需引用

```csharp
[DependsOn(typeof(AbpBackgroundTasksAbstractionsModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```
