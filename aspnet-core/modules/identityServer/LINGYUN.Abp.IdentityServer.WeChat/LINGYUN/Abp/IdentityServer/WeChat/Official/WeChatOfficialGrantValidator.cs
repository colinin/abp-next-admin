using IdentityServer4.Services;
using LINGYUN.Abp.WeChat.Official;
using LINGYUN.Abp.WeChat.OpenId;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Volo.Abp.Identity;
using Volo.Abp.IdentityServer.Localization;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace LINGYUN.Abp.IdentityServer.WeChat.Official
{
    /// <summary>
    /// 对于公众平台绑定用户的扩展授权验证器
    /// </summary>
    public class WeChatOfficialGrantValidator : WeChatGrantValidator
    {
        public override string GrantType => AbpWeChatOfficialConsts.GrantType;

        public override string LoginProviderKey => AbpWeChatOfficialConsts.ProviderKey;

        public override string AuthenticationMethod => AbpWeChatOfficialConsts.AuthenticationMethod;

        protected AbpWeChatOfficialOptions Options { get; }

        public WeChatOfficialGrantValidator(
            IEventService eventService, 
            IWeChatOpenIdFinder weChatOpenIdFinder, 
            UserManager<IdentityUser> userManager, 
            IIdentityUserRepository userRepository,
            IStringLocalizer<Volo.Abp.Identity.Localization.IdentityResource> identityLocalizer, 
            IStringLocalizer<AbpIdentityServerResource> identityServerLocalizer,
            IOptions<AbpWeChatOfficialOptions> options) 
            : base(eventService, weChatOpenIdFinder, userManager, userRepository, identityLocalizer, identityServerLocalizer)
        {
            Options = options.Value;
        }

        protected override async Task<WeChatOpenId> FindOpenIdAsync(string code)
        {
            return await WeChatOpenIdFinder.FindAsync(code, Options.AppId, Options.AppSecret);
        }
    }
}
