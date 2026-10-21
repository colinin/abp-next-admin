using System.ComponentModel.DataAnnotations;
using Volo.Abp.Identity;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.Account;

public class VerifyEmailRegisterCodeInput
{
    [Required]
    [EmailAddress]
    [Display(Name = "EmailAddress")]
    [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxEmailLength))]
    public string EmailAddress { get; set; } = default!;

    [Required]
    [StringLength(10)]
    public string VerifyCode { get; set; } = default!;
}
