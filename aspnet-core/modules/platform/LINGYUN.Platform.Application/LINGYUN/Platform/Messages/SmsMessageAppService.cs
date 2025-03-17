using LINGYUN.Platform.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Platform.Messages;

[Authorize(PlatformPermissions.SmsMessage.Default)]
public class SmsMessageAppService : PlatformApplicationServiceBase, ISmsMessageAppService
{
    private readonly ISmsMessageManager _smsMessageManager;
    private readonly ISmsMessageRepository _smsMessageRepository;

    public SmsMessageAppService(
        ISmsMessageManager smsMessageManager,
        ISmsMessageRepository smsMessageRepository)
    {
        _smsMessageManager = smsMessageManager;
        _smsMessageRepository = smsMessageRepository;
    }

    [Authorize(PlatformPermissions.SmsMessage.Delete)]
    public async virtual Task DeleteAsync(Guid id)
    {
        var smsMessage = await _smsMessageRepository.GetAsync(id);

        await _smsMessageRepository.DeleteAsync(smsMessage);

        await CurrentUnitOfWork.SaveChangesAsync();
    }

    public async virtual Task<SmsMessageDto> GetAsync(Guid id)
    {
        var smsMessage = await _smsMessageRepository.GetAsync(id);

        return ObjectMapper.Map<SmsMessage, SmsMessageDto>(smsMessage);
    }

    public async virtual Task<PagedResultDto<SmsMessageDto>> GetListAsync(SmsMessageGetListInput input)
    {
        var specification = new SmsMessageGetListSpecification(input);

        var totalCount = await _smsMessageRepository.GetCountAsync(specification);
        var smsMessages = await _smsMessageRepository.GetListAsync(
            specification,
            input.Sorting,
            input.MaxResultCount,
            input.SkipCount
        );

        return new PagedResultDto<SmsMessageDto>(totalCount,
            ObjectMapper.Map<List<SmsMessage>, List<SmsMessageDto>>(smsMessages)
        );
    }

    [Authorize(PlatformPermissions.SmsMessage.SendMessage)]
    public async virtual Task SendAsync(Guid id)
    {
        var smsMessage = await _smsMessageRepository.GetAsync(id);

        await _smsMessageManager.SendAsync(smsMessage);

        await _smsMessageRepository.UpdateAsync(smsMessage);

        await CurrentUnitOfWork.SaveChangesAsync();
    }

    private class SmsMessageGetListSpecification : Volo.Abp.Specifications.Specification<SmsMessage>
    {
        protected SmsMessageGetListInput Input { get; }
        public SmsMessageGetListSpecification(SmsMessageGetListInput input)
        {
            Input = input;
        }

        public override Expression<Func<SmsMessage, bool>> ToExpression()
        {
            Expression<Func<SmsMessage, bool>> expression = _ => true;

            return expression
                .AndIf(!Input.PhoneNumber.IsNullOrWhiteSpace(), x => x.Receiver.Contains(Input.PhoneNumber))
                .AndIf(!Input.Content.IsNullOrWhiteSpace(), x => x.Content.Contains(Input.Content))
                .AndIf(Input.Status.HasValue, x => x.Status == Input.Status)
                .AndIf(Input.BeginSendTime.HasValue, x => x.SendTime >= Input.BeginSendTime)
                .AndIf(Input.EndSendTime.HasValue, x => x.SendTime <= Input.EndSendTime);
        }
    }
}
