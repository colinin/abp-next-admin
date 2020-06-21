using System.ComponentModel.DataAnnotations;
using Volo.Abp.Auditing;
using Volo.Abp.Identity;

namespace LINGYUN.Abp.Account
{
    public class WeChatRegisterDto
    {
        [Required]
        public string Code { get; set; }

        [DisableAuditing]
        [DataType(DataType.Password)]
        [Required]
        [StringLength(IdentityUserConsts.MaxPasswordLength)]
        public string Password { get; set; }

        [StringLength(IdentityUserConsts.MaxNameLength)]
        public string Name { get; set; }

        [StringLength(IdentityUserConsts.MaxUserNameLength)]
        public string UserName { get; set; }

        [EmailAddress]
        [StringLength(IdentityUserConsts.MaxEmailLength)]
        public string EmailAddress { get; set; }
    }
}
