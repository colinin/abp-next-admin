using System.ComponentModel.DataAnnotations;
using Volo.Abp.Auditing;
using Volo.Abp.Identity;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.Account
{
    public class WeChatRegisterDto
    {
        [Required]
        [DisableAuditing]
        [Display(Name = "DisplayName:WeChatCode")]
        public string Code { get; set; }

        [DataType(DataType.Password)]
        [Required]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
        [DisableAuditing]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxUserNameLength))]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxEmailLength))]
        [Display(Name = "EmailAddress")]
        public string EmailAddress { get; set; }
    }
}
