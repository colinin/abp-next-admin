using System;
using Volo.Abp.MultiTenancy;
namespace LINGYUN.Abp.Identity.QrCode;

[Serializable]
[IgnoreMultiTenancy]
public class QrCodeCacheItem
{
    public string Key { get; set; }
    public string Token { get; set; }
    public QrCodeStatus Status { get; set; }
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string Picture { get; set; }
    public Guid? TenantId { get; set; }
    public QrCodeCacheItem()
    {

    }
    public QrCodeCacheItem(string key)
    {
        Key = key;
        Status = QrCodeStatus.Created;
    }

    public QrCodeInfo GetQrCodeInfo()
    {
        var qrCodeInfo = new QrCodeInfo(Key)
        {
            UserId = UserId,
            UserName = UserName,
            Picture = Picture,
        };
        qrCodeInfo.SetToken(Token);
        qrCodeInfo.SetStatus(Status);

        return qrCodeInfo;
    }
}
