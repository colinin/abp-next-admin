using System.ComponentModel.DataAnnotations;
using Volo.Abp.Auditing;
using Volo.Abp.Identity;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.Account
{
    public class ChangePhoneNumberInput
    {
        /// <summary>
        /// 新手机号
        /// </summary>
        [Required]
        [Phone]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPhoneNumberLength))]
        [Display(Name = "PhoneNumber")]
        public string NewPhoneNumber { get; set; }
        /// <summary>
        /// 安全验证码
        /// </summary>
        [Required]
        [DisableAuditing]
        [StringLength(6, MinimumLength = 6)]
        [Display(Name = "SmsVerifyCode")]
        public string Code { get; set; }
    }
}
