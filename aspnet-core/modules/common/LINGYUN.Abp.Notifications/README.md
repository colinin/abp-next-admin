# LINGYUN.Abp.Notifications

实时通知基础模块


#### 注意

* 当前的通知数据模型 NotificationData 设计极度不合理  
  
  将在框架升级为4.0版本之后变更,将与现有通知数据不兼容(这可能就是敏捷开发的避免吧 ;) )

## 配置使用

```csharp
[DependsOn(typeof(AbpNotificationModule))]
public class YouProjectModule : AbpModule
{
  // other
}
