using LINGYUN.Abp.AI.Chats;
using LINGYUN.Abp.AI.Models;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Specifications;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.AIManagement.Chats;

[Dependency(ReplaceServices = true)]
public class ConversationStore : IConversationStore, ITransientDependency
{
    private readonly IClock _clock;
    private readonly ICurrentTenant _currentTenant;
    private readonly ConversationCleanupOptions _cleanupOptions;
    private readonly IConversationRecordRepository _conversationRecordRepository;

    public ConversationStore(
        IClock clock, 
        ICurrentTenant currentTenant,
        IOptions<ConversationCleanupOptions> cleanupOptions,
        IConversationRecordRepository conversationRecordRepository)
    {
        _clock = clock;
        _currentTenant = currentTenant;
        _cleanupOptions = cleanupOptions.Value;
        _conversationRecordRepository = conversationRecordRepository;
    }

    public async virtual Task CleanupAsync()
    {
        if (!_cleanupOptions.IsCleanupEnabled)
        {
            return;
        }

        var specification = new ExpressionSpecification<ConversationRecord>(
            x => x.ExpiredAt <= _clock.Now);

        var totalCount = await _conversationRecordRepository.GetCountAsync(specification);
        var expiredRecords = await _conversationRecordRepository.GetListAsync(specification, maxResultCount: totalCount);

        await _conversationRecordRepository.DeleteManyAsync(expiredRecords);
    }

    public async virtual Task<Conversation?> FindAsync(Guid conversationId)
    {
        var conversationRecord = await _conversationRecordRepository.FindAsync(conversationId);
        if (conversationRecord == null)
        {
            return null;
        }

        var conversation = new Conversation(
            conversationRecord.Id,
            conversationRecord.Name,
            conversationRecord.CreatedAt)
        {
            UpdateAt = conversationRecord.UpdateAt,
            ExpiredAt = conversationRecord.ExpiredAt,
        };


        return conversation;
    }

    public async virtual Task SaveAsync(Conversation conversation)
    {
        var conversationRecord = await _conversationRecordRepository.FindAsync(conversation.Id);
        if (conversationRecord == null)
        {
            var expiredTime = conversation.CreatedAt.Add(_cleanupOptions.ExpiredTime);
            conversationRecord = new ConversationRecord(
                conversation.Id,
                conversation.Name,
                conversation.CreatedAt,
                expiredTime,
                _currentTenant.Id);

            await _conversationRecordRepository.InsertAsync(conversationRecord);
        }
        else
        {
            var expiredTime = (conversation.UpdateAt ?? _clock.Now).Add(_cleanupOptions.ExpiredTime);
            conversationRecord.UpdateAt = conversation.UpdateAt;
            conversationRecord.ExpiredAt = expiredTime;

            await _conversationRecordRepository.UpdateAsync(conversationRecord);
        }
    }
}
