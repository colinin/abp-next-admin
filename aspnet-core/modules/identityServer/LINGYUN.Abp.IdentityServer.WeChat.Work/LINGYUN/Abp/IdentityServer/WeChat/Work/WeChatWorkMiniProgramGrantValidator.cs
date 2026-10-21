using IdentityServer4.Services;
using IdentityServer4.Validation;
using LINGYUN.Abp.WeChat.Work;
using LINGYUN.Abp.WeChat.Work.Authorize;
using LINGYUN.Abp.WeChat.Work.Authorize.Models;
using LINGYUN.Abp.WeChat.Work.Localization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.IdentityServer.WeChat.Work;

public class WeChatWorkMiniProgramGrantValidator : WeChatWorkGrantValidator
{
    public WeChatWorkMiniProgramGrantValidator(
        IEventService eventService, 
        ICurrentTenant currentTenant, 
        IGuidGenerator guidGenerator,
        ISettingProvider settingProvider, 
        UserManager<Volo.Abp.Identity.IdentityUser> userManager, 
        IdentitySecurityLogManager identitySecurityLogManager, 
        IStringLocalizer<WeChatWorkResource> weChatWorkLocalizer,
        IWeChatWorkUserFinder weChatWorkUserFinder, 
        ILogger<WeChatWorkGrantValidator> logger) : 
        base(
            eventService,
            currentTenant,
            guidGenerator,
            settingProvider,
            userManager,
            identitySecurityLogManager, 
            weChatWorkLocalizer, 
            weChatWorkUserFinder,
            logger)
    {
    }

    public override string GrantType => AbpWeChatWorkGlobalConsts.MiniProgramGrantType;

    protected async override Task<WeChatWorkUserInfo> FindUserInfoAsync(ExtensionGrantValidationContext context, string code)
    {
        var code2Session = await WeChatWorkUserFinder.GetCode2SessionAsync(code);

        return new WeChatWorkUserInfo
        {
            UserId = code2Session.UserId,
            UserTicket = code2Session.SessionKey,
        };
    }
}
