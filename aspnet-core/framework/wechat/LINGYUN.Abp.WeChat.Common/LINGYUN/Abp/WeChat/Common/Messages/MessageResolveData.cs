namespace LINGYUN.Abp.WeChat.Common.Messages;
public class MessageResolveData
{
    public string AppId { get; set; }

    public string Token { get; set; }

    public string EncodingAESKey { get; set; }

    public string Signature { get; set; }

    public int TimeStamp { get; set; }

    public string Nonce { get; set; }

    public string Data { get; set; }
    public MessageResolveData(
        string appId,
        string token,
        string encodingAESKey,
        string signature,
        int timeStamp,
        string nonce,
        string data)
    {
        AppId = appId;
        Token = token;
        EncodingAESKey = encodingAESKey;
        Signature = signature;
        TimeStamp = timeStamp;
        Nonce = nonce;
        Data = data;
    }
}
