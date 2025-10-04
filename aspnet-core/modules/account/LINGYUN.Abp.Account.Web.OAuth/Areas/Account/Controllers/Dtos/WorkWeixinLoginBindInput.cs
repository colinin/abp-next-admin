using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.Account.Web.OAuth.Areas.Account.Controllers.Dtos;

public class WorkWeixinLoginBindInput
{
    [Required]
    [Display(Name = "WorkWeixin:Code")]
    public string Code { get; set; }
}
