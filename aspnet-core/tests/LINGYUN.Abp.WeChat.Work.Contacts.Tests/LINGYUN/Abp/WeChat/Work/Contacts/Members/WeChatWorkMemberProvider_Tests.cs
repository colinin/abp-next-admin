using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.Work.Contacts.Members;
public class WeChatWorkMemberProvider_Tests : AbpWeChatWorkContactTestBase
{
    public readonly IWeChatWorkMemberProvider _provider;
    public readonly IConfiguration _configuration;
    public WeChatWorkMemberProvider_Tests()
    {
        _provider = GetRequiredService<IWeChatWorkMemberProvider>();
        _configuration = GetRequiredService<IConfiguration>();
    }

    [Fact]
    public async virtual Task Should_Get_Member()
    {
        var userId = _configuration["WeChat:Work:Contacts:Members:GetMember:UserId"];
        var res = await _provider.GetMemberAsync(userId);

        res.ErrorCode.ShouldBe(0);
        res.ErrorMessage.ShouldBe("ok");
    }
}