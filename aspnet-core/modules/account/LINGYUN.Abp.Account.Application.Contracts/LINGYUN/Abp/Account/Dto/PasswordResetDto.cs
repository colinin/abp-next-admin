using System.ComponentModel.DataAnnotations;
using Volo.Abp.Auditing;
using Volo.Abp.Identity;

namespace LINGYUN.Abp.Account
{
    public class PasswordResetDto
    {
        [Required]
        [Phone]
        [StringLength(IdentityUserConsts.MaxPhoneNumberLength)]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(IdentityUserConsts.MaxPasswordLength)]
        [DataType(DataType.Password)]
        [DisableAuditing]
        public string NewPassword { get; set; }

        [Required]
        [StringLength(6)]
        public string VerifyCode { get; set; }
    }
}
