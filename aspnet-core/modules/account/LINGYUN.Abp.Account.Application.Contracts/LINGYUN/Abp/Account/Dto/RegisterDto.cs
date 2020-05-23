using System.ComponentModel.DataAnnotations;
using Volo.Abp.Identity;

namespace LINGYUN.Abp.Account
{
    public class RegisterDto
    {
        [Required]
        [Phone]
        [StringLength(IdentityUserConsts.MaxPhoneNumberLength)]
        public string PhoneNumber { get; set; }
    }
}
