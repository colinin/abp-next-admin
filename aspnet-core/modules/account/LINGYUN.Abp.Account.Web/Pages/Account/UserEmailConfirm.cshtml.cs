using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;
using System.Threading.Tasks;
using Volo.Abp.Account.Web.Pages.Account;
using Volo.Abp.Identity;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.Account.Web.Pages.Account
{
    public class UserEmailConfirmModel : AccountPageModel
    {
        [BindProperty]
        public UserEmailConfirmInputModel Input { get; set; } = default!;

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public bool AlreadySend { get; set; }

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string? ReturnUrl { get; set; }

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string? ReturnUrlHash { get; set; }

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid? LinkUserId { get; set; }

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid? LinkTenantId { get; set; }

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string? LinkToken { get; set; }

        public IMyProfileAppService MyProfileAppService => LazyServiceProvider.LazyGetRequiredService<IMyProfileAppService>();

        public async virtual Task<IActionResult> OnGetAsync()
        {
            AlreadySend = false;
            Input = new UserEmailConfirmInputModel();

            var user = await GetCurrentUser();
            if (user == null || user.TenantId != CurrentTenant.Id)
            {
                await HttpContext.SignOutAsync(AbpAccountAuthenticationTypes.ConfirmUserScheme);
                return RedirectToPage("/Login", new { 
                    ReturnUrl, 
                    ReturnUrlHash,
                    LinkUserId,
                    LinkTenantId,
                    LinkToken,
                });
            }

            Input.EmailAddress = user.Email;

            return Page();
        }

        public async virtual Task<IActionResult> OnPostAsync()
        {
            await MyProfileAppService.SendEmailConfirmLinkAsync(
                new SendEmailConfirmCodeDto
                {
                    AppName = "MVC",
                    Email = Input.EmailAddress,
                    ReturnUrl = ReturnUrl,
                    ReturnUrlHash = ReturnUrlHash,
                });
            AlreadySend = true;

            Alerts.Success(L["EmailConfirmationSentMessage"]);

            return Page();
        }

        protected async virtual Task<IdentityUser?> GetCurrentUser()
        {
            var result = await HttpContext.AuthenticateAsync(AbpAccountAuthenticationTypes.ConfirmUserScheme);

            var userId = result?.Principal?.FindUserId();
            if (!userId.HasValue)
            {
                return null;
            }

            var tenantId = result?.Principal?.FindTenantId();
            using (CurrentTenant.Change(tenantId, null))
            {
                return await UserManager.FindByIdAsync(userId.Value.ToString());
            }
        }
    }

    public class UserEmailConfirmInputModel
    {
        [Required]
        [EmailAddress]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxEmailLength))]
        public string EmailAddress { get; set; } = default!;
    }
}
