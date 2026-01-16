using LINGYUN.Abp.AI.Messages;
using LINGYUN.Abp.AI.Models;
using LINGYUN.Abp.AIManagement.Settings;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.AIManagement.Messages;

[Dependency(ReplaceServices = true)]
public class UserMessageStore : IUserMessageStore, ITransientDependency
{
    private readonly ICurrentTenant _currentTenant;
    private readonly IGuidGenerator _guidGenerator;
    private readonly ISettingProvider _settingProvider;
    private readonly IObjectMapper<AbpAIManagementDomainModule> _objectMapper;
    private readonly IUserTextMessageRecordRepository _messageRecordRepository;

    public UserMessageStore(
        ICurrentTenant currentTenant, 
        IGuidGenerator guidGenerator,
        ISettingProvider settingProvider,
        IObjectMapper<AbpAIManagementDomainModule> objectMapper,
        IUserTextMessageRecordRepository messageRecordRepository)
    {
        _currentTenant = currentTenant;
        _guidGenerator = guidGenerator;
        _settingProvider = settingProvider;
        _objectMapper = objectMapper;
        _messageRecordRepository = messageRecordRepository;
    }

    public async virtual Task<IEnumerable<UserMessage>> GetHistoryMessagesAsync(string conversationId)
    {
        var maxLatestHistoryMessagesToKeep = await _settingProvider.GetAsync(AIManagementSettingNames.UserMessage.MaxLatestHistoryMessagesToKeep, 0);
        if (maxLatestHistoryMessagesToKeep < 1)
        {
            return Array.Empty<UserMessage>();
        }

        var userTextMessages = await _messageRecordRepository.GetHistoryMessagesAsync(conversationId, maxLatestHistoryMessagesToKeep);

        return _objectMapper.Map<IEnumerable<UserTextMessageRecord>, IEnumerable<UserTextMessage>>(userTextMessages);
    }

    public async virtual Task<string> SaveMessageAsync(UserMessage message)
    {
        var messageId = message.Id;
        if (messageId.IsNullOrWhiteSpace())
        {
            messageId = _guidGenerator.Create().ToString();
            message.WithMessageId(messageId);
        }

        await StoreMessageAsync(Guid.Parse(messageId), message);

        return messageId;
    }

    protected async virtual Task StoreMessageAsync(Guid messageId, UserMessage message)
    {
        switch (message)
        {
            case UserTextMessage textMessage:
                await StoreUserTextMessageAsync(messageId, textMessage);
                break;
        }
    }

    protected async virtual Task StoreUserTextMessageAsync(Guid messageId, UserTextMessage textMessage)
    {
        var textMessageRecord = await _messageRecordRepository.FindAsync(messageId);
        if (textMessageRecord == null)
        {
            textMessageRecord = new UserTextMessageRecord(
                messageId,
                textMessage.Workspace,
                textMessage.Content,
                _currentTenant.Id);

            UpdateUserMessageRecord(textMessageRecord, textMessage);

            await _messageRecordRepository.InsertAsync(textMessageRecord);
        }
        else
        {
            textMessageRecord.WithContent(textMessage.Content);

            UpdateUserMessageRecord(textMessageRecord, textMessage);

            await _messageRecordRepository.UpdateAsync(textMessageRecord);
        }
    }

    private static void UpdateUserMessageRecord(UserMessageRecord messageRecord, UserMessage message)
    {
        if (!message.ConversationId.IsNullOrWhiteSpace())
        {
            messageRecord.WithConversationId(message.ConversationId);
        }
        if (!message.ReplyMessage.IsNullOrWhiteSpace())
        {
            messageRecord.WithConversationId(message.ReplyMessage);
        }
    }
}
