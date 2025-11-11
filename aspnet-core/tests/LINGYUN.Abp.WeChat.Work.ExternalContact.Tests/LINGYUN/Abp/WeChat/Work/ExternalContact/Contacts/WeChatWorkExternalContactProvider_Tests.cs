using LINGYUN.Abp.WeChat.Work.ExternalContact.Contacts.Request;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact.Contacts;

public class WeChatWorkExternalContactProvider_Tests : AbpWeChatWorkExternalContactTestBase
{
    public readonly IWeChatWorkExternalContactProvider _provider;
    public WeChatWorkExternalContactProvider_Tests()
    {
        _provider = GetRequiredService<IWeChatWorkExternalContactProvider>();
    }

    [Fact]
    public async virtual Task Should_Get_External_Contact_List()
    {
        var res = await _provider.GetExternalContactListAsync(new WeChatWorkGetExternalContactListRequest());

        res.ErrorCode.ShouldBe(0);
        res.ErrorMessage.ShouldBe("ok");
    }
}
