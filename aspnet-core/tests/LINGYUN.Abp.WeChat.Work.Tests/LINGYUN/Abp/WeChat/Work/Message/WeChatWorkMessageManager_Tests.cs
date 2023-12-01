using LINGYUN.Abp.WeChat.Work.Messages;
using LINGYUN.Abp.WeChat.Work.Messages.Models;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.Work.Message;
public class WeChatWorkMessageManager_Tests : AbpWeChatWorkTestBase
{
    protected IWeChatWorkMessageSender Sender { get; }
    protected IWeChatWorkMessageManager Manager { get; }
    public WeChatWorkMessageManager_Tests()
    {
        Sender = GetRequiredService<IWeChatWorkMessageSender>();
        Manager = GetRequiredService<IWeChatWorkMessageManager>();
    }

    [Theory]
    [InlineData("1000002")]
    public async Task ReCall_Message_Test(string agentId)
    {
        var text = new TextMessage("发送一条用于测试撤回接口的消息");
        var message = new WeChatWorkTextMessage(agentId, text)
        {
            ToUser = "@all"
        };

        var response = await Sender.SendAsync(message);
        response.IsSuccessed.ShouldBeTrue();

        var recallRet = await Manager.ReCallMessageAsync(agentId, response.MsgId);
        recallRet.ShouldBeTrue();
    }
}
