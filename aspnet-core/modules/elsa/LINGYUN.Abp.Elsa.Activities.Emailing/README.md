# LINGYUN.Abp.Elsa.Activities.Emailing

Elsa工作流的邮件发送活动集成模块

## 功能

* 提供 **SendEmailing** 活动用于发送电子邮件
  * 支持发送给多个收件人
  * 支持使用模板渲染邮件内容
  * 支持JavaScript和Liquid语法
  * 集成ABP框架的 `IEmailSender` 接口
  * 集成ABP框架的 `ITemplateRenderer` 接口进行模板渲染

## 配置使用

```csharp
[DependsOn(
    typeof(AbpElsaActivitiesEmailingModule)
    )]
public class YouProjectModule : AbpModule
{
}
```

## appsettings.json

```json
{
    "Elsa": {
        "Emailing": true    // 启用邮件发送活动
    }
}
```

## 活动参数

* **To**: 收件人邮箱地址列表
* **Subject**: 邮件主题
* **Body**: 邮件正文内容
* **Culture**: 文化信息
* **Template**: 模板名称
* **Model**: 用于格式化模板内容的模型参数

[English](./README.EN.md)
