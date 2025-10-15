# 企业微信消息通知

## 功能特性

* [发送应用消息](https://developer.work.weixin.qq.com/document/path/90236)
* [发送群聊消息](https://developer.work.weixin.qq.com/document/path/90248)
* [发送Webhook消息](https://developer.work.weixin.qq.com/document/path/99110)


## 使用方法

- 发送Webhook消息
```csharp
public class Demo
{
	private readonly IWeChatWorkMessageSender _sender;
	
	public Demo(IWeChatWorkMessageSender sender)
	{
		_sender = sender;
	}

	public async Task Send()
	{
		await _sender.SendAsync(
            "693a91f6-7xxx-4bc4-97a0-0ec2sifa5aaa",
            new WeChatWorkWebhookTemplateCardMessage(
                new WebhookTextNoticeCardMessage(
                    TemplateCardAction.Link("https://developer.work.weixin.qq.com/document/path/99110"),
                    new TemplateCardMainTitle("请假通过通知", "您的请假申请已通过,请前往查看!"),
                    source: TemplateCardSource.Black("https://wwcdn.weixin.qq.com/node/wework/images/wecom-logo.a61830413b.svg", "企业微信"),
                    horizontalContents: new List<TemplateCardHorizontalContent>
                    {
                        TemplateCardHorizontalContent.Default("审批单号", "QJ20251000000136"),
                        TemplateCardHorizontalContent.Default("请假日期", "2025/10/01-2025/10/10"),
                        TemplateCardHorizontalContent.Default("通过时间", "2025-10-01 15:30:00"),
                        TemplateCardHorizontalContent.Default("审批备注", "做好考勤及交接事项"),
                    },
                    jumps: new List<TemplateCardJump>
                    {
                        TemplateCardJump.Link("去OA查看", "https://developer.work.weixin.qq.com/document/path/99110")
                    })));
	}
}
```


## 更多文档

* [企业微信文档](https://developer.work.weixin.qq.com/document/path/90664)

