using LINGYUN.Platform.Messages;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;

namespace LY.MicroService.PlatformManagement.Messages;
public class MessageCreatedEventHandler :
    IDistributedEventHandler<EntityCreatedEto<SmsMessageEto>>,
    IDistributedEventHandler<EntityCreatedEto<EmailMessageEto>>,
    ITransientDependency
{
    private readonly ISmsMessageManager _smsMessageManager;
    private readonly ISmsMessageRepository _smsMessageRepository;

    private readonly IEmailMessageManager _emailMessageManager;
    private readonly IEmailMessageRepository _emailMessageRepository;

    public MessageCreatedEventHandler(
        ISmsMessageManager smsMessageManager,
        ISmsMessageRepository smsMessageRepository,
        IEmailMessageManager emailMessageManager,
        IEmailMessageRepository emailMessageRepository)
    {
        _smsMessageManager = smsMessageManager;
        _smsMessageRepository = smsMessageRepository;
        _emailMessageManager = emailMessageManager;
        _emailMessageRepository = emailMessageRepository;
    }

    [UnitOfWork]
    public async virtual Task HandleEventAsync(EntityCreatedEto<SmsMessageEto> eventData)
    {
        var smsMessage = await _smsMessageRepository.GetAsync(eventData.Entity.Id);

        smsMessage = await _smsMessageManager.SendAsync(smsMessage);

        await _smsMessageRepository.UpdateAsync(smsMessage);
    }

    [UnitOfWork]
    public async virtual Task HandleEventAsync(EntityCreatedEto<EmailMessageEto> eventData)
    {
        var emailMessage = await _emailMessageRepository.GetAsync(eventData.Entity.Id);

        emailMessage = await _emailMessageManager.SendAsync(emailMessage);

        await _emailMessageRepository.UpdateAsync(emailMessage);
    }
}
