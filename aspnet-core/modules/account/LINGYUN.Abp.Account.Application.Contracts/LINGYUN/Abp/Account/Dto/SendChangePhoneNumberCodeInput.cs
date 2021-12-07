using System.ComponentModel.DataAnnotations;
using Volo.Abp.Identity;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.Account
{
    public class SendChangePhoneNumberCodeInput
    {
        /// <summary>
        /// 新手机号
        /// </summary>
        [Required]
        [Phone]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPhoneNumberLength))]
        [Display(Name = "PhoneNumber")]
        public string NewPhoneNumber { get; set; }
    }
}
