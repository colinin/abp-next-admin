using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.Account.Web.Pages.Account.Components.ProfileManagementGroup.SecurityLog;

public class AccountProfileSecurityLogManagementGroupViewComponent : AbpViewComponent
{
    public AccountProfileSecurityLogManagementGroupViewComponent()
    {
    }

    public async virtual Task<IViewComponentResult> InvokeAsync()
    {
        await Task.CompletedTask;

        return View("~/Pages/Account/Components/ProfileManagementGroup/SecurityLog/Index.cshtml");
    }
}
