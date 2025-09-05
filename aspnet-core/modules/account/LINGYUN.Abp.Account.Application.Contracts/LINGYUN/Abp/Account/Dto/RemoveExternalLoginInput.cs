using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.Account;
public class RemoveExternalLoginInput
{
    [Required]
    public string LoginProvider { get; set; }

    [Required]
    public string ProviderKey { get; set; }
}
