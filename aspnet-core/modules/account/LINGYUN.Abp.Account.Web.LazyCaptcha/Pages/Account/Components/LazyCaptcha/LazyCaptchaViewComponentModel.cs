using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.Account.Web.LazyCaptcha.Pages.Account.Components.LazyCaptcha;

public class LazyCaptchaViewComponentModel
{
    public LazyCaptchaInputModel Input { get; set; } = default!;
}

public class LazyCaptchaInputModel
{
    [Required]
    [StringLength(10)]
    public string CaptchaCode { get; set; } = default!;
}