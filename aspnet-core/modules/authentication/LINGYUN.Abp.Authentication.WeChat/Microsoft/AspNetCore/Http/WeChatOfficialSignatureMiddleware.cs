using LINGYUN.Abp.WeChat.Official;
using System;
using System.Collections;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace Microsoft.AspNetCore.Http
{
    public class WeChatOfficialSignatureMiddleware : IMiddleware, ITransientDependency
    {
        protected AbpWeChatOfficialOptionsFactory WeChatOfficialOptionsFactory { get; }
        public WeChatOfficialSignatureMiddleware(
            AbpWeChatOfficialOptionsFactory weChatOfficialOptionsFactory)
        {
            WeChatOfficialOptionsFactory = weChatOfficialOptionsFactory;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (context.Request.Path.HasValue)
            {
                var options = await WeChatOfficialOptionsFactory.CreateAsync();

                var requestPath = context.Request.Path.Value;
                // 访问地址是否与定义的地址匹配
                if (requestPath.Equals(options.Url))
                {
                    var timestamp = context.Request.Query["timestamp"];
                    var nonce = context.Request.Query["nonce"];
                    var signature = context.Request.Query["signature"];
                    var echostr = context.Request.Query["echostr"];
                    // 验证消息合法性
                    var check = CheckWeChatSignature(options.Token, timestamp, nonce, signature);
                    if (check)
                    {
                        // 验证通过需要把微信服务器传递的字符原封不动传回
                        await context.Response.WriteAsync(echostr);
                        return;
                    }
                    // 微信消息验证不通过
                    throw new AbpException("Invalid wechat signature");
                }
            }
            // 不属于微信的消息进入下一个中间件
            await next(context);
        }

        protected bool CheckWeChatSignature(string token, string timestamp, string nonce, string signature)
        {
            var al = new ArrayList
            {
                token,
                timestamp,
                nonce
            };
            // step1 排序
            al.Sort();
            string signatureStr = string.Empty;
            // step2 拼接
            for (int i = 0; i < al.Count; i++)
            {
                signatureStr += al[i];
            }
            // step3 SHA1加密
            byte[] bytes_out = signatureStr.Sha1();
            string result = BitConverter.ToString(bytes_out).Replace("-", "");
            // step4 比对
            if (result.Equals(signature, StringComparison.CurrentCultureIgnoreCase))
            {
                return true;
            }
            return false;
        }
    }
}
