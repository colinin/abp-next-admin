using LINGYUN.Abp.WeChat.MiniProgram;
using LINGYUN.Abp.WeChat.OpenId;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using Volo.Abp.Json;

namespace LINGYUN.Abp.WeChat.Crypto
{
    [Authorize]
    public class CryptoAppService : WeChatApplicationServiceBase, ICryptoAppService
    {
        protected IJsonSerializer JsonSerializer { get; }
        protected IWeChatOpenIdFinder OpenIdFinder { get; }
        protected IWeChatCryptoService WeChatCryptoService { get; }
        protected AbpWeChatMiniProgramOptionsFactory MiniProgramOptionsFactory { get; }

        public CryptoAppService(
            IJsonSerializer jsonSerializer,
            IWeChatOpenIdFinder openIdFinder,
            IWeChatCryptoService weChatCryptoService,
            AbpWeChatMiniProgramOptionsFactory miniProgramOptionsFactory)
        {
            JsonSerializer = jsonSerializer;
            OpenIdFinder = openIdFinder;
            WeChatCryptoService = weChatCryptoService;
            MiniProgramOptionsFactory = miniProgramOptionsFactory;
        }

        public virtual async Task<UserInfoDto> GetUserInfoAsync(GetUserInfoInput input)
        {
            var options = await MiniProgramOptionsFactory.CreateAsync();
            WeChatOpenId weChatOpenId = input.Code.IsNullOrWhiteSpace()
                ? await OpenIdFinder.FindAsync(options.AppId)
                : await OpenIdFinder.FindAsync(input.Code, options.AppId, options.AppSecret);

            var decryptedData = WeChatCryptoService.Decrypt(input.EncryptedData, input.IV, weChatOpenId.SessionKey);

            return JsonSerializer.Deserialize<UserInfoDto>(decryptedData);
        }
    }
}
