namespace LINGYUN.Abp.Sms.Aliyun;
public class SmsVerifyCodeMessageParam
{
    public string Code { get; }
    public string Min { get; }
    public SmsVerifyCodeMessageParam(string code, string min = "5")
    {
        Code = code;
        Min = min;
    }
}
