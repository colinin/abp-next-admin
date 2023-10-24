# LINGYUN.Abp.Notifications.Sms

通知发布提供程序的短信实现

大部分重写的模块都和官方模块名称保持一致,通过命名空间区分,主要是只改写了一小部分或者增加额外的功能
如果大部分模块代码都重写,或者完全就是扩展模块,才会定义自己的名字

#### 注意

自定义的发送方法可以通过实现 ##ISmsNotificationSender## 接口或重写 ##SmsNotificationSender## 即可

内置了通知数据 NotificationDataMappings 方法  
可通过 NotificationDataMappings.MappingAll(SmsNotificationPublishProvider.ProviderName, Func<NotificationData, NotificationData> func) 来自定义规则


## 配置使用

* 此配置项将在下一个短信相关大版本移除

```json

{
  "Notifications": {
    "Sms": {
      "TemplateParamsPrefix": "短信模板变量前缀"
    }
  }
}

```

```csharp
[DependsOn(typeof(AbpNotificationsSmsModule))]
public class YouProjectModule : AbpModule
{
  // other
}
