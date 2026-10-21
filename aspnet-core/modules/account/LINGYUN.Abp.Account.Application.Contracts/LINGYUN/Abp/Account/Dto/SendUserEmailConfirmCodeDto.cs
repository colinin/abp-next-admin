using System;
using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.Account;

public class SendUserEmailConfirmCodeDto
{
    [Required]
    public Guid UserId { get; set; }

    [Required]
    public string AppName { get; set; } = default!;

    public string? ReturnUrl { get; set; }

    public string? ReturnUrlHash { get; set; }
}
