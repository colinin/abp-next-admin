using System.ComponentModel.DataAnnotations;
using Volo.Abp.Identity;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.Identity
{
    public class SendChangeEmailAddressCodeDto
    {
        /// <summary>
        /// 新邮件地址
        /// </summary>
        [Required]
        [EmailAddress]
        [Display(Name = "EmailAddress")]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxEmailLength))]
        public string NewEmailAddress { get; set; }
    }
}
