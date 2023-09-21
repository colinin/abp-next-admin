using Shouldly;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.Work.Authorize;
public class WeChatWorkUserFinder_Tests : AbpWeChatWorkTestBase
{
    protected IWeChatWorkUserFinder WeChatWorkUserFinder { get; }
    public WeChatWorkUserFinder_Tests()
    {
        WeChatWorkUserFinder = GetRequiredService<IWeChatWorkUserFinder>();
    }

    [Theory]
    [InlineData("1000002", "nuE7XPAh5AJbQ4SawxH0OmUHO_9PzRD-PSghQafeU3A")]
    public async Task GetUserInfo_Test(string agentid, string code)
    {
        var userInfo = await WeChatWorkUserFinder.GetUserInfoAsync(agentid, code);

        userInfo.ShouldNotBeNull();
        userInfo.UserId.ShouldNotBeNullOrWhiteSpace();
        userInfo.UserTicket.ShouldNotBeNullOrWhiteSpace();

        var userDetail = await WeChatWorkUserFinder.GetUserDetailAsync(agentid, userInfo.UserTicket);

        userDetail.ShouldNotBeNull();
        userDetail.UserId.ShouldBe(userInfo.UserId);
        userDetail.QrCode.ShouldNotBeNullOrWhiteSpace();
    }
}
