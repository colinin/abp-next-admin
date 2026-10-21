using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp.Account.Web.Pages.Account;

namespace LINGYUN.Abp.Account.Web.Pages.Account
{
    public class TwoFactorRecoveryCodesModel : AccountPageModel
    {
        [Required]
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string RecoveryCodes { get; set; } = default!;

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string? ReturnUrl { get; set; }

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string? ReturnUrlHash { get; set; }

        public virtual Task<IActionResult> OnGetAsync()
        {
            return Task.FromResult<IActionResult>(Page());
        }

        public async virtual Task<IActionResult> OnPostAsync()
        {
            return await RedirectSafelyAsync(ReturnUrl!, ReturnUrlHash);
        }
    }
}
