using LINGYUN.Abp.WeChat.Common.Crypto.Models;

namespace LINGYUN.Abp.WeChat.Common.Crypto
{
    public interface IWeChatCryptoService
    {
        /// <summary>
        /// 校验
        /// </summary>
        /// <param name="data"></param>
        /// <param name="sReplyEchoStr"></param>
        /// <returns></returns>
        string Validation(WeChatCryptoEchoData data);
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="data"></param>
        /// <param name="sMsg"></param>
        /// <returns></returns>
        string Decrypt(WeChatCryptoDecryptData data);
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="data"></param>
        /// <param name="sEncryptMsg"></param>
        /// <returns></returns>
        string Encrypt(WeChatCryptoEncryptData data);
    }
}
