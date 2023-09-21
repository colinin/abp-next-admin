namespace LINGYUN.Abp.WeChat.Work.Security;
/// <summary>
/// 企业微信加解密接口
/// </summary>
public interface IWeChatWorkCryptoService
{
    /// <summary>
    /// 校验
    /// </summary>
    /// <param name="data"></param>
    /// <param name="sReplyEchoStr"></param>
    /// <returns></returns>
    string Validation(WeChatWorkCryptoEchoData data);
    /// <summary>
    /// 解密
    /// </summary>
    /// <param name="data"></param>
    /// <param name="sMsg"></param>
    /// <returns></returns>
    string Decrypt(WeChatWorkCryptoDecryptData data);
    /// <summary>
    /// 加密
    /// </summary>
    /// <param name="data"></param>
    /// <param name="sEncryptMsg"></param>
    /// <returns></returns>
    string Encrypt(WeChatWorkCryptoData data);
}
