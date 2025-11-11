using LINGYUN.Abp.WeChat.Work.ExternalContact.GroupChats.Request;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.GroupChats;
public class WeChatWorkGroupChatProvider_Tests : AbpWeChatWorkExternalContactTestBase
{
    public readonly IWeChatWorkGroupChatProvider _provider;
    public readonly IConfiguration _configuration;
    public WeChatWorkGroupChatProvider_Tests()
    {
        _provider = GetRequiredService<IWeChatWorkGroupChatProvider>();
        _configuration = GetRequiredService<IConfiguration>();
    }

    [Fact]
    public async virtual Task Should_Get_Group_Chat_List()
    {
        var res = await _provider.GetGroupChatListAsync(
            new WeChatWorkGetGroupChatListRequest());

        res.ErrorCode.ShouldBe(0);
        res.ErrorMessage.ShouldBe("ok");
    }

    [Fact]
    public async virtual Task Should_Get_Group_Chat()
    {
        var chatId = _configuration["WeChat:Work:ExternalContact:GroupChats:GetGroupChat:ChatId"];
        var res = await _provider.GetGroupChatAsync(
            new WeChatWorkGetGroupChatRequest(chatId, true));

        res.ErrorCode.ShouldBe(0);
        res.ErrorMessage.ShouldBe("ok");
    }

    [Fact]
    public async virtual Task Should_OpengId_To_ChatId()
    {
        var opengId = _configuration["WeChat:Work:ExternalContact:GroupChats:GetGroupChat:OpengIdToChatId"];
        var res = await _provider.OpengIdToChatIdAsync(
            new WeChatWorkOpengIdToChatIdRequest(opengId));

        res.ErrorCode.ShouldBe(0);
        res.ErrorMessage.ShouldBe("ok");
    }
}
