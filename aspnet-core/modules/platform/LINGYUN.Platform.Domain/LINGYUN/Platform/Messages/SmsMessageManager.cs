using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.Abp.DynamicProxy;
using Volo.Abp.Sms;

namespace LINGYUN.Platform.Messages;

public class SmsMessageManager : DomainService, ISmsMessageManager
{
    protected ISmsMessageRepository Repository { get; }
    public SmsMessageManager(ISmsMessageRepository repository)
    {
        Repository = repository;
    }

    public async virtual Task<SmsMessage> SendAsync(SmsMessage message)
    {
        var smsSender = GetSmsSender();

        message.Provider = ProxyHelper.GetUnProxiedType(smsSender).Name;

        var error = await TrySendAsync(smsSender, message);
        if (error.IsNullOrWhiteSpace())
        {
            message.Sent(Clock);
        }
        else
        {
            message.Failed(error, Clock);
        }

        return message;
    }

    public async virtual Task<string> TrySendAsync(ISmsSender smsSender, SmsMessage message)
    {
        try
        {
            var smsMessage = new Volo.Abp.Sms.SmsMessage(
                message.Receiver,
                message.Content);

            foreach (var prop in message.ExtraProperties)
            {
                smsMessage.Properties.Add(prop.Key, prop.Value);
            }

            await smsSender.SendAsync(smsMessage);

            return null;
        }
        catch (Exception ex)
        {
            Logger.LogWarning("Failed to send a short message, error: {message}", ex.ToString());

            return ex.Message;
        }
    }

    protected virtual ISmsSender GetSmsSender()
    {
        return LazyServiceProvider.LazyGetRequiredService<ISmsSender>();
    }
}
