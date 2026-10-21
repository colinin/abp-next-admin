namespace LINGYUN.Abp.Account.Web.TencentCaptcha.Models;

public class CaptchaConfigModel
{
    public string CaptchaAppId { get; set; } = default!;
    public string? AidEncrypted { get; set; }
    public string? AidEncryptedType { get; set; }
    public string? AidEncryptedAad { get; set; }
}
