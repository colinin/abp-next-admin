using System.ComponentModel.DataAnnotations;
using Volo.Abp.Auditing;
using Volo.Abp.Identity;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.Identity
{
    public class ChangeEmailAddressDto
    {
        /// <summary>
        /// 新邮件地址
        /// </summary>
        [Required]
        [EmailAddress]
        [Display(Name = "EmailAddress")]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxEmailLength))]
        public string NewEmailAddress { get; set; }
        /// <summary>
        /// 安全验证码
        /// </summary>
        [Required]
        [DisableAuditing]
        [StringLength(6, MinimumLength = 6)]
        [Display(Name = "EmailVerifyCode")]
        public string Code { get; set; }
    }
}
