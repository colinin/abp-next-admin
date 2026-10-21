namespace LINGYUN.Abp.Account.Web.AliyunCaptcha.Models;

public class CaptchaConfigModel
{
    public string Region { get; set; } = default!;
    public string Prefix { get; set; } = default!;
    public string SceneId { get; set; } = default!;
    public string? EncryptedSceneId { get; set; }
}
