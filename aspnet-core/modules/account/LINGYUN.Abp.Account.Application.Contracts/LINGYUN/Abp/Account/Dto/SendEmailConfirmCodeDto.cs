using System.ComponentModel.DataAnnotations;
using Volo.Abp.Identity;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.Account;

public class SendEmailConfirmCodeDto
{
    [Required]
    [EmailAddress]
    [Display(Name = "EmailAddress")]
    [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxEmailLength))]
    public string Email { get; set; } = default!;

    [Required]
    public string AppName { get; set; } = default!;

    public string? ReturnUrl { get; set; }

    public string? ReturnUrlHash { get; set; }
}
