using LINGYUN.Abp.Identity.QrCode;

namespace LINGYUN.Abp.Account.Web.Areas.Account.Controllers.Models;

public class QrCodeInfoResult
{
    public string Key { get; set; }
    public QrCodeStatus Status { get; set; }
}
