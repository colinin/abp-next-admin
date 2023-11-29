using LINGYUN.Abp.WeChat.Common.Crypto.Models;
using LINGYUN.Abp.WeChat.Common.Utils;
using System;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.WeChat.Common.Crypto
{
    public class WeChatCryptoService : IWeChatCryptoService, ITransientDependency
    {
        public string Decrypt(WeChatCryptoDecryptData data)
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
                throw new AbpWeChatCryptoException(data.ReceiveId, code: $"WeChat:{ret}");
            }

            return retMsg;
        }

        public string Encrypt(WeChatCryptoEncryptData data)
        {
            var crypto = new WXBizMsgCrypt(
                data.Token,
                data.EncodingAESKey,
                data.ReceiveId);

            var sinature = "";
            var timestamp = DateTimeOffset.Now.ToLocalTime().ToUnixTimeSeconds().ToString();
            var nonce = DateTimeOffset.Now.Ticks.ToString("x");
            var sinatureRet = WXBizMsgCrypt.GenarateSinature(
                data.Token,
                timestamp,
                nonce,
                data.Data,
                ref sinature);
            if (sinatureRet != 0)
            {
                throw new AbpWeChatCryptoException(data.ReceiveId, code: $"WeChat:{sinatureRet}");
            }

            var retMsg = "";

            var ret = crypto.EncryptMsg(
                sinature,
                timestamp,
                nonce,
                ref retMsg);

            if (ret != 0)
            {
                throw new AbpWeChatCryptoException(data.ReceiveId, code: $"WeChat:{ret}");
            }

            return retMsg;
        }

        public string Validation(WeChatCryptoEchoData data)
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
                throw new AbpWeChatCryptoException(data.ReceiveId, code: $"WeChat:{ret}");
            }

            return retMsg;
        }
    }
}
