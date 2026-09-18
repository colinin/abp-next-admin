using Elsa.Extensions;
using Elsa.Workflows;
using Elsa.Workflows.Attributes;
using Elsa.Workflows.Models;
using Elsa.Workflows.UIHints;
using LINGYUN.Abp.ElsaNext.Email.Contracts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Emailing;
using Volo.Abp.Json;

namespace LINGYUN.Abp.ElsaNext.Email.Activities;

[Activity("Elsa", "Email", "Send an email message.", Kind = ActivityKind.Task)]
public class SendEmail : Activity
{
    /// <summary>
    /// The sender's email address.
    /// </summary>
    [Input(Description = "The sender's email address.")]
    public Input<string?> From { get; set; } = null!;

    /// <summary>
    /// The recipients email addresses.
    /// </summary>
    [Input(Description = "The recipients email addresses.", UIHint = InputUIHints.MultiText)]
    public Input<ICollection<string>> To { get; set; } = null!;

    /// <summary>
    /// The CC recipient email addresses.
    /// </summary>
    [Input(
        Description = "The CC recipient email addresses.",
        UIHint = InputUIHints.MultiText,
        Category = "More")]
    public Input<ICollection<string>> Cc { get; set; } = null!;

    /// <summary>
    /// The BCC recipients email addresses.
    /// </summary>
    [Input(
        Description = "The BCC recipients email addresses.",
        UIHint = InputUIHints.MultiText,
        Category = "More")]
    public Input<ICollection<string>> Bcc { get; set; } = null!;

    /// <summary>
    /// The subject of the email message.
    /// </summary>
    [Input(Description = "The subject of the email message.")]
    public Input<string?> Subject { get; set; } = null!;

    /// <summary>
    /// The attachments to send with the email message.
    /// </summary>
    [Input(
        Description = "The attachments to send with the email message. Can be (an array of) a fully-qualified file path, URL, stream, byte array or instances of EmailAttachment.",
        UIHint = InputUIHints.MultiLine
    )]
    public Input<object?> Attachments { get; set; } = null!;

    /// <summary>
    /// The body of the email message.
    /// </summary>
    [Input(
        Description = "The body of the email message.",
        UIHint = InputUIHints.MultiLine
    )]
    public Input<string> Body { get; set; } = null!;

    /// <summary>
    /// The activity to execute when an error occurs while trying to send the email.
    /// </summary>
    [Port] public IActivity? Error { get; set; }

    /// <inheritdoc />
    protected override async ValueTask ExecuteAsync(ActivityExecutionContext context)
    {
        var cancellationToken = context.CancellationToken;
        var message = new MailMessage();
        var from = From.GetOrDefault(context);

        if (MailAddress.TryCreate(from, out var fromAddress))
        {
            message.Sender = fromAddress;
            message.From = fromAddress;
        }
        message.Subject = Subject.GetOrDefault(context) ?? "";

        await AddAttachmentsAsync(context, message, cancellationToken);

        message.Body = Body.GetOrDefault(context);

        SetRecipientsEmailAddresses(message.To, GetAddresses(context, To));
        SetRecipientsEmailAddresses(message.CC, GetAddresses(context, Cc));
        SetRecipientsEmailAddresses(message.Bcc, GetAddresses(context, Bcc));

        var emailSender = context.GetRequiredService<IEmailSender>();
        var logger = context.GetRequiredService<ILogger<SendEmail>>();

        try
        {
            await emailSender.SendAsync(message);
            await context.CompleteActivityAsync();
        }
        catch (Exception e)
        {
            logger.LogWarning(e, "Error while sending email message");
            context.AddExecutionLogEntry("Error", e.Message, payload: new
            {
                e.StackTrace
            });
            await context.ScheduleActivityAsync(Error, OnErrorCompletedAsync);
        }
    }

    private async ValueTask OnErrorCompletedAsync(ActivityCompletedContext context) => await context.TargetContext.CompleteActivityAsync();

    private static ICollection<string> GetAddresses(ActivityExecutionContext context, Input<ICollection<string>> input)
    {
        return input.GetOrDefault(context) ?? new List<string>(0);
    }

