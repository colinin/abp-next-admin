using System.Threading.Tasks;
using Volo.Abp.BlobStoring;

namespace LINGYUN.Platform.Messages.Integration;

public class EmailMessageIntegrationService : PlatformApplicationServiceBase, IEmailMessageIntegrationService
{
    private readonly IBlobContainer<MessagingContainer> _blobContainer;
    private readonly IEmailMessageRepository _emailMessageRepository;

    public EmailMessageIntegrationService(
        IBlobContainer<MessagingContainer> blobContainer, 
        IEmailMessageRepository emailMessageRepository)
    {
        _blobContainer = blobContainer;
        _emailMessageRepository = emailMessageRepository;
    }

    public async virtual Task<EmailMessageDto> CreateAsync(EmailMessageCreateDto input)
    {
        var emailMessage = new EmailMessage(
            GuidGenerator.Create(),
            input.To,
            input.From,
            input.Subject,
            input.Content,
            input.IsBodyHtml,
            input.CC,
            CurrentUser.Id,
            CurrentUser.UserName)
        {
            Normalize = input.Normalize,
            Priority = input.Priority,
            BodyTransferEncoding = input.BodyTransferEncoding,
            DeliveryNotificationOptions = input.DeliveryNotificationOptions
        };
        if (input.Headers != null)
        {
            foreach (var header in input.Headers)
            {
                emailMessage.AddHeader(header.Key, header.Value);
            }
        }
        if (input.Attachments != null)
        {
            foreach (var attachment in input.Attachments)
            {
                var attachmentStream = attachment.GetStream();

                var attachmentNamePrefix = $"attachments/{Clock.Now:yyyy-MM-dd}";
                var attachmentName = $"{attachmentNamePrefix}/{emailMessage.Id}/{attachment.FileName}";

                await _blobContainer.SaveAsync(attachmentName, attachmentStream, overrideExisting: true);

                emailMessage.AddAttachment(attachment.FileName, attachmentName, attachmentStream.Length);
            }
        }

        emailMessage = await _emailMessageRepository.InsertAsync(emailMessage);

        await CurrentUnitOfWork.SaveChangesAsync();

        return ObjectMapper.Map<EmailMessage, EmailMessageDto>(emailMessage);
    }
}
