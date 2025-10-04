using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Security.Encryption;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.Users;

namespace LINGYUN.Abp.WeChat.Work.Authorize;

[IntegrationService]
public class WeChatWorkAuthorizeAppService : ApplicationService, IWeChatWorkAuthorizeAppService
{
    private readonly IAppUrlProvider _appUrlProvider;
    private readonly IStringEncryptionService _encryptionService;
    private readonly IWeChatWorkAuthorizeGenerator _authorizeGenerator;

    public WeChatWorkAuthorizeAppService(
        IAppUrlProvider appUrlProvider,
        IStringEncryptionService encryptionService,
        IWeChatWorkAuthorizeGenerator authorizeGenerator)
    {
        _appUrlProvider = appUrlProvider;
        _encryptionService = encryptionService;
        _authorizeGenerator = authorizeGenerator;
    }

    public async virtual Task<string> GenerateOAuth2AuthorizeAsync(
        string urlName,
        string responseType = "code", 
        string scope = "snsapi_base")
    {
        var userId = CurrentUser.GetId().ToString("D");
        var state = _encryptionService.Encrypt(userId);
        var redirectUri = await _appUrlProvider.GetUrlAsync(AbpWeChatWorkGlobalConsts.ProviderName, urlName);

        return await _authorizeGenerator.GenerateOAuth2AuthorizeAsync(redirectUri, state, responseType, scope);
    }

    public async virtual Task<string> GenerateOAuth2LoginAsync(
        string urlName,
        string loginType = "CorpApp")
    {
        var userId = CurrentUser.GetId().ToString("D");
        var state = _encryptionService.Encrypt(userId);
        var redirectUri = await _appUrlProvider.GetUrlAsync(AbpWeChatWorkGlobalConsts.ProviderName, urlName);

        return await _authorizeGenerator.GenerateOAuth2LoginAsync(redirectUri, state, loginType);
    }
}
