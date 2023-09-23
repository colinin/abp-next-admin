using LINGYUN.Abp.WeChat.Work.Security.Models;
using LINGYUN.Abp.WeChat.Work.Utils;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.WeChat.Work.Security;

public class WeChatWorkCryptoService : IWeChatWorkCryptoService, ISingletonDependency
{
    public string Decrypt(WeChatWorkCryptoDecryptData data)
    {
        var crypto = new WXBizMsgCrypt(
            data.Token,
            data.EncodingAESKey,
            data.ReceiveId);

        var retMsg = "";
        var ret = crypto.DecryptMsg(
            data.MsgSignature,
            data.TimeStamp,
            data.Nonce,
            data.PostData,
            ref retMsg);

        if (ret != 0)
        {
            throw new AbpWeChatWorkCryptoException(data.ReceiveId, code: $"WeChatWork:{ret}");
        }

        return retMsg;
    }

    public string Encrypt(WeChatWorkCryptoData data)
    {
        var crypto = new WXBizMsgCrypt(
            data.Token,
            data.EncodingAESKey,
            data.ReceiveId);

        var retMsg = "";

        var ret = crypto.EncryptMsg(
            data.MsgSignature,
            data.TimeStamp,
            data.Nonce,
            ref retMsg);

        if (ret != 0)
        {
            throw new AbpWeChatWorkCryptoException(data.ReceiveId, code: $"WeChatWork:{ret}");
        }

        return retMsg;
    }

    public string Validation(WeChatWorkCryptoEchoData data)
    {
        var crypto = new WXBizMsgCrypt(
            data.Token,
            data.EncodingAESKey,
            data.ReceiveId);

        var retMsg = "";

        var ret = crypto.VerifyURL(
            data.MsgSignature,
            data.TimeStamp,
            data.Nonce,
            data.EchoStr,
            ref retMsg);

        if (ret != 0)
        {
            throw new AbpWeChatWorkCryptoException(data.ReceiveId, code: $"WeChatWork:{ret}");
        }

        return retMsg;
    }
}
