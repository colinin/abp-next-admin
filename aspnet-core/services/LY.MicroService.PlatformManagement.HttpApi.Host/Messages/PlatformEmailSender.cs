using LINGYUN.Platform.Messages;
using LINGYUN.Platform.Messages.Integration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Threading.Tasks;
using Volo.Abp.Content;
using Volo.Abp.Data;
using Volo.Abp.Emailing;

namespace LY.MicroService.PlatformManagement.Messages;

public class PlatformEmailSender : IEmailSender
{
    private readonly IEmailMessageIntegrationService _service;
    public PlatformEmailSender(IEmailMessageIntegrationService service)
    {
        _service = service;
    }

    public virtual Task QueueAsync(string to, string subject, string body, bool isBodyHtml = true, AdditionalEmailSendingArgs additionalEmailSendingArgs = null)
    {
        return SendAsync(from: null, to, subject, body, isBodyHtml, additionalEmailSendingArgs);
    }

    public virtual Task QueueAsync(string from, string to, string subject, string body, bool isBodyHtml = true, AdditionalEmailSendingArgs additionalEmailSendingArgs = null)
    {
        return SendAsync(from, to, subject, body, isBodyHtml, additionalEmailSendingArgs);
    }

    public virtual Task SendAsync(string to, string subject, string body, bool isBodyHtml = true, AdditionalEmailSendingArgs additionalEmailSendingArgs = null)
    {
        return SendAsync(from: null, to, subject, body, isBodyHtml, additionalEmailSendingArgs);
    }

    public async virtual Task SendAsync(string from, string to, string subject, string body, bool isBodyHtml = true, AdditionalEmailSendingArgs additionalEmailSendingArgs = null)
    {
        var createInput = new EmailMessageCreateDto(
            to,
            body,
            from,
            subject,
            isBodyHtml,
            additionalEmailSendingArgs?.CC?.JoinAsString(","))
        {
            Normalize = true
        };

        if (additionalEmailSendingArgs?.Attachments != null)
        {
            var attachments = new List<IRemoteStreamContent>();

            foreach (var attachment in additionalEmailSendingArgs.Attachments)
            {
                var stream = new MemoryStream(attachment.File.Length);

                await stream.WriteAsync(attachment.File, 0, attachment.File.Length);

                stream.Seek(0, SeekOrigin.Begin);

                attachments.Add(new RemoteStreamContent(stream, attachment.Name));
            }

            createInput.Attachments = attachments.ToArray();
        }
        if (additionalEmailSendingArgs?.ExtraProperties != null)
        {
            createInput.ExtraProperties = new ExtraPropertyDictionary();
            foreach (var prop in additionalEmailSendingArgs.ExtraProperties)
            {
                createInput.ExtraProperties.Add(prop.Key, prop.Value);
            }
        }

        await _service.CreateAsync(createInput);
    }

    public async virtual Task SendAsync(MailMessage mail, bool normalize = true)
    {
        var createInput = new EmailMessageCreateDto(
            mail.To.ToString(),
            mail.Body,
            mail.From?.ToString(),
            mail.Subject,
            mail.IsBodyHtml,
            mail.CC?.ToString())
        {
            Normalize = normalize,
            Priority = mail.Priority,
            BodyTransferEncoding = mail.BodyTransferEncoding,
            DeliveryNotificationOptions = mail.DeliveryNotificationOptions
        };

        if (mail.Attachments != null)
        {
            var attachments = new List<IRemoteStreamContent>();

            foreach (var attachment in mail.Attachments)
            {
                attachments.Add(
                    new RemoteStreamContent(
                        attachment.ContentStream, 
                        attachment.Name,
                        attachment.ContentType?.ToString()));
            }

            createInput.Attachments = attachments.ToArray();
        }

        if (mail.Headers != null)
        {
            var headers = new List<EmailMessageHeaderDto>();
            foreach (var key in mail.Headers.AllKeys)
            {
                var value = mail.Headers.Get(key);
                if (!value.IsNullOrWhiteSpace())
                {
                    headers.Add(new EmailMessageHeaderDto(key, value));
                }
            }
            createInput.Headers = headers;
        }

        await _service.CreateAsync(createInput);
    }
}
