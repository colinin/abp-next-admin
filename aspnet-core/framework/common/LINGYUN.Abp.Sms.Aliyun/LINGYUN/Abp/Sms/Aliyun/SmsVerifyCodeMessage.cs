namespace LINGYUN.Abp.Sms.Aliyun;
public class SmsVerifyCodeMessage
{
    /// <summary>
    /// 方案名称，如果不填则为“默认方案”。最多不超过 20 个字符。
    /// </summary>
    public string SchemeName { get; set; }
    /// <summary>
    /// 号码国家编码。默认为 86，目前也仅支持中国国内号码发送。
    /// </summary>
    public string CountryCode { get; set; }
    /// <summary>
    /// 上行短信扩展码。上行短信指发送给通信服务提供商的短信，用于定制某种服务、完成查询，或是办理某种业务等，需要收费，按运营商普通短信资费进行扣费。
    /// </summary>
    public string SmsUpExtendCode { get; set; }
    /// <summary>
    /// 外部流水号。
    /// </summary>
    public string OutId { get; set; }
    /// <summary>
    /// 验证码长度支持 4～8 位长度，默认是 4 位。
    /// </summary>
    public long? CodeLength { get; set; }
    /// <summary>
    /// 验证码有效时长，单位秒，默认为 300 秒。
    /// </summary>
    public long? ValidTime { get; set; }
    /// <summary>
    /// 核验规则，当有效时间内对同场景内的同号码重复发送验证码时，旧验证码如何处理。
    /// </summary>
    /// <remarks>
    /// 1 - 覆盖处理（默认），即旧验证码会失效掉。<br />
    /// 2 - 保留，即多个验证码都是在有效期内都可以校验通过。
    /// </remarks>
    public long? DuplicatePolicy { get; set; }
    /// <summary>
    /// 时间间隔，单位：秒。即多久间隔可以发送一次验证码，用于频控，默认 60 秒。
    /// </summary>
    public long? Interval { get; set; }
    /// <summary>
    /// 生成的验证码类型。当参数 TemplateParam 传入占位符时，此参数必填，将由系统根据指定的规则生成验证码。
    /// </summary>
    /// <remarks>
    /// 1 - 纯数字（默认）。
    /// 2 - 纯大写字母。
    /// 3 - 纯小写字母。
    /// 4 - 大小字母混合。
    /// 5 - 数字+大写字母混合。
    /// 6 - 数字+小写字母混合。
    /// 7 - 数字+大小写字母混合。
    /// </remarks>
    public long? CodeType { get; set; }
    /// <summary>
    /// 是否返回验证码。
    /// </summary>
    public bool? ReturnVerifyCode { get; set; }
    /// <summary>
    /// 是否自动替换签名重试（默认开启。
    /// </summary>
    public bool? AutoRetry { get; set; }
    /// <summary>
    /// 短信接收方手机号。
    /// </summary>
    public string PhoneNumber { get; }
    /// <summary>
    /// 签名名称。暂不支持使用自定义签名，请使用系统赠送的签名。
    /// </summary>
    public string SignName { get; }
    /// <summary>
    /// 短信模板 CODE。参数SignName选择赠送签名时，必须搭配赠送模板下发短信。您可在赠送模板配置页面选择适用您业务场景的模板。
    /// </summary>
    public string TemplateCode { get; }
    /// <summary>
    /// 短信模板参数。
    /// </summary>
    public SmsVerifyCodeMessageParam TemplateParam { get; }
    public SmsVerifyCodeMessage(
        string phoneNumber, 
        SmsVerifyCodeMessageParam templateParam,
        string signName = null,
        string templateCode = null)
    {
        PhoneNumber = phoneNumber;
        TemplateParam = templateParam;
        SignName = signName;
        TemplateCode = templateCode;
    }
}
