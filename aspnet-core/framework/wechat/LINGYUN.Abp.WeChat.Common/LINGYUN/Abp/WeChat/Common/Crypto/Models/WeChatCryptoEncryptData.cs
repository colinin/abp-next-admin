namespace LINGYUN.Abp.WeChat.Common.Crypto.Models;
public class WeChatCryptoEncryptData
{
    public string Data { get; }
    public string ReceiveId { get; }
    public string Token { get; }
    public string EncodingAESKey { get; }
    public WeChatCryptoEncryptData(
        string data,
        string receiveId,
        string token,
        string encodingAESKey)
    {
        Data = data;
        ReceiveId = receiveId;
        Token = token;
        EncodingAESKey = encodingAESKey;
    }
}
