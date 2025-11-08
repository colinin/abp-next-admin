namespace LINGYUN.Abp.Sms.Aliyun;
public class AliyunSmsVerifyCodeResponse
{
    /// <summary>
    /// 请求状态码, OK代表请求成功
    /// </summary>
    public string Code { get; set; }
    /// <summary>
    /// 状态码的描述
    /// </summary>
    public string Message { get; set; }
    /// <summary>
    /// 请求是否成功
    /// </summary>
    public bool Success { get; set; }
    /// <summary>
    /// 请求结果数据
    /// </summary>
    public AliyunSmsVerifyCodeModel Model { get; set; }
}

public class AliyunSmsVerifyCodeModel
{
    /// <summary>
    /// 请求Id
    /// </summary>
    public string RequestId { get; set; }
    /// <summary>
    /// 业务Id
    /// </summary>
    public string BizId { get; set; }
    /// <summary>
    /// 外部流水号
    /// </summary>
    public string OutId { get; set; }
    /// <summary>
    /// 验证码, 仅当使用阿里云短信验证服务生成验证码时携带
    /// </summary>
    public string VerifyCode { get; set; }
}
