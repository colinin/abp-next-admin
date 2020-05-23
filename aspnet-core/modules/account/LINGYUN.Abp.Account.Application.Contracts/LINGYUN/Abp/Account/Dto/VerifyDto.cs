using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.Account
{
    public class VerifyDto
    {
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public PhoneNumberVerifyType VerifyType { get; set; }
    }
}
