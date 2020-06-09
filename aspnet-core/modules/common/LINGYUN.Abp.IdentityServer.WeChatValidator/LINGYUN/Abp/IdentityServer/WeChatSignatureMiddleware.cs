using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.IdentityServer
{
    public class WeChatSignatureMiddleware : IMiddleware, ITransientDependency
    {
        protected WeChatSignatureOptions Options { get; }
        public WeChatSignatureMiddleware(IOptions<WeChatSignatureOptions> options)
        {
            Options = options.Value;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (context.Request.Path.HasValue)
            {
                var requestPath = context.Request.Path.Value;
                if (requestPath.Equals(Options.RequestPath))
                {
                    var timestamp = context.Request.Query["timestamp"];
                    var nonce = context.Request.Query["nonce"];
                    var signature = context.Request.Query["signature"];
                    var echostr = context.Request.Query["echostr"];
                    var check = CheckWeChatSignature(Options.Token, timestamp, nonce, signature);
                    if (check)
                    {
                        await context.Response.WriteAsync(echostr);
                        return;
                    }
                    throw new AbpException("微信验证不通过");
                }
            }
            await next(context);
        }

        protected bool CheckWeChatSignature(string token, string timestamp, string nonce, string signature)
        {
            var al = new ArrayList();
            al.Add(token);
            al.Add(timestamp);
            al.Add(nonce);
            al.Sort();
            string signatureStr = string.Empty;
            for(int i = 0; i < al.Count; i++)
            {
                signatureStr += al[i];
            }
            using (var sha1 = new SHA1CryptoServiceProvider())
            {
                byte[] bytes_in = Encoding.ASCII.GetBytes(signatureStr);
                byte[] bytes_out = sha1.ComputeHash(bytes_in);
                string result = BitConverter.ToString(bytes_out).Replace("-", "");
                if (result.Equals(signature, StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }
                return false;
            }
        }
    }
}
