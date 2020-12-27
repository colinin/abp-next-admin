# LINGYUN.Abp.EventBus.CAP

分布式事件总线 CAP 集成

#### 注意

* 自定义事件处理器接口 ICustomDistributedEventSubscriber 未实现,当前的分布式事件只能由 CustomDistributedEventSubscriber 在启动中发现  

* 由于Abp建议将 EventNameAttribute 标注在事件对象类,但是CAP的建议是标注在事件订阅者 EventHandler之上  
  后者有一个好处就是可以在一个启动项目中有多个订阅者消费同一个事件对象  

  模块提供一个可选配置项，它指定了事件名称将从哪里解析 AbpCAPEventBusOptions.NameInEventDataType
  如果为 true:		事件名称将从事件对象类型中获取,比如 Volo.Abp.Users.UserEto
  如果为 false:		事件名称将从事件消费者类型中获取,比如 Volo.Abp.Users.EventBus.Handlers.UsersSynchronizer
  默认为 true,遵循Abp的文档,如果需要请自行配置为 false 来保持和 CAP 相同的习性

## 配置使用

```csharp
[DependsOn(typeof(AbpCAPEventBusModule))]
public class YouProjectModule : AbpModule
{
  // other
}
