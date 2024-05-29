using System;
using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.Account;

public class GetTwoFactorProvidersInput
{
    [Required]
    public Guid UserId { get; set; }
}
