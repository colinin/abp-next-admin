namespace LINGYUN.Abp.WeChat.Common.Crypto.Models;
public class WeChatCryptoEchoData
{
    public string EchoStr { get; }
    public string MsgSignature { get; }
    public string ReceiveId { get; }
    public string Token { get; }
    public string EncodingAESKey { get; }
    public string TimeStamp { get; }
    public string Nonce { get; }
    public WeChatCryptoEchoData(
        string echoStr,
        string receiveId,
        string token,
        string encodingAESKey,
        string msgSignature,
        string timeStamp,
        string nonce)
    {
        EchoStr = echoStr;
        ReceiveId = receiveId;
        Token = token;
        EncodingAESKey = encodingAESKey;
        MsgSignature = msgSignature;
        TimeStamp = timeStamp;
        Nonce = nonce;
    }
}
