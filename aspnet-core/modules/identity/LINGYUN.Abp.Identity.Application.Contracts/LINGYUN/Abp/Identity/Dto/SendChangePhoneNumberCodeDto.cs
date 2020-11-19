using System.ComponentModel.DataAnnotations;
using Volo.Abp.Identity;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.Identity
{
    public class SendChangePhoneNumberCodeDto
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
