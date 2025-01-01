# LINGYUN.Abp.PushPlus

集成PushPlus  

实现PushPlus相关Api文档,拥有PushPlus开放能力  

详情见PushPlus文档: https://www.pushplus.plus/doc/guide/openApi.html#%E6%96%87%E6%A1%A3%E8%AF%B4%E6%98%8E  

## 模块引用

```csharp
[DependsOn(typeof(AbpPushPlusModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Features

* PushPlus										PushPlus特性分组  
* PushPlus.Message.Enable						全局启用PushPlus消息通道  
* PushPlus.Channel.WeChat						微信公众号消息通道	
* PushPlus.Channel.WeChat.Enable				启用微信公众号消息通道	
* PushPlus.Channel.WeChat.SendLimit				微信公众号消息通道限制次数	
* PushPlus.Channel.WeChat.SendLimitInterval		微信公众号消息通道限制周期(天)	
* PushPlus.Channel.WeWork						企业微信消息通道	
* PushPlus.Channel.WeWork.Enable				启用企业微信消息通道	
* PushPlus.Channel.WeWork.SendLimit				企业微信消息通道限制次数	
* PushPlus.Channel.WeWork.SendLimitInterval		企业微信消息通道限制周期(天)	
* PushPlus.Channel.Webhook						Webhook消息通道	
* PushPlus.Channel.Webhook.Enable				启用Webhook消息通道	
* PushPlus.Channel.Webhook.SendLimit			Webhook消息通道限制次数	
* PushPlus.Channel.Webhook.SendLimitInterval	Webhook消息通道限制周期(天)	
* PushPlus.Channel.Email						Email消息通道	
* PushPlus.Channel.Email.Enable					启用Email消息通道	
* PushPlus.Channel.Email.SendLimit				Email消息通道限制次数	
* PushPlus.Channel.Email.SendLimitInterval		Email消息通道限制周期(天)	
* PushPlus.Channel.Sms							短信消息通道	
* PushPlus.Channel.Sms.Enable					启用短信消息通道	
* PushPlus.Channel.Sms.SendLimit				短信消息通道限制次数	
* PushPlus.Channel.Sms.SendLimitInterval		短信消息通道限制周期(天)	

## 配置项

```json
{
  "PushPlus": {
    "Security": {
      "Token": "你的PushPlus Token",      // PushPlus平台获取的Token
      "SecretKey": "你的PushPlus密钥"     // PushPlus平台获取的密钥
    }
  }
}
```

## 基本用法

1. 配置 PushPlus 凭证
   * 在PushPlus平台获取Token和密钥
   * 在配置文件中设置Token和密钥

2. 发送消息
   ```csharp
   public class YourService
   {
       private readonly IPushPlusMessageSender _messageSender;

       public YourService(IPushPlusMessageSender messageSender)
       {
           _messageSender = messageSender;
       }

       public async Task SendMessageAsync()
       {
           // 发送微信消息
           await _messageSender.SendWeChatAsync(
               title: "消息标题",
               content: "消息内容",
               topic: "可选的主题",
               template: PushPlusMessageTemplate.Html
           );

           // 发送企业微信消息
           await _messageSender.SendWeWorkAsync(
               title: "消息标题",
               content: "消息内容"
           );

           // 发送邮件
           await _messageSender.SendEmailAsync(
               title: "邮件标题",
               content: "邮件内容"
           );

           // 发送短信
           await _messageSender.SendSmsAsync(
               title: "短信标题",
               content: "短信内容"
           );

           // 发送Webhook消息
           await _messageSender.SendWebhookAsync(
               title: "消息标题",
               content: "消息内容",
               webhook: "webhook地址"
           );
       }
   }
   ```

## 消息模板

* Html - HTML格式（默认）
* Text - 纯文本格式
* Json - JSON格式
* Markdown - Markdown格式

## 更多信息

* [PushPlus官方文档](https://www.pushplus.plus/doc/guide/openApi.html)
* [ABP框架](https://abp.io/)