    private async Task AddAttachmentsAsync(ActivityExecutionContext context, MailMessage mailMessage, CancellationToken cancellationToken)
    {
        var attachments = Attachments.GetOrDefault(context);

        if (attachments == null || attachments is string s && string.IsNullOrWhiteSpace(s))
        {
            return;
        }

        var index = 0;
        var attachmentObjects = InterpretAttachmentsModel(attachments);

        foreach (var attachmentObject in attachmentObjects)
        {
            switch (attachmentObject)
            {
                case Uri url:
                    await AttachOnlineFileAsync(context, mailMessage, url, cancellationToken);
                    break;
                case string path when path?.Contains("://") == true:
                    await AttachOnlineFileAsync(context, mailMessage, new Uri(path), cancellationToken);
                    break;
                case string path when !string.IsNullOrWhiteSpace(path):
                    AttachLocalFile(mailMessage, path);
                    break;
                case byte[] bytes:
                    {
                        var fileName = $"Attachment-{++index}";
                        var byteStream = new MemoryStream(bytes.Length);
                        await byteStream.WriteAsync(bytes, cancellationToken);
                        mailMessage.Attachments.Add(new Attachment(byteStream, new ContentType { Name = fileName }));
                        break;
                    }
                case Stream stream:
                    {
                        var fileName = $"Attachment-{++index}";
                        mailMessage.Attachments.Add(new Attachment(stream, new ContentType { Name = fileName }));
                        break;
                    }
                case Attachment attachment:
                    {
                        attachment.Name ??= $"Attachment-{++index}";
                        mailMessage.Attachments.Add(attachment);
                        break;
                    }
                case EmailAttachment emailAttachment when emailAttachment.File != null:
                    {
                        var fileName = emailAttachment.Name ?? $"Attachment-{++index}";
                        var emailAttachmentStream = new MemoryStream(emailAttachment.File.Length);
                        await emailAttachmentStream.WriteAsync(emailAttachment.File, cancellationToken);
                        mailMessage.Attachments.Add(new Attachment(emailAttachmentStream, new ContentType { Name = fileName }));
                        break;
                    }
                case ExpandoObject expandoObject:
                    {
                        var dictionary = new Dictionary<string, object>(expandoObject!, StringComparer.OrdinalIgnoreCase);
                        var fileName = dictionary.GetValue<string>("FileName") ?? $"Attachment-{++index}";
                        var contentType = dictionary.GetValue<string>("ContentType") ?? "application/octet-stream";
                        var parsedContentType = new ContentType(contentType);
                        var content = dictionary.GetValue<object>("Content");

                        if (content is byte[] bytes)
                        {
                            var byteStream = new MemoryStream(bytes.Length);
                            await byteStream.WriteAsync(bytes, cancellationToken);
                            mailMessage.Attachments.Add(new Attachment(byteStream, new ContentType { Name = fileName }));
                        }
                        else if (content is Stream stream)
                        {
                            mailMessage.Attachments.Add(new Attachment(stream, new ContentType { Name = fileName }));
                        }

                        break;
                    }
                default:
                    {
                        var jsonSerializer = context.GetRequiredService<IJsonSerializer>();
                        var json = jsonSerializer.Serialize(attachmentObject);
                        var fileName = $"Attachment-{++index}";

                        var jsonBytes = Encoding.UTF8.GetBytes(json);
                        var byteStream = new MemoryStream(jsonBytes.Length);
                        await byteStream.WriteAsync(jsonBytes, cancellationToken);
                        mailMessage.Attachments.Add(new Attachment(byteStream, new ContentType { Name = fileName }));
                        break;
                    }
            }
        }
    }

    private static void AttachLocalFile(MailMessage mailMessage, string path)
    {
        var stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        mailMessage.Attachments.Add(new Attachment(stream, new ContentType{ Name = Path.GetFileName(path) }));
    }

    private async static Task AttachOnlineFileAsync(ActivityExecutionContext context, MailMessage mailMessage, Uri url, CancellationToken cancellationToken)
    {
        var fileName = Path.GetFileName(url.LocalPath);
        var downloader = context.GetRequiredService<IDownloader>();
        var response = await downloader.DownloadAsync(url, cancellationToken);
        var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);
        var contentType = response.Content.Headers.ContentType?.MediaType ?? "application/binary";
        mailMessage.Attachments.Add(new Attachment(contentStream, new ContentType(contentType) { Name = fileName }));
    }

    private static IEnumerable InterpretAttachmentsModel(object attachments)
    {
        if (attachments is byte[] bytes)
        {
            return new[]
            {
                bytes
            };
        }

        return attachments is string text
            ? new[]
            {
                text
            }
            : attachments as IEnumerable ?? new[]
            {
                attachments
            };
    }

    private static void SetRecipientsEmailAddresses(MailAddressCollection list, IEnumerable<string>? addresses)
    {
        if (addresses == null)
        {
            return;
        }
        foreach (var addresse in addresses)
        {
            if (MailAddress.TryCreate(addresse, out var mailAddress))
            {
                list.Add(mailAddress);
            }
        }
    }
}
