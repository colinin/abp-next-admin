namespace LINGYUN.Abp.WeChat.Common.Crypto.Models;
public class WeChatCryptoDecryptData
{
    public string MsgSignature { get; }
    public string ReceiveId { get; }
    public string Token { get; }
    public string EncodingAESKey { get; }
    public string TimeStamp { get; }
    public string Nonce { get; }
    public string PostData { get; }
    public WeChatCryptoDecryptData(
        string postData,
        string receiveId,
        string token,
        string encodingAESKey,
        string msgSignature,
        string timeStamp,
        string nonce)
    {
        PostData = postData;
        ReceiveId = receiveId;
        Token = token;
        EncodingAESKey = encodingAESKey;
        MsgSignature = msgSignature;
        TimeStamp = timeStamp;
        Nonce = nonce;
    }
}
