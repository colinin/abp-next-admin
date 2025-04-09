using System;

namespace LINGYUN.Abp.Identity.QrCode;

public class QrCodeScanParams
{
    public string UserId { get; }
    public string UserName { get; }
    public Guid? TenantId { get; }
    public QrCodeScanParams(
        string userId, 
        string userName, 
        Guid? tenantId = null)
    {
        UserId = userId;
        UserName = userName;
        TenantId = tenantId;
    }
}
