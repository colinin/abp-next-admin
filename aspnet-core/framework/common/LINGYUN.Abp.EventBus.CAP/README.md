# LINGYUN.Abp.EventBus.CAP

分布式事件总线 CAP 集成

#### 注意

* 自定义事件处理器接口 ICustomDistributedEventSubscriber 未实现,当前的分布式事件只能由 CustomDistributedEventSubscriber 在启动中发现  

* 由于 CAP 不支持同一事件参数的多个消费者,在 ConsumerServiceSelector 接口通过消费者的类全名创建一个新的Group

## 配置使用

```csharp
[DependsOn(typeof(AbpCAPEventBusModule))]
public class YouProjectModule : AbpModule
{
  // other
}

## 变更历史

【2021-03-14】 增加多租户的支持
