using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.Account.Web.Pages.Account.Components.ProfileManagementGroup.Authenticator;

public class AccountProfileAuthenticatorManagementGroupViewComponent : AbpViewComponent
{
    protected IMyProfileAppService ProfileAppService { get; }

    public AccountProfileAuthenticatorManagementGroupViewComponent(
        IMyProfileAppService profileAppService)
    {
        ProfileAppService = profileAppService;
    }

    public async virtual Task<IViewComponentResult> InvokeAsync()
    {
        var authenticatorDto = await ProfileAppService.GetAuthenticatorAsync();

        var model = new AuthenticatorModel
        {
            AuthenticatorUri = authenticatorDto.AuthenticatorUri,
            IsAuthenticated = authenticatorDto.IsAuthenticated,
            SharedKey = authenticatorDto.SharedKey,
        };

        return View("~/Pages/Account/Components/ProfileManagementGroup/Authenticator/Index.cshtml", model);
    }

    public class AuthenticatorModel
    {
        public bool IsAuthenticated { get; set; }

        public string SharedKey { get; set; }

        public string AuthenticatorUri { get; set; }

        [Required]
        [StringLength(6)]
        public string AuthenticatorCode { get; set; }
    }
}
