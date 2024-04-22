using LINGYUN.Abp.WeChat.Common.Crypto;
using LINGYUN.Abp.WeChat.Common.Crypto.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.WeChat.Common.Messages;
public class MessageResolver : IMessageResolver, ITransientDependency
{
    private readonly IWeChatCryptoService _cryptoService;
    private readonly IServiceProvider _serviceProvider;
    private readonly AbpWeChatMessageResolveOptions _options;

    public MessageResolver(
        IOptions<AbpWeChatMessageResolveOptions> options,
        IWeChatCryptoService cryptoService,
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _cryptoService = cryptoService;
        _options = options.Value;
    }
    /// <summary>
    /// 解析微信服务器推送消息/事件
    /// </summary>
    /// <param name="messageData"></param>
    /// <returns></returns>
    public async virtual Task<MessageResolveResult> ResolveMessageAsync(MessageResolveData messageData)
    {
        var result = new MessageResolveResult(messageData.Data);
        using (var serviceScope = _serviceProvider.CreateScope())
        {
            /* 明文数据格式
             * <xml>
             *  <ToUserName><![CDATA[toUser]]></ToUserName>
                <FromUserName><![CDATA[fromUserName]]></FromUserName>
                <CreateTime>1699433172</CreateTime>
                <MsgType><![CDATA[event]]></MsgType>
                <Event><![CDATA[event]]></Event>
                <EventKey><![CDATA[eventKey]]></EventKey>
                <Ticket><![CDATA[gQH97zwAAAAAAAAAAS5odHRwOi8vd2VpeGluLnFxLmNvbS9xLzAyTVoyOTBoLVJka0YxazFiYnhCMXcAAgTFSktlAwQ8AAAA]]></Ticket>
               </xml>
             */
            var xmlDocument = XDocument.Parse(messageData.Data);
            if (!xmlDocument.Elements("Encrypt").Any())
            {
                /* 加密数据格式
                 * <xml>
                 *  <ToUserName><![CDATA[toUser]]></ToUserName>
                    <Encrypt><![CDATA[msg_encrypt]]></Encrypt>
                   </xml>
                 */
                var cryptoDecryptData = new WeChatCryptoDecryptData(
                    messageData.Data,
                    messageData.AppId,
                    messageData.Token,
                    messageData.EncodingAESKey,
                    messageData.Signature,
                    messageData.TimeStamp.ToString(),
                    messageData.Nonce);
                // 经过解密函数得到如上真实数据
                var decryptMessage = _cryptoService.Decrypt(cryptoDecryptData);
                xmlDocument = XDocument.Parse(decryptMessage);
                result.Input = decryptMessage;
            }

            var context = new MessageResolveContext(result.Input, xmlDocument, serviceScope.ServiceProvider);

            foreach (var messageResolver in _options.MessageResolvers)
            {
                await messageResolver.ResolveAsync(context);

                result.AppliedResolvers.Add(messageResolver.Name);

                if (context.HasResolvedMessage())
                {
                    result.Message = context.Message;
                    break;
                }
            }
        }
        return result;
    }
}
