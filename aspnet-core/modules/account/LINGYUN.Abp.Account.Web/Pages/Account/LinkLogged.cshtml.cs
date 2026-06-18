using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp.Account.Web.Pages.Account;
using Volo.Abp.Identity;
using Volo.Abp.Security.Claims;

namespace LINGYUN.Abp.Account.Web.Pages.Account;

[Authorize]
public class LinkLoggedModel : AccountPageModel
{
    [BindProperty(SupportsGet = true)]
    public string ReturnUrl { get; set; }

    [BindProperty(SupportsGet = true)]
    public string ReturnUrlHash { get; set; }

    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid LinkUserId { get; set; }

    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid? LinkTenantId { get; set; }

    public string LinkTenantAndUserName { get; set; }

    protected ICurrentPrincipalAccessor CurrentPrincipalAccessor => LazyServiceProvider.LazyGetRequiredService<ICurrentPrincipalAccessor>();
    public IIdentityLinkUserAppService IdentityLinkUserAppService => LazyServiceProvider.LazyGetRequiredService<IIdentityLinkUserAppService>();
    public IdentityDynamicClaimsPrincipalContributorCache IdentityDynamicClaimsPrincipalContributorCache => LazyServiceProvider.LazyGetRequiredService<IdentityDynamicClaimsPrincipalContributorCache>();

    public virtual async Task<IActionResult> OnGetAsync()
    {
        var validLinkUser = await IdentityLinkUserAppService.VerifyLinkUserAsync(
            new Dto.VerifyLinkUserInput
            {
                UserId = LinkUserId,
                TenantId = LinkTenantId,
            });
        if (!validLinkUser.IsLinked)
        {
            return await RedirectToLoginPageAsync();
        }
        LinkTenantAndUserName = !validLinkUser.LinkTenantName.IsNullOrWhiteSpace() 
            ? $"{validLinkUser.LinkTenantName}\\{validLinkUser.LinkUserName}" 
            : validLinkUser.LinkUserName;

        return Page();
    }

    public async virtual Task<IActionResult> OnPostAsync()
    {
        if (LinkUserId == CurrentUser.Id && LinkTenantId == CurrentTenant.Id)
        {
            return await RedirectSafelyAsync(ReturnUrl, ReturnUrlHash);
        }

        using (CurrentTenant.Change(LinkTenantId))
        {
            var sourceUser = await UserManager.GetByIdAsync(LinkUserId);
            using (CurrentPrincipalAccessor.Change(await SignInManager.CreateUserPrincipalAsync(sourceUser)))
            {
                var validLinkUser = await IdentityLinkUserAppService.VerifyLinkUserAsync(
                    new Dto.VerifyLinkUserInput
                    {
                        UserId = LinkUserId,
                        TenantId = LinkTenantId,
                    });
                if (validLinkUser.IsLinked)
                {
                    var isPersistent = (await HttpContext.AuthenticateAsync(IdentityConstants.ApplicationScheme))?.Properties?.IsPersistent ?? false;
                    await SignInManager.SignOutAsync();
                    using (CurrentTenant.Change(LinkTenantId))
                    {
                        var targetUser = await UserManager.GetByIdAsync(LinkUserId);
                        await SignInManager.SignInAsync(targetUser, isPersistent);
                        await IdentityDynamicClaimsPrincipalContributorCache.ClearAsync(targetUser.Id, targetUser.TenantId);
                    }

                    return await RedirectSafelyAsync(ReturnUrl, ReturnUrlHash);
                }
            }
        }

        Alerts.Warning(L["Volo.Abp.Identity:InvalidToken"]);
        return Page();
    }

    public virtual Task<string> GetReturnUrlAsync(string returnUrl, string returnUrlHash)
    {
        return base.GetRedirectUrlAsync(returnUrl, returnUrlHash);
    }

    protected virtual Task<IActionResult> RedirectToLoginPageAsync()
    {
        return Task.FromResult<IActionResult>(RedirectToPage("./Login", new
        {
            ReturnUrl,
            ReturnUrlHash
        }));
    }
}
