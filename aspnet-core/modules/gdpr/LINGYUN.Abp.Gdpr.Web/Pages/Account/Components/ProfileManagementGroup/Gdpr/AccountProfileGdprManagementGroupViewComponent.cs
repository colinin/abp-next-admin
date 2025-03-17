using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.Gdpr.Web.Pages.Account.Components.ProfileManagementGroup.Gdpr;

public class AccountProfileGdprManagementGroupViewComponent : AbpViewComponent
{
    public AccountProfileGdprManagementGroupViewComponent()
    {
    }

    public async virtual Task<IViewComponentResult> InvokeAsync()
    {
        await Task.CompletedTask;

        return View("~/Pages/Account/Components/ProfileManagementGroup/Gdpr/Index.cshtml");
    }
}
