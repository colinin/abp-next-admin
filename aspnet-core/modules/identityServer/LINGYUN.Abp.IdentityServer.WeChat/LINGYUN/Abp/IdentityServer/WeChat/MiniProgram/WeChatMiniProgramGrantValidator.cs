using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using LINGYUN.Abp.WeChat.Localization;
using LINGYUN.Abp.WeChat.MiniProgram;
using LINGYUN.Abp.WeChat.MiniProgram.Features;
using LINGYUN.Abp.WeChat.OpenId;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;
using Volo.Abp.Features;
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

        public override string LoginProvider => AbpWeChatMiniProgramConsts.ProviderName;

        public override string AuthenticationMethod => AbpWeChatMiniProgramConsts.AuthenticationMethod;

        protected AbpWeChatMiniProgramOptionsFactory MiniProgramOptionsFactory { get; }

        protected IStringLocalizer<WeChatResource> WeChatLocalizer { get; }

        protected IFeatureChecker FeatureChecker => ServiceProvider.LazyGetRequiredService<IFeatureChecker>();

        public WeChatMiniProgramGrantValidator(
            IEventService eventService,
            IWeChatOpenIdFinder weChatOpenIdFinder,
            UserManager<IdentityUser> userManager,
            IIdentityUserRepository userRepository,
            IStringLocalizer<Volo.Abp.Identity.Localization.IdentityResource> identityLocalizer,
            IStringLocalizer<AbpIdentityServerResource> identityServerLocalizer,
            IStringLocalizer<WeChatResource> wechatLocalizer,
            AbpWeChatMiniProgramOptionsFactory miniProgramOptionsFactory)
            : base(eventService, weChatOpenIdFinder, userManager, userRepository, identityLocalizer, identityServerLocalizer)
        {
            WeChatLocalizer = wechatLocalizer;
            MiniProgramOptionsFactory = miniProgramOptionsFactory;
        }

        protected override async Task<bool> CheckFeatureAsync(ExtensionGrantValidationContext context)
        {
            if (!await FeatureChecker.IsEnabledAsync(WeChatMiniProgramFeatures.EnableAuthorization))
            {
                context.Result = new GrantValidationResult(
                    TokenRequestErrors.InvalidGrant,
                    IdentityServerLocalizer["AuthorizationDisabledMessage",
                    WeChatLocalizer["Features:WeChat.MiniProgram.EnableAuthorization"]]);
                return false;
            }
            return true;
        }

        protected override async Task<WeChatOpenId> FindOpenIdAsync(string code)
        {
            var options = await MiniProgramOptionsFactory.CreateAsync();

            return await WeChatOpenIdFinder.FindAsync(code, options.AppId, options.AppSecret);
        }
    }
}
