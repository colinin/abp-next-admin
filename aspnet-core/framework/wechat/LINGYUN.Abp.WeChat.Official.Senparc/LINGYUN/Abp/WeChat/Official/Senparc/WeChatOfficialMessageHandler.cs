using Senparc.NeuChar.App.AppStore;
using Senparc.NeuChar.Entities;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.MessageHandlers;
using System;
using System.IO;

namespace LINGYUN.Abp.WeChat.Official.Senparc;

public class WeChatOfficialMessageHandler : MessageHandler<WeChatOfficialMessageContext>
{
    public WeChatOfficialMessageHandler(
        Stream inputStream, 
        PostModel postModel, 
        int maxRecordCount = 0, 
        bool onlyAllowEncryptMessage = false, 
        DeveloperInfo developerInfo = null,
        IServiceProvider serviceProvider = null) 
        : base(inputStream, postModel, maxRecordCount, onlyAllowEncryptMessage, developerInfo, serviceProvider)
    {
    }

    public override IResponseMessageBase DefaultResponseMessage(IRequestMessageBase requestMessage)
    {
        var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
        responseMessage.Content = $"这条消息来自DefaultResponseMessage。\r\n您收到这条消息，表明该公众号没有对【{requestMessage.MsgType}】类型做处理。";
        return responseMessage;
    }
}
