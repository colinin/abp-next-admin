# LINGYUN.Abp.Elsa.Activities.Sms

Elsa工作流的短信活动集成模块

## 功能

* 提供 **SendSms** 活动用于发送短信
  * 支持发送给多个手机号码
  * 支持自定义消息属性
  * 支持JavaScript、JSON和Liquid语法
  * 集成ABP框架的 `ISmsSender` 接口

## 配置使用

```csharp
[DependsOn(
    typeof(AbpElsaActivitiesSmsModule)
    )]
public class YouProjectModule : AbpModule
{
}
```

## appsettings.json

```json
{
    "Elsa": {
        "Sms": true    // 启用短信活动
    }
}
```

## 活动参数

* **To**: 接收手机号码列表
* **Message**: 短信内容
* **Properties**: 附加属性（高级选项）

[English](./README.EN.md)
