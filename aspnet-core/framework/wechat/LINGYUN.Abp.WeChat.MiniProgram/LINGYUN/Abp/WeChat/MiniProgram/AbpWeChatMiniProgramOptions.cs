namespace LINGYUN.Abp.WeChat.MiniProgram;

public class AbpWeChatMiniProgramOptions
{
    /// <summary>
    /// 小程序AppId
    /// </summary>
    public string AppId { get; set; } = default!;
    /// <summary>
    /// 小程序AppSecret
    /// </summary>
    public string AppSecret { get; set; } = default!;
    /// <summary>
    /// 小程序消息解密Token
    /// </summary>
    public string Token { get; set; } = default!;
    /// <summary>
    /// 小程序消息解密AESKey
    /// </summary>
    public string EncodingAESKey { get; set; } = default!;
}
