namespace LINGYUN.Abp.WeChat.Work.JsSdk.Dtos;
public class JsApiSignatureDto
{
    public string Nonce { get; set; }
    public string Timestamp { get; set; }
    public string Signature { get; set; }
    public JsApiSignatureDto()
    {

    }
    public JsApiSignatureDto(string nonce, string timestamp, string signature)
    {
        Nonce = nonce;
        Timestamp = timestamp;
        Signature = signature;
    }
}
