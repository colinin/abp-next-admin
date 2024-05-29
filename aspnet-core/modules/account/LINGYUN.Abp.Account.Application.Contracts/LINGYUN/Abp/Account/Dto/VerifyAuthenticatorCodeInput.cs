using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.Account;
public class VerifyAuthenticatorCodeInput
{
    [Required]
    [StringLength(6)]
    public string AuthenticatorCode { get; set; }
}
