using IdentityServer4.Services;
using LINGYUN.Abp.WeChat.MiniProgram;
using LINGYUN.Abp.WeChat.OpenId;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Volo.Abp.Identity;
using Volo.Abp.IdentityServer.Localization;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace LINGYUN.Abp.IdentityServer.WeChat.MiniProgram
{
    /// <summary>
    /// 对于小程序绑定用户的扩展授权验证器
    /// </summary>
    public class WeChatMiniProgramGrantValidator : WeChatGrantValidator
    {
        public override string GrantType => AbpWeChatMiniProgramConsts.GrantType;

        public override string LoginProviderKey => AbpWeChatMiniProgramConsts.ProviderKey;

        public override string AuthenticationMethod => AbpWeChatMiniProgramConsts.AuthenticationMethod;

        protected AbpWeChatMiniProgramOptions Options { get; }

        public WeChatMiniProgramGrantValidator(
            IEventService eventService,
            IWeChatOpenIdFinder weChatOpenIdFinder,
            UserManager<IdentityUser> userManager,
            IIdentityUserRepository userRepository,
            IStringLocalizer<Volo.Abp.Identity.Localization.IdentityResource> identityLocalizer,
            IStringLocalizer<AbpIdentityServerResource> identityServerLocalizer,
            IOptions<AbpWeChatMiniProgramOptions> options)
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
