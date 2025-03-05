using System;

namespace LINGYUN.Abp.Identity.QrCode;

public class QrCodeScanParams
{
    public string UserId { get; }
    public string UserName { get; }
    public string Picture { get; }
    public Guid? TenantId { get; }
    public QrCodeScanParams(
        string userId, 
        string userName, 
        string picture = null, 
        Guid? tenantId = null)
    {
        UserId = userId;
        UserName = userName;
        Picture = picture;
        TenantId = tenantId;
    }
}
