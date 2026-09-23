namespace LINGYUN.Abp.Aliyun.Captcha;

public interface ICaptchaRegionProvider
{
    string GetEndpoint();

    string GetRegion();
}
