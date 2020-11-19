using System.ComponentModel.DataAnnotations;
using Volo.Abp.Auditing;
using Volo.Abp.Identity;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.Account
{
    public class PhoneResetPasswordDto
    {
        [Required]
        [Phone]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPhoneNumberLength))]

        // 如果Dto属性和本地化内容不一致,需要指定本地化名称如下
        // [Display(Name = "DisplayName:RequiredPhoneNumber")] //json本地化文件中必须有相同的格式: DisplayName:RequiredPhoneNumber
        //[DisplayName("DisplayName:RequiredPhoneNumber")] //两种方法都可以

        // 如果Dto属性与本地化内容一致,不需要显示指定名称,但是本地化文件必须存在对应格式的文本: DisplayName:PhoneNumber
        public string PhoneNumber { get; set; }

        [Required]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
        [DataType(DataType.Password)]
        [DisableAuditing]
        public string NewPassword { get; set; }

        [Required]
        [StringLength(6)]
        [DisableAuditing]
        [Display(Name = "DisplayName:SmsVerifyCode")]
        public string Code { get; set; }
    }
}
