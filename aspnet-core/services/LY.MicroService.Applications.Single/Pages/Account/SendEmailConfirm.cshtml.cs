using LINGYUN.Abp.Account;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp.Account.Localization;
using Volo.Abp.Account.Web.Pages.Account;
using Volo.Abp.Identity;
using Volo.Abp.Validation;

namespace LY.MicroService.Applications.Single.Pages.Account
{
    public class SendEmailConfirmModel : AccountPageModel
    {
        [BindProperty(SupportsGet = true)]
        public string Email { get; set; }

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string ReturnUrlHash { get; set; }

        public IMyProfileAppService MyProfileAppService { get; set; }

        public SendEmailConfirmModel()
        {
            LocalizationResourceType = typeof(AccountResource);
        }

        public virtual Task<IActionResult> OnGetAsync()
        {
            Email = CurrentUser.Email;

            return Task.FromResult<IActionResult>(Page());
        }

        public async virtual Task<IActionResult> OnPostAsync()
        {
            try
            {
                ValidateModel();

                await MyProfileAppService.SendEmailConfirmLinkAsync(
                    new SendEmailConfirmCodeDto
                    {
                        Email = Email,
                        AppName = "MVC",
                        ReturnUrl = ReturnUrl,
                        ReturnUrlHash = ReturnUrlHash
                    });
            }
            catch (AbpIdentityResultException e)
            {
                if (!string.IsNullOrWhiteSpace(e.Message))
                {
                    Alerts.Warning(GetLocalizeExceptionMessage(e));
                    return Page();
                }

                throw;
            }
            catch (AbpValidationException)
            {
                return Page();
            }

            return RedirectToPage("~/Account/Manage", new
            {
                returnUrl = ReturnUrl
            });
        }
    }
}
