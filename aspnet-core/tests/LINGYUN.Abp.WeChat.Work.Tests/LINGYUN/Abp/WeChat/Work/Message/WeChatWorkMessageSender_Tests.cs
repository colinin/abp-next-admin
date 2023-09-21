using LINGYUN.Abp.WeChat.Work.Message.Models;
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
    public async Task Send_Text_Message(string agentId)
    {
        var text = new TextMessage("你的快递已到，请携带工卡前往邮件中心领取。\n出发前可查看<a href=\"http://work.weixin.qq.com\">邮件中心视频实况</a>，聪明避开排队。");
        var message = new WeChatWorkTextMessage(agentId, text)
        {
            ToUser = "@all"
        };

        await Sender.SendAsync(message);
    }

    [Theory]
    [InlineData("1000002")]
    public async Task Send_Markdown_Message(string agentId)
    {
        var markdown = new MarkdownMessage("您的会议室已经预定，稍后会同步到`邮箱`  \n>**事项详情**  \n>事　项：<font color=\"info\">开会</font>  \n>组织者：@miglioguan  \n>参与者：@miglioguan、@kunliu、@jamdeezhou、@kanexiong、@kisonwang  \n>  \n>会议室：<font color=\"info\">广州TIT 1楼 301</font>  \n>日　期：<font color=\"warning\">2018年5月18日</font>  \n>时　间：<font color=\"comment\">上午9:00-11:00</font>  \n>  \n>请准时参加会议。  \n>  \n>如需修改会议信息，请点击：[修改会议信息](https://work.weixin.qq.com)");
        var message = new WeChatWorkMarkdownMessage(agentId, markdown)
        {
            ToUser = "@all"
        };

        await Sender.SendAsync(message);
    }
}
