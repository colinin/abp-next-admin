using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.Account;

public class ConfirmEmailInput
{
    [Required]
    public string ConfirmToken { get; set; }
}
