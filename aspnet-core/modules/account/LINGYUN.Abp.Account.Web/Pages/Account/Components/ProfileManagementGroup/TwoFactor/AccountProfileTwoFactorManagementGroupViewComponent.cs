using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.Account.Web.Pages.Account.Components.ProfileManagementGroup.TwoFactor;

public class AccountProfileTwoFactorManagementGroupViewComponent : AbpViewComponent
{
    protected IMyProfileAppService ProfileAppService { get; }

    public AccountProfileTwoFactorManagementGroupViewComponent(IMyProfileAppService profileAppService)
    {
        ProfileAppService = profileAppService;
    }

    public async virtual Task<IViewComponentResult> InvokeAsync()
    {
        var dto = await ProfileAppService.GetTwoFactorEnabledAsync();
        var model = new ChangeTwoFactorModel
        {
            TwoFactorEnabled = dto.Enabled
        };

        return View("~/Pages/Account/Components/ProfileManagementGroup/TwoFactor/Default.cshtml", model);
    }

    public class ChangeTwoFactorModel
    {
        [DisplayName("TwoFactor:Enabled")]
        public bool TwoFactorEnabled { get; set; }
    }
}
