using LINGYUN.Abp.WeChat.Common.Utils;
using Shouldly;
using System.Xml;
using Xunit;

namespace LINGYUN.Abp.WeChat.Common.Crypto;

public class Crypto_Tests : AbpWeChatCommonTestBase
{
    private const string sToken = "QDG6eK";
    private const string sCorpID = "wx5823bf96d3bd56c7";
    private const string sEncodingAESKey = "jWmYm7qr5nMoAUwZRjGtBxmz3KA1tkAj3ykkR6q2B2C";

    [Fact]
    public void CryptTest()
    {
        var wxcpt = new WXBizMsgCrypt(sToken, sEncodingAESKey, sCorpID);
        // string sVerifyMsgSig = HttpUtils.ParseUrl("msg_signature");
        string sVerifyMsgSig = "5c45ff5e21c57e6ad56bac8758b79b1d9ac89fd3";
        // string sVerifyTimeStamp = HttpUtils.ParseUrl("timestamp");
        string sVerifyTimeStamp = "1409659589";
        // string sVerifyNonce = HttpUtils.ParseUrl("nonce");
        string sVerifyNonce = "263014780";
        // string sVerifyEchoStr = HttpUtils.ParseUrl("echostr");
        string sVerifyEchoStr = "P9nAzCzyDtyTWESHep1vC5X9xho/qYX3Zpb4yKa9SKld1DsH3Iyt3tP3zNdtp+4RPcs8TgAE7OaBO+FZXvnaqQ==";
        int ret = 0;
        string sEchoStr = "";
        ret = wxcpt.VerifyURL(sVerifyMsgSig, sVerifyTimeStamp, sVerifyNonce, sVerifyEchoStr, ref sEchoStr);

        ret.ShouldBe(0);

        // string sReqTimeStamp = HttpUtils.ParseUrl("timestamp");
        string sReqTimeStamp = "1409659813";
        // string sReqNonce = HttpUtils.ParseUrl("nonce");
        string sReqNonce = "1372623149";

        string sRespData = "<xml><ToUserName><![CDATA[mycreate]]></ToUserName><FromUserName><![CDATA[wx582396d3bd56c7]]></FromUserName><CreateTime>1348831860</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[this is a test]]></Content><MsgId>1234567890123456</MsgId><AgentID>128</AgentID></xml>";
        string sEncryptMsg = ""; //xml格式的密文
        ret = wxcpt.EncryptMsg(sRespData, sReqTimeStamp, sReqNonce, ref sEncryptMsg);

        ret.ShouldBe(0);
    }

    [Fact]
    public void DecryptTest()
    {
        var wxcpt = new WXBizMsgCrypt(sToken, sEncodingAESKey, sCorpID);

        string sReqMsgSig = "477715d11cdb4164915debcba66cb864d751f3e6";
        // string sReqTimeStamp = HttpUtils.ParseUrl("timestamp");
        string sReqTimeStamp = "1409659813";
        // string sReqNonce = HttpUtils.ParseUrl("nonce");
        string sReqNonce = "1372623149";
        // Post请求的密文数据
        // string sReqData = HttpUtils.PostData();
        string sReqData = "<xml><ToUserName><![CDATA[wx5823bf96d3bd56c7]]></ToUserName><Encrypt><![CDATA[RypEvHKD8QQKFhvQ6QleEB4J58tiPdvo+rtK1I9qca6aM/wvqnLSV5zEPeusUiX5L5X/0lWfrf0QADHHhGd3QczcdCUpj911L3vg3W/sYYvuJTs3TUUkSUXxaccAS0qhxchrRYt66wiSpGLYL42aM6A8dTT+6k4aSknmPj48kzJs8qLjvd4Xgpue06DOdnLxAUHzM6+kDZ+HMZfJYuR+LtwGc2hgf5gsijff0ekUNXZiqATP7PF5mZxZ3Izoun1s4zG4LUMnvw2r+KqCKIw+3IQH03v+BCA9nMELNqbSf6tiWSrXJB3LAVGUcallcrw8V2t9EL4EhzJWrQUax5wLVMNS0+rUPA3k22Ncx4XXZS9o0MBH27Bo6BpNelZpS+/uh9KsNlY6bHCmJU9p8g7m3fVKn28H3KDYA5Pl/T8Z1ptDAVe0lXdQ2YoyyH2uyPIGHBZZIs2pDBS8R07+qN+E7Q==]]></Encrypt><AgentID><![CDATA[218]]></AgentID></xml>";
        string sMsg = "";  // 解析之后的明文
        var ret = wxcpt.DecryptMsg(sReqMsgSig, sReqTimeStamp, sReqNonce, sReqData, ref sMsg);

        ret.ShouldBe(0);

        XmlDocument doc = new XmlDocument();
        doc.LoadXml(sMsg);
        XmlNode root = doc.FirstChild;
        string content = root["Content"].InnerText;

        content.ShouldNotBeNullOrWhiteSpace();
    }
}
