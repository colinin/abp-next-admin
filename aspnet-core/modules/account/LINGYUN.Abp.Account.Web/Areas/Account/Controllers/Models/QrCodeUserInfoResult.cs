namespace LINGYUN.Abp.Account.Web.Areas.Account.Controllers.Models;

public class QrCodeUserInfoResult : QrCodeInfoResult
{
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string Picture { get; set; }
}
