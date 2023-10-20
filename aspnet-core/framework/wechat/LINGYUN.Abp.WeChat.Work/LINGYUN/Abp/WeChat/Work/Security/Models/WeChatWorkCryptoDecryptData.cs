namespace LINGYUN.Abp.WeChat.Work.Security.Models;
public class WeChatWorkCryptoDecryptData : WeChatWorkCryptoData
{
    public string PostData { get; }
    public WeChatWorkCryptoDecryptData(
        string postData,
        string receiveId,
        string token,
        string encodingAESKey,
        string msgSignature,
        string timeStamp,
        string nonce) : base(receiveId, token, encodingAESKey, msgSignature, timeStamp, nonce)
    {
        PostData = postData;
    }
}
