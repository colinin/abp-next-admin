using LINGYUN.Abp.Sms.Aliyun;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Account.Security.Aliyun;

[Dependency(ServiceLifetime.Transient, ReplaceServices = true)]
[ExposeServices(typeof(IAccountSmsSecurityCodeSender), typeof(AliyunAccountSmsSecurityCodeSender))]
public class AliyunAccountSmsSecurityCodeSender : IAccountSmsSecurityCodeSender
{
    protected IAliyunSmsVerifyCodeSender SmsVerifyCodeSender { get; }
    public AliyunAccountSmsSecurityCodeSender(
        IAliyunSmsVerifyCodeSender smsVerifyCodeSender)
    {
        SmsVerifyCodeSender = smsVerifyCodeSender;
    }

    public async virtual Task SendAsync(
        string phone,
        string code,
        string templateCode,
        CancellationToken cancellation = default)
    {
        // TODO: 传递验证码有效期
        await SmsVerifyCodeSender.SendAsync(
            new SmsVerifyCodeMessage(phone, 
                new SmsVerifyCodeMessageParam(code, "5"),
                templateCode: templateCode));
    }
}
