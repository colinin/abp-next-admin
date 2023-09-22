using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp.Account.Web.Pages.Account;

namespace LY.MicroService.IdentityServer.Pages.Account;

[AllowAnonymous]
public class EmailConfirmConfirmationModel : AccountPageModel
{
    [BindProperty(SupportsGet = true)]
    public string ReturnUrl { get; set; }

    [BindProperty(SupportsGet = true)]
    public string ReturnUrlHash { get; set; }
}
