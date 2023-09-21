namespace LINGYUN.Abp.WeChat.Work.Security;
public class WeChatWorkCryptoData
{
    public string ReceiveId { get; }
    public string Token { get; }
    public string EncodingAESKey { get; }
    public string MsgSignature { get; }
    public string TimeStamp { get; }
    public string Nonce { get; }
    public WeChatWorkCryptoData(
        string receiveId, 
        string token, 
        string encodingAESKey, 
        string msgSignature, 
        string timeStamp, 
        string nonce)
    {
        ReceiveId = receiveId;
        Token = token;
        EncodingAESKey = encodingAESKey;
        MsgSignature = msgSignature;
        TimeStamp = timeStamp;
        Nonce = nonce;
    }
}
