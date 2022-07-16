using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Sms;

namespace LINGYUN.Abp.Account;

public class AccountSmsSecurityCodeSender : IAccountSmsSecurityCodeSender, ITransientDependency
{
    protected ISmsSender SmsSender { get; }

    public AccountSmsSecurityCodeSender(ISmsSender smsSender)
    {
        SmsSender = smsSender;
    }

    public async virtual Task SendSmsCodeAsync(
        string phone,
        string token,
        string template,
        CancellationToken cancellation = default)
    {
        Check.NotNullOrWhiteSpace(template, nameof(template));

        var smsMessage = new SmsMessage(phone, token);
        smsMessage.Properties.Add("code", token);
        smsMessage.Properties.Add("TemplateCode", template);

        await SmsSender.SendAsync(smsMessage);
    }
}
