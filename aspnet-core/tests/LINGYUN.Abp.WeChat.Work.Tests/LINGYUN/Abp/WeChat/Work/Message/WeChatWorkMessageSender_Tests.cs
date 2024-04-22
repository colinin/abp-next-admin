using LINGYUN.Abp.WeChat.Work.Messages;
using LINGYUN.Abp.WeChat.Work.Messages.Models;
using LINGYUN.Abp.WeChat.Work.Messages.Templates;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.Work.Message;
public class WeChatWorkMessageSender_Tests : AbpWeChatWorkTestBase
{
    protected IWeChatWorkMessageSender Sender { get; }
    public WeChatWorkMessageSender_Tests()
    {
        Sender = GetRequiredService<IWeChatWorkMessageSender>();
    }

    [Theory]
    [InlineData("1000002")]
    public async Task Send_Text_Message_Test(string agentId)
    {
        var text = new TextMessage("你的快递已到，请携带工卡前往邮件中心领取。\n出发前可查看<a href=\"http://work.weixin.qq.com\">邮件中心视频实况</a>，聪明避开排队。");
        var message = new WeChatWorkTextMessage(agentId, text)
        {
            ToUser = "@all"
        };

        var response = await Sender.SendAsync(message);
        response.IsSuccessed.ShouldBeTrue();
    }

    [Theory]
    [InlineData("1000002", "wr0vTBJwAAFY2xtHp76qFMRDcVXu42qw")]
    public async Task Send_AppChat_Text_Message_Test(string agentId, string chatId)
    {
        var text = new TextMessage("你的快递已到，请携带工卡前往邮件中心领取。\n出发前可查看<a href=\"http://work.weixin.qq.com\">邮件中心视频实况</a>，聪明避开排队。");
        var message = new WeChatWorkAppChatTextMessage(agentId, chatId, text);

        var response = await Sender.SendAsync(message);
        response.IsSuccessed.ShouldBeTrue();
    }

    [Theory]
    [InlineData("1000002")]
    public async Task Send_Markdown_Message_Test(string agentId)
    {
        var markdown = new MarkdownMessage("您的会议室已经预定，稍后会同步到`邮箱`  \n>**事项详情**  \n>事　项：<font color=\"info\">开会</font>  \n>组织者：@miglioguan  \n>参与者：@miglioguan、@kunliu、@jamdeezhou、@kanexiong、@kisonwang  \n>  \n>会议室：<font color=\"info\">广州TIT 1楼 301</font>  \n>日　期：<font color=\"warning\">2018年5月18日</font>  \n>时　间：<font color=\"comment\">上午9:00-11:00</font>  \n>  \n>请准时参加会议。  \n>  \n>如需修改会议信息，请点击：[修改会议信息](https://work.weixin.qq.com)");
        var message = new WeChatWorkMarkdownMessage(agentId, markdown)
        {
            ToUser = "@all"
        };

        var response = await Sender.SendAsync(message);
        response.IsSuccessed.ShouldBeTrue();
    }

    [Theory]
    [InlineData("1000002", "wr0vTBJwAAFY2xtHp76qFMRDcVXu42qw")]
    public async Task Send_AppChat_Markdown_Message_Test(string agentId, string chatId)
    {
        var markdown = new MarkdownMessage("您的会议室已经预定，稍后会同步到`邮箱`  \n>**事项详情**  \n>事　项：<font color=\"info\">开会</font>  \n>组织者：@miglioguan  \n>参与者：@miglioguan、@kunliu、@jamdeezhou、@kanexiong、@kisonwang  \n>  \n>会议室：<font color=\"info\">广州TIT 1楼 301</font>  \n>日　期：<font color=\"warning\">2018年5月18日</font>  \n>时　间：<font color=\"comment\">上午9:00-11:00</font>  \n>  \n>请准时参加会议。  \n>  \n>如需修改会议信息，请点击：[修改会议信息](https://work.weixin.qq.com)");
        var message = new WeChatWorkAppChatMarkdownMessage(agentId, chatId, markdown);

        var response = await Sender.SendAsync(message);
        response.IsSuccessed.ShouldBeTrue();
    }

    [Theory]
    [InlineData("1000002")]
    public async Task Send_Text_Template_Card_Message_Test(string agentId)
    {
        var cardAction = new TemplateCardCardAction("https://work.weixin.qq.com");
        var template = new TextTemplateCard(cardAction, Guid.NewGuid().ToString("D"))
        {
            Source = new TemplateCardSource("图片的url", "图片的url", DescriptionColor.Black),
            ActionMenu = new TemplateCardActionMenu(
                new List<TemplateCardAction>
                {
                    new TemplateCardAction("A","接受推送"),
                    new TemplateCardAction("B","不再推送"),
                },
                "卡片副交互辅助文本说明"),
            MainTitle = new TemplateCardMainTitle("欢迎使用企业微信", "您的好友正在邀请您加入企业微信"),
            QuoteArea = new TemplateCardQuoteArea("https://work.weixin.qq.com", "企业微信的引用样式", "企业微信真好用呀真好用"),
            EmphasisContent = new TemplateCardEmphasisContent("100", "核心数据"),
            SubTitle = "下载企业微信还能抢红包！",
            HorizontalContents = new List<TemplateCardHorizontalContent>
            {
                TemplateCardHorizontalContent.None("邀请人","张三"),
                TemplateCardHorizontalContent.FromUrl("企业微信官网","https://work.weixin.qq.com", "点击访问"),
                //TemplateCardHorizontalContent.FromMedia("企业微信下载","文件的media_id", "企业微信.apk"),
                //TemplateCardHorizontalContent.FromUser("员工信息","点击查看", "张三"),
            },
            Jumps = new List<TemplateCardJump>
            {
                new TemplateCardJump("企业微信官网","https://work.weixin.qq.com"),
                new TemplateCardJump("跳转小程序","跳转小程序","/index.html"),
            },
        };
        var message = new WeChatWorkTemplateCardMessage(agentId, template)
        {
            ToUser = "@all"
        };

        var response = await Sender.SendAsync(message);
        response.IsSuccessed.ShouldBeTrue();
    }
}
