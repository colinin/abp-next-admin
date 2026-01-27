using LINGYUN.Abp.AI.Chats;
using LINGYUN.Abp.AI.Models;
using LINGYUN.Abp.AIManagement.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.AIManagement.Chats;

[Dependency(ReplaceServices = true)]
public class ChatMessageStore : IChatMessageStore, ITransientDependency
{
    private readonly ICurrentTenant _currentTenant;
    private readonly IGuidGenerator _guidGenerator;
    private readonly ISettingProvider _settingProvider;
    private readonly IObjectMapper<AbpAIManagementDomainModule> _objectMapper;
    private readonly ITextChatMessageRecordRepository _messageRecordRepository;

    public ChatMessageStore(
        ICurrentTenant currentTenant, 
        IGuidGenerator guidGenerator,
        ISettingProvider settingProvider,
        IObjectMapper<AbpAIManagementDomainModule> objectMapper,
        ITextChatMessageRecordRepository messageRecordRepository)
    {
        _currentTenant = currentTenant;
        _guidGenerator = guidGenerator;
        _settingProvider = settingProvider;
        _objectMapper = objectMapper;
        _messageRecordRepository = messageRecordRepository;
    }

    public async virtual Task<IEnumerable<ChatMessage>> GetHistoryMessagesAsync(Guid conversationId)
    {
        var maxLatestHistoryMessagesToKeep = await _settingProvider.GetAsync(
            AIManagementSettingNames.ChatMessage.MaxLatestHistoryMessagesToKeep, 0);
        if (maxLatestHistoryMessagesToKeep < 1)
        {
            return Array.Empty<ChatMessage>();
        }

        var userTextMessages = await _messageRecordRepository.GetHistoryMessagesAsync(conversationId, maxLatestHistoryMessagesToKeep);

        return userTextMessages.Select(msg =>
        {
            var chatMessage = new TextChatMessage(msg.Workspace, msg.Content, msg.Role, msg.CreatedAt);
            chatMessage.WithMessageId(msg.Id);
            if (msg.ConversationId.HasValue)
            {
                chatMessage.WithConversationId(msg.ConversationId.Value);
            }
            if (!msg.ReplyMessage.IsNullOrWhiteSpace() && msg.ReplyAt.HasValue)
            {
                chatMessage.WithReply(msg.ReplyMessage, msg.ReplyAt.Value);
            }

            return chatMessage;
        });
    }

    public async virtual Task<Guid> SaveMessageAsync(ChatMessage message)
    {
        var messageId = message.Id;
        if (!messageId.HasValue)
        {
            messageId = _guidGenerator.Create();
            message.WithMessageId(messageId.Value);
        }

        await StoreMessageAsync(messageId.Value, message);

        return messageId.Value;
    }

    protected async virtual Task StoreMessageAsync(Guid messageId, ChatMessage message)
    {
        switch (message)
        {
            case TextChatMessage textMessage:
                await StoreUserTextMessageAsync(messageId, textMessage);
                break;
        }
    }

    protected async virtual Task StoreUserTextMessageAsync(Guid messageId, TextChatMessage textMessage)
    {
        var textMessageRecord = await _messageRecordRepository.FindAsync(messageId);
        if (textMessageRecord == null)
        {
            textMessageRecord = new TextChatMessageRecord(
                messageId,
                textMessage.Workspace,
                textMessage.Content,
                textMessage.Role,
                textMessage.CreatedAt,
                _currentTenant.Id);

            UpdateUserMessageRecord(textMessageRecord, textMessage);

            await _messageRecordRepository.InsertAsync(textMessageRecord);
        }
        else
        {
            textMessageRecord.SetContent(textMessage.Content);

            UpdateUserMessageRecord(textMessageRecord, textMessage);

            await _messageRecordRepository.UpdateAsync(textMessageRecord);
        }
    }

    private static void UpdateUserMessageRecord(ChatMessageRecord messageRecord, ChatMessage message)
    {
        if (message.ConversationId.HasValue)
        {
            messageRecord.SetConversationId(message.ConversationId.Value);
        }
        if (!message.ReplyMessage.IsNullOrWhiteSpace())
        {
            messageRecord.SetReply(message.ReplyMessage, message.ReplyAt);
        }
    }
}
