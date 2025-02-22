using LINGYUN.Platform.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Platform.Messages;

[Authorize(PlatformPermissions.EmailMessage.Default)]
public class EmailMessageAppService : PlatformApplicationServiceBase, IEmailMessageAppService
{
    private readonly IEmailMessageManager _emailMessageManager;
    private readonly IEmailMessageRepository _emailMessageRepository;

    public EmailMessageAppService(
        IEmailMessageManager emailMessageManager,
        IEmailMessageRepository emailMessageRepository)
    {
        _emailMessageManager = emailMessageManager;
        _emailMessageRepository = emailMessageRepository;
    }

    [Authorize(PlatformPermissions.EmailMessage.Delete)]
    public async virtual Task DeleteAsync(Guid id)
    {
        var emailMessage = await _emailMessageRepository.GetAsync(id);

        await _emailMessageRepository.DeleteAsync(emailMessage);

        await CurrentUnitOfWork.SaveChangesAsync();
    }

    public async virtual Task<EmailMessageDto> GetAsync(Guid id)
    {
        var emailMessage = await _emailMessageRepository.GetAsync(id);

        return ObjectMapper.Map<EmailMessage, EmailMessageDto>(emailMessage);
    }

    public async virtual Task<PagedResultDto<EmailMessageDto>> GetListAsync(EmailMessageGetListInput input)
    {
        var specification = new EmailMessageGetListSpecification(input);

        var totalCount = await _emailMessageRepository.GetCountAsync(specification);
        var emailMessages = await _emailMessageRepository.GetListAsync(
            specification,
            input.Sorting,
            input.MaxResultCount,
            input.SkipCount
        );

        return new PagedResultDto<EmailMessageDto>(totalCount,
            ObjectMapper.Map<List<EmailMessage>, List<EmailMessageDto>>(emailMessages)
        );
    }

    [Authorize(PlatformPermissions.EmailMessage.SendMessage)]
    public async virtual Task SendAsync(Guid id)
    {
        var emailMessage = await _emailMessageRepository.GetAsync(id);

        await _emailMessageManager.SendAsync(emailMessage);

        await _emailMessageRepository.UpdateAsync(emailMessage);

        await CurrentUnitOfWork.SaveChangesAsync();
    }

    private class EmailMessageGetListSpecification : Volo.Abp.Specifications.Specification<EmailMessage>
    {
        protected EmailMessageGetListInput Input { get; }
        public EmailMessageGetListSpecification(EmailMessageGetListInput input)
        {
            Input = input;
        }

        public override Expression<Func<EmailMessage, bool>> ToExpression()
        {
            Expression<Func<EmailMessage, bool>> expression = _ => true;

            return expression
                .AndIf(!Input.EmailAddress.IsNullOrWhiteSpace(), x => x.Receiver.Contains(Input.EmailAddress))
                .AndIf(!Input.Subject.IsNullOrWhiteSpace(), x => x.Subject.Contains(Input.Subject))
                .AndIf(!Input.Content.IsNullOrWhiteSpace(), x => x.Content.Contains(Input.Content))
                .AndIf(!Input.From.IsNullOrWhiteSpace(), x => x.From.Contains(Input.From))
                .AndIf(Input.Status.HasValue, x => x.Status == Input.Status)
                .AndIf(Input.Priority.HasValue, x => x.Priority == Input.Priority)
                .AndIf(Input.BeginSendTime.HasValue, x => x.SendTime >= Input.BeginSendTime)
                .AndIf(Input.EndSendTime.HasValue, x => x.SendTime <= Input.EndSendTime);
        }
    }
}
