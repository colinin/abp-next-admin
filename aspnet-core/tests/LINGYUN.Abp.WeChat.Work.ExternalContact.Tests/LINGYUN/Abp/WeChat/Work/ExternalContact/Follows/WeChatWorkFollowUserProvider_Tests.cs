using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Follows;
public class WeChatWorkFollowUserProvider_Tests : AbpWeChatWorkExternalContactTestBase
{
    public readonly IWeChatWorkFollowUserProvider _provider;
    public WeChatWorkFollowUserProvider_Tests()
    {
        _provider = GetRequiredService<IWeChatWorkFollowUserProvider>();
    }

    [Fact]
    public async virtual Task Should_Get_Follow_User_List()
    {
        var res = await _provider.GetFollowUserListAsync();

        res.ErrorCode.ShouldBe(0);
        res.ErrorMessage.ShouldBe("ok");
    }
}
