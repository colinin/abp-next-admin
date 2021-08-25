namespace LINGYUN.Abp.WeChat.Crypto
{
    public interface IWeChatCryptoService
    {
        string Decrypt(string encryptedData, string iv, string sessionKey);
    }
}
