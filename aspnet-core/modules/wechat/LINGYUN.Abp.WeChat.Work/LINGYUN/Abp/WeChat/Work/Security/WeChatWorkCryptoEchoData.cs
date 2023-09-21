namespace LINGYUN.Abp.WeChat.Work.Security;
public class WeChatWorkCryptoEchoData : WeChatWorkCryptoData
{
    public string EchoStr { get; }
    public WeChatWorkCryptoEchoData(
        string echoStr,
        string receiveId,
        string token,
        string encodingAESKey,
        string msgSignature,
        string timeStamp,
        string nonce) : base(receiveId, token, encodingAESKey, msgSignature, timeStamp, nonce)
    {
        EchoStr = echoStr;
    }
}
