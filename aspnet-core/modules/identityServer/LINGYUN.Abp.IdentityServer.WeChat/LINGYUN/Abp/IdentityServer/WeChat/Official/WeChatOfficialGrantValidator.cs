using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using LINGYUN.Abp.WeChat.Localization;
using LINGYUN.Abp.WeChat.Official;
using LINGYUN.Abp.WeChat.Official.Features;
using LINGYUN.Abp.WeChat.OpenId;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;
using Volo.Abp.Features;
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

        public override string LoginProvider => AbpWeChatOfficialConsts.ProviderName;

        public override string AuthenticationMethod => AbpWeChatOfficialConsts.AuthenticationMethod;

        protected AbpWeChatOfficialOptionsFactory WeChatOfficialOptionsFactory { get; }

        protected IStringLocalizer<WeChatResource> WeChatLocalizer { get; }

        protected IFeatureChecker FeatureChecker => ServiceProvider.LazyGetRequiredService<IFeatureChecker>();

        public WeChatOfficialGrantValidator(
            IEventService eventService,
            IWeChatOpenIdFinder weChatOpenIdFinder,
            UserManager<IdentityUser> userManager,
            IIdentityUserRepository userRepository,
            IStringLocalizer<Volo.Abp.Identity.Localization.IdentityResource> identityLocalizer,
            IStringLocalizer<AbpIdentityServerResource> identityServerLocalizer,
            IStringLocalizer<WeChatResource> wechatLocalizer,
            AbpWeChatOfficialOptionsFactory weChatOfficialOptionsFactory)
            : base(eventService, weChatOpenIdFinder, userManager, userRepository, identityLocalizer, identityServerLocalizer)
        {
            WeChatLocalizer = wechatLocalizer;
            WeChatOfficialOptionsFactory = weChatOfficialOptionsFactory;
        }

        protected override async Task<bool> CheckFeatureAsync(ExtensionGrantValidationContext context)
        {
            if (!await FeatureChecker.IsEnabledAsync(WeChatOfficialFeatures.EnableAuthorization))
            {
                context.Result = new GrantValidationResult(
                    TokenRequestErrors.InvalidGrant,
                    IdentityServerLocalizer["AuthorizationDisabledMessage",
                    WeChatLocalizer["Features:WeChat.Official.EnableAuthorization"]]);
                return false;
            }
            return true;
        }

        protected override async Task<WeChatOpenId> FindOpenIdAsync(string code)
        {
            var options = await WeChatOfficialOptionsFactory.CreateAsync();

            return await WeChatOpenIdFinder.FindAsync(code, options.AppId, options.AppSecret);
        }
    }
}
