using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Volo.Abp.BlobStoring;
using Volo.Abp.Domain.Services;
using Volo.Abp.DynamicProxy;
using Volo.Abp.Emailing;
using Volo.Abp.Settings;

namespace LINGYUN.Platform.Messages;
public class EmailMessageManager : DomainService, IEmailMessageManager
{
    private const string FromAddressPattern = @"([a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,})";

    protected ISettingProvider SettingProvider { get; }
    protected IEmailMessageRepository Repository { get; }
    protected IBlobContainer<MessagingContainer> BlobContainer { get; }

    public EmailMessageManager(
        ISettingProvider settingProvider,
        IEmailMessageRepository repository,
        IBlobContainer<MessagingContainer> blobContainer)
    {
        SettingProvider = settingProvider;
        Repository = repository;
        BlobContainer = blobContainer;
    }

    public async virtual Task<EmailMessage> SendAsync(EmailMessage message)
    {
        var emailSender = GetEmailSender();

        message.Provider = ProxyHelper.GetUnProxiedType(emailSender).Name;

        var error = await TrySendAsync(emailSender, message);

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

    public async virtual Task<string> TrySendAsync(IEmailSender emailSender, EmailMessage message)
    {
        try
        {
            MailAddress from;
            if (message.From.IsNullOrWhiteSpace())
            {
                var defaultFrom = await SettingProvider.GetOrNullAsync(EmailSettingNames.DefaultFromAddress);
                var defaultFromDisplayName = await SettingProvider.GetOrNullAsync(EmailSettingNames.DefaultFromDisplayName);
                from = new MailAddress(defaultFrom, defaultFromDisplayName);

                message.From = $"{defaultFrom}{(defaultFromDisplayName.IsNullOrWhiteSpace() ? "" : $"<{defaultFromDisplayName}>")}";
            }
            else
            {
                var match = Regex.Match(message.From, FromAddressPattern);
                if (match.Success)
                {
                    from = new MailAddress(match.Value);
                }
                else
                {
                    from = new MailAddress(message.From);
                }
            }
            var to = new MailAddress(message.Receiver);

            var mailMessage = new MailMessage(from, to)
            {
                Subject = message.Subject,
                Body = message.Content,
                IsBodyHtml = message.IsBodyHtml,
            };

            if (!message.CC.IsNullOrWhiteSpace())
            {
                mailMessage.CC.Add(message.CC);
            }

            if (message.Priority.HasValue)
            {
                mailMessage.Priority = message.Priority.Value;
            }
            if (message.BodyTransferEncoding.HasValue)
            {
                mailMessage.BodyTransferEncoding = message.BodyTransferEncoding.Value;
            }
            if (message.DeliveryNotificationOptions.HasValue)
            {
                mailMessage.DeliveryNotificationOptions = message.DeliveryNotificationOptions.Value;
            }

            foreach (var header in message.Headers)
            {
                mailMessage.Headers.Add(header.Key, header.Value);
            }

            foreach (var attachment in message.Attachments)
            {
                var blob = await BlobContainer.GetOrNullAsync(attachment.BlobName);
                if (blob != null && blob != Stream.Null)
                {
                    mailMessage.Attachments.Add(new Attachment(blob, attachment.Name));
                }
            }

            await emailSender.SendAsync(mailMessage, message.Normalize);

            return null;
        }
        catch(Exception ex)
        {
            Logger.LogWarning("Failed to send a email message, error: {message}", ex.ToString());

            return ex.Message;
        }
    }

    protected virtual IEmailSender GetEmailSender()
    {
        return LazyServiceProvider.LazyGetRequiredService<IEmailSender>();
    }
}
