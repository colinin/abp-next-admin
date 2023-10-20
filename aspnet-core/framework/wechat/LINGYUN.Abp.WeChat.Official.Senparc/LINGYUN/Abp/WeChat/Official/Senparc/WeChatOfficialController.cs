using Microsoft.AspNetCore.Mvc;
using Senparc.NeuChar.MessageHandlers;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.Entities.Request;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.WeChat.Official.Senparc;

[Area("wechat-official")]
[Route("api/wechat/official")]
public class WeChatOfficialController : AbpControllerBase
{
    private readonly AbpWeChatOfficialOptionsFactory _optionsFactory;

    public WeChatOfficialController(AbpWeChatOfficialOptionsFactory optionsFactory)
    {
        _optionsFactory = optionsFactory;
    }



    /// <summary>
    /// 微信后台验证地址（使用Get），微信后台的“接口配置信息”的Url填写如：http://weixin.senparc.com/weixin
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Get(PostModel postModel, string echostr)
    {
        var options = await _optionsFactory.CreateAsync();
        if (CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, options.Token))
        {
            return Content(echostr); //返回随机字符串则表示验证通过
        }
        else
        {
            return Content(
                "failed:" + postModel.Signature + "," + 
                CheckSignature.GetSignature(postModel.Timestamp, postModel.Nonce, options.Token) + "。" +
                "如果你在浏览器中看到这句话，说明此地址可以被作为微信公众账号后台的Url，请注意保持Token一致。");
        }
    }

    /// <summary>
    /// 用户发送消息后，微信平台自动Post一个请求到这里，并等待响应XML。
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Post(PostModel postModel)
    {
        var options = await _optionsFactory.CreateAsync();
        if (!CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, options.Token))
        {
            return Content("参数错误！");
        }

        postModel.Token = options.Token;//根据自己后台的设置保持一致
        postModel.EncodingAESKey = options.EncodingAESKey;//根据自己后台的设置保持一致
        postModel.AppId = options.AppId;//根据自己后台的设置保持一致

        //自定义MessageHandler，对微信请求的详细判断操作都在这里面。
        var messageHandler = new WeChatOfficialMessageHandler(Request.Body, postModel);//接收消息

        #region 设置消息去重设置 + 优先调用同步、异步方法设置

        /* 如果需要添加消息去重功能，只需打开OmitRepeatedMessage功能，SDK会自动处理。
         * 收到重复消息通常是因为微信服务器没有及时收到响应，会持续发送2-5条不等的相同内容的 RequestMessage */
        messageHandler.OmitRepeatedMessage = true;//默认已经是开启状态，此处仅作为演示，也可以设置为 false 在本次请求中停用此功能

        //当同步方法被重写，且异步方法未被重写时，尝试调用同步方法
        messageHandler.DefaultMessageHandlerAsyncEvent = DefaultMessageHandlerAsyncEvent.SelfSynicMethod;

        #endregion

        messageHandler.SaveRequestMessageLog();//记录 Request 日志（可选）

        await messageHandler.ExecuteAsync(HttpContext.RequestAborted);//执行微信处理过程（关键，第二步）

        messageHandler.SaveResponseMessageLog();//记录 Response 日志（可选）

        return Ok();
    }
}
