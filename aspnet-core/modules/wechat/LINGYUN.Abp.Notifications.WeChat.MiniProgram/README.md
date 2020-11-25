# LINGYUN.Abp.Notifications.WeChat.MiniProgram

通知发布提供程序的微信小程序实现

大部分重写的模块都和官方模块名称保持一致,通过命名空间区分,主要是只改写了一小部分或者增加额外的功能
如果大部分模块代码都重写,或者完全就是扩展模块,才会定义自己的名字

#### 注意

内置了通知数据 NotificationDataMappings 方法  
可通过 NotificationDataMappings.MappingAll(WeChatMiniProgramNotificationPublishProvider.ProviderName, Func<NotificationData, NotificationData> func) 来自定义规则

## 配置使用

* 此配置项将在下一个微信相关大版本移除,合并到 LINGYUN.Abp.WeChat.MiniProgram.AbpWeChatMiniProgramOptions

```json

{
  "Notifications": {
    "WeChat": {
      "MiniProgram": {
         "DefaultMsgPrefix": "默认消息头部标记",
         "DefaultTemplateId": "默认小程序模板",
         "DefaultState": "默认跳转小程序类型",
         "DefaultLanguage": "默认小程序语言"
      }
    }
  }
}

```


```csharp
[DependsOn(typeof(AbpNotificationsWeChatMiniProgramModule))]
public class YouProjectModule : AbpModule
{
  // other
}
