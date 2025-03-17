using System;

namespace LINGYUN.Abp.Identity.QrCode;

public class QrCodeInfo
{
    public string Key { get; }
    public string Token { get; private set; }
    public QrCodeStatus Status { get; private set; }
    public string UserId { get; set; }
    public Guid? TenantId { get; set; }
    public string UserName { get; set; }
    public string Picture { get; set; }

    public QrCodeInfo(string key)
    {
        Key = key;
        Status = QrCodeStatus.Created;
    }

    public void SetToken(string token)
    {
        Token = token;
    }

    public void SetStatus(QrCodeStatus status)
    {
        Status = status;
    }
}
