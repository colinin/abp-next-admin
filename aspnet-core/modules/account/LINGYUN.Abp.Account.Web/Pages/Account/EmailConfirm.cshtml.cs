using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp.Account.Web.Pages.Account;
using Volo.Abp.Identity;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.Account.Web.Pages.Account
{
    public class EmailConfirmModel : AccountPageModel
    {
        [Required]
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid UserId { get; set; }

        [Required]
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string ConfirmToken { get; set; } = default!;

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string? ReturnUrl { get; set; }

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string? ReturnUrlHash { get; set; }

        protected IAccountAppService LAbpAccountAppService { get; }

        public EmailConfirmModel(IAccountAppService accountAppService)
        {
            LAbpAccountAppService = accountAppService;
        }

        public async virtual Task<IActionResult> OnPostAsync()
        {
            try
            {
                ValidateModel();

                await LAbpAccountAppService.ConfirmEmailAsync(
                    new ConfirmUserEmailInput
                    {
                        UserId = UserId,
                        ConfirmToken = ConfirmToken,
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

            return RedirectToPage("./EmailConfirmConfirmation", new
            {
                returnUrl = ReturnUrl,
                returnUrlHash = ReturnUrlHash
            });
        }
    }
}
