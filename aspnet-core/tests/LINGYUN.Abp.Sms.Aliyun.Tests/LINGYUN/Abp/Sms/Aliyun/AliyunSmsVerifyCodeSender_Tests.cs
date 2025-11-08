using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Xunit;

namespace LINGYUN.Abp.Sms.Aliyun;
public class AliyunSmsVerifyCodeSender_Tests : AbpAliyunTestBase
{
    protected IAliyunSmsVerifyCodeSender SmsSender { get; }
    protected IConfiguration Configuration { get; }

    public AliyunSmsVerifyCodeSender_Tests()
    {
        SmsSender = GetRequiredService<IAliyunSmsVerifyCodeSender>();
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
            new SmsVerifyCodeMessage(
                phone,
                new SmsVerifyCodeMessageParam(code),
                signName,
                template));
    }
}
