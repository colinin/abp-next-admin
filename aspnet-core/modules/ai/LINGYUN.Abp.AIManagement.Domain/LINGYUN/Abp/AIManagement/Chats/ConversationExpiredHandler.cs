using LINGYUN.Abp.AIManagement.Workspaces;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.Specifications;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.AIManagement.Chats;

public class ConversationExpiredHandler :
    ILocalEventHandler<EntityDeletedEventData<WorkspaceDefinitionRecord>>,
    ITransientDependency
{
    private readonly IClock _clock;
    private readonly IConversationRecordRepository _conversationRecordRepository;

    public ConversationExpiredHandler(
        IClock clock,
        IConversationRecordRepository conversationRecordRepository)
    {
        _clock = clock;
        _conversationRecordRepository = conversationRecordRepository;
    }

    public async virtual Task HandleEventAsync(EntityDeletedEventData<WorkspaceDefinitionRecord> eventData)
    {
        var specification = new ExpressionSpecification<ConversationRecord>(
            x => x.Workspace == eventData.Entity.Name);

        var totalCount = await _conversationRecordRepository.GetCountAsync(specification);
        var conversations = await _conversationRecordRepository.GetListAsync(specification,
            maxResultCount: totalCount);

        foreach (var conversation in conversations )
        {
            conversation.ExpiredAt = _clock.Now;
        }

        await _conversationRecordRepository.UpdateManyAsync(conversations);
    }
}
