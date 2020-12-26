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

        protected AbpWeChatMiniProgramOptionsFactory MiniProgramOptionsFactory { get; }

        public WeChatMiniProgramGrantValidator(
            IEventService eventService,
            IWeChatOpenIdFinder weChatOpenIdFinder,
            UserManager<IdentityUser> userManager,
            IIdentityUserRepository userRepository,
            IStringLocalizer<Volo.Abp.Identity.Localization.IdentityResource> identityLocalizer,
            IStringLocalizer<AbpIdentityServerResource> identityServerLocalizer,
            AbpWeChatMiniProgramOptionsFactory miniProgramOptionsFactory)
            : base(eventService, weChatOpenIdFinder, userManager, userRepository, identityLocalizer, identityServerLocalizer)
        {
            MiniProgramOptionsFactory = miniProgramOptionsFactory;
        }

        protected override async Task<WeChatOpenId> FindOpenIdAsync(string code)
        {
            var options = await MiniProgramOptionsFactory.CreateAsync();

            return await WeChatOpenIdFinder.FindAsync(code, options.AppId, options.AppSecret);
        }
    }
}
