using LINGYUN.Abp.WeChat.Work.Settings;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Security.Encryption;

namespace LINGYUN.Abp.WeChat.Work.Authorize;

[IntegrationService]
public class WeChatWorkAuthorizeAppService : ApplicationService, IWeChatWorkAuthorizeAppService
{
    private readonly IStringEncryptionService _encryptionService;
    private readonly IWeChatWorkAuthorizeGenerator _authorizeGenerator;

    public WeChatWorkAuthorizeAppService(
        IStringEncryptionService encryptionService,
        IWeChatWorkAuthorizeGenerator authorizeGenerator)
    {
        _encryptionService = encryptionService;
        _authorizeGenerator = authorizeGenerator;
    }

    public async virtual Task<string> GenerateOAuth2AuthorizeAsync(string agentid, string redirectUri, string responseType = "code", string scope = "snsapi_base")
    {
        var state = _encryptionService.Encrypt($"agentid={agentid}&redirectUri={redirectUri}&responseType={responseType}&scope={scope}&random={Guid.NewGuid():D}").ToMd5();

        return await _authorizeGenerator.GenerateOAuth2AuthorizeAsync(agentid, redirectUri, state, responseType, scope);
    }

    public async virtual Task<string> GenerateOAuth2LoginAsync(string redirectUri, string loginType = "ServiceApp", string agentid = "")
    {
        var state = _encryptionService.Encrypt($"agentid={agentid}&redirectUri={redirectUri}&loginType={loginType}&agentid={agentid}&random={Guid.NewGuid():D}").ToMd5();

        var corpId = await SettingProvider.GetOrNullAsync(WeChatWorkSettingNames.Connection.CorpId);

        Check.NotNullOrEmpty(corpId, nameof(corpId));

        return await _authorizeGenerator.GenerateOAuth2LoginAsync(corpId, redirectUri, state, loginType, agentid);
    }
}
