using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Volo.Abp.Sms;
using Xunit;

namespace LINGYUN.Abp.Sms.Aliyun;
public class AliyunSmsVerifyCodeSender_Tests : AbpAliyunTestBase
{
    protected IAliyunSmsVerifyCodeSender SmsVerifyCodeSender { get; }
    protected ISmsSender SmsSender { get; }
    protected IConfiguration Configuration { get; }

    public AliyunSmsVerifyCodeSender_Tests()
    {
        SmsSender = GetRequiredService<ISmsSender>();
        SmsVerifyCodeSender = GetRequiredService<IAliyunSmsVerifyCodeSender>();
        Configuration = GetRequiredService<IConfiguration>();
    }

    /// <summary>
    /// 阿里云短信测试
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    [Theory]
    [InlineData("123456")]
    public async Task Send_Sms_Verify_Code_Test(string code)
    {
        var signName = Configuration["Aliyun:Sms:Sender:SignName"];
        var phone = Configuration["Aliyun:Sms:Sender:PhoneNumber"];
        var template = Configuration["Aliyun:Sms:Sender:TemplateCode"];

        await SmsVerifyCodeSender.SendAsync(
            new SmsVerifyCodeMessage(
                phone,
                new SmsVerifyCodeMessageParam(code),
                signName,
                template));
    }

    /// <summary>
    /// 阿里云短信测试
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    [Theory]
    [InlineData("123456")]
    public async Task Send_Sms_Test(string code)
    {
        var signName = Configuration["Aliyun:Sms:Sender:SignName"];
        var phone = Configuration["Aliyun:Sms:Sender:PhoneNumber"];
        var template = Configuration["Aliyun:Sms:Sender:TemplateCode"];

        var message = new SmsMessage(phone, "test");
        message.Properties.Add("SignName", signName);
        message.Properties.Add("SmsVerifyCode", true);
        message.Properties.Add("TemplateCode", template);
        message.Properties.Add("code", code);

        await SmsSender.SendAsync(message);
    }
}
