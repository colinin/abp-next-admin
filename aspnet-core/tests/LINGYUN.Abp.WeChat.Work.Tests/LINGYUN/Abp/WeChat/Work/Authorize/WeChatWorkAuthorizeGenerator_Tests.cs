using System.Threading.Tasks;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.WeChat.Work.Authorize;
public class WeChatWorkAuthorizeGenerator_Tests : AbpWeChatWorkTestBase
{
    protected ISettingProvider SettingProvider { get; }
    protected IWeChatWorkAuthorizeGenerator AuthorizeGenerator { get; }

    public WeChatWorkAuthorizeGenerator_Tests()
    {
        SettingProvider = GetRequiredService<ISettingProvider>();
        AuthorizeGenerator = GetRequiredService<IWeChatWorkAuthorizeGenerator>();
    }

    [Theory]
    [InlineData("http://api.3dept.com/cgi-bin/query?action=get", "sdjnvh83tg93fojve2g92gyuh29", "code", "snsapi_privateinfo")]
    public async Task GenerateOAuth2Authorize_Test(
        string redirectUri,
        string state,
        string responseType = "code",
        string scope = "snsapi_base")
    {
        var url = await AuthorizeGenerator.GenerateOAuth2AuthorizeAsync(redirectUri, state, responseType, scope);
        url.ShouldNotBeNullOrWhiteSpace();
    }

    [Theory]
    [InlineData("http://api.3dept.com/cgi-bin/query?action=get", "sdjnvh83tg93fojve2g92gyuh29", "CorpApp", "zh")]
    public async Task GenerateOAuth2Login_Test(
        string agentid,
        string redirectUri,
        string state,
        string loginType = "CorpApp",
        string lang = "zh")
    {
        var url = await AuthorizeGenerator.GenerateOAuth2LoginAsync(redirectUri, state, loginType, agentid, lang);
        url.ShouldNotBeNullOrWhiteSpace();
    }
}
