namespace LINGYUN.Abp.WeChat.Work.JsSdk.Models;
public class JsApiSignatureData
{
    public string Nonce { get; }
    public string Timestamp { get; }
    public string Signature { get; }
    public JsApiSignatureData(string nonce, string timestamp, string signature)
    {
        Nonce = nonce;
        Timestamp = timestamp;
        Signature = signature;
    }
}
