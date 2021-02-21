using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Sms;
using Xunit;

namespace LINGYUN.Abp.Sms.Aliyun
{
    public class AliyunSmsSenderTests : AbpAliyunTestBase
    {
        protected ISmsSender SmsSender { get; }
        protected IConfiguration Configuration { get; }

        public AliyunSmsSenderTests()
        {
            SmsSender = GetRequiredService<ISmsSender>();
            Configuration = GetRequiredService<IConfiguration>();
        }

        /// <summary>
        /// 阿里云短信测试
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [Theory]
        [InlineData("123456")]
        public async Task Send_Test(string code)
        {
            var signName = Configuration["Aliyun:Sms:Sender:SignName"];
            var phone = Configuration["Aliyun:Sms:Sender:PhoneNumber"];
            var template = Configuration["Aliyun:Sms:Sender:TemplateCode"];

            await SmsSender.SendAsync(
                signName, 
                template, 
                phone, 
                new Dictionary<string, object>
                {
                    { "code", code }
                });
        }
    }
}
