using LINGYUN.Abp.AI;
using LINGYUN.Abp.AIManagement.Localization;
using LINGYUN.Abp.AIManagement.Tokens;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Localization;
using System;
using System.Globalization;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Guids;
using Volo.Abp.Localization;
using Volo.Abp.Specifications;
using Volo.Abp.Uow;

namespace LINGYUN.Abp.AIManagement.Chats;
public class ConversationChangeNameHandler :
    IDistributedEventHandler<EntityCreatedEto<TextChatMessageRecordEto>>,
    ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IGuidGenerator _guidGenerator;
    private readonly IAbpDistributedLock _distributedLock;
    private readonly IChatClientFactory _chatClientFactory;
    private readonly IStringLocalizer<AIManagementResource> _stringLocalizer;
    private readonly ITokenUsageRecordRepository _tokenUsageRecordRepository;
    private readonly IConversationRecordRepository _conversationRecordRepository;
    private readonly ITextChatMessageRecordRepository _textChatMessageRecordRepository;

    public ConversationChangeNameHandler(
        IServiceProvider serviceProvider,
        IGuidGenerator guidGenerator,
        IAbpDistributedLock distributedLock,
        IChatClientFactory chatClientFactory, 
        IStringLocalizer<AIManagementResource> stringLocalizer,
        ITokenUsageRecordRepository tokenUsageRecordRepository, 
        IConversationRecordRepository conversationRecordRepository, 
        ITextChatMessageRecordRepository textChatMessageRecordRepository)
    {
        _serviceProvider = serviceProvider;
        _guidGenerator = guidGenerator;
        _distributedLock = distributedLock;
        _chatClientFactory = chatClientFactory;
        _stringLocalizer = stringLocalizer;
        _tokenUsageRecordRepository = tokenUsageRecordRepository;
        _conversationRecordRepository = conversationRecordRepository;
        _textChatMessageRecordRepository = textChatMessageRecordRepository;
    }

    [UnitOfWork]
    public async virtual Task HandleEventAsync(EntityCreatedEto<TextChatMessageRecordEto> eventData)
    {
        /*
         * 业务逻辑: 当用户第一次向智能体发送消息时,系统根据用户消息创建一个额外的智能体
         * 通过消息摘要生成一个简短有效的问题描述,用作本次对话的名称, 设计灵感: Deepseek
         */

        if (!eventData.Entity.ConversationId.HasValue)
        {
            return;
        }

        await using var lockHandle = await _distributedLock.TryAcquireAsync($"{nameof(ConversationChangeNameHandler)}_{eventData.Entity.ConversationId}_DesignConversationName");

        if (lockHandle == null)
        {
            return;
        }

        var conversation = await _conversationRecordRepository.FindAsync(eventData.Entity.ConversationId.Value);
        if (conversation == null)
        {
            return;
        }

        var specification = new ExpressionSpecification<TextChatMessageRecord>(
                x => x.ConversationId == eventData.Entity.ConversationId && x.Role == ChatRole.User);

        var historyChatMessages = await _textChatMessageRecordRepository.GetListAsync(
            specification: specification,
            sorting: nameof(TextChatMessageRecord.CreationTime),
            maxResultCount: 2);

        if (historyChatMessages.Count > 1)
        {
            return;
        }

        var chatMessage = historyChatMessages[0];
        var currentCulture = chatMessage.GetProperty(nameof(CultureInfo.CurrentCulture), CultureInfo.CurrentCulture.Name);

        using (CultureHelper.Use(currentCulture!))
        {
            var chatClient = await _chatClientFactory.CreateAsync(chatMessage.Workspace);
            var instructions = _stringLocalizer["DesignConversationNamePrompt", ConversationRecordConsts.MaxNameLength].Value;
            var aiAgent = chatClient
                .AsBuilder()
                .ConfigureOptions(options =>
                {
                    // 不受工具影响
                    options.Tools = [];
                })
                .BuildAIAgent(
                    instructions: instructions,
                    services: _serviceProvider);

            var agentRunRes = await aiAgent.RunAsync([
                new ChatMessage(ChatRole.System, instructions),
                new ChatMessage(ChatRole.User, chatMessage.Content)]);

            conversation.SetName(
                agentRunRes.Text.Length > ConversationRecordConsts.MaxNameLength
                ? agentRunRes.Text[..ConversationRecordConsts.MaxNameLength]
                : agentRunRes.Text);

            await _conversationRecordRepository.UpdateAsync(conversation);

            if (agentRunRes.Usage != null)
            {
                var tokenUsageRecord = new TokenUsageRecord(
                    _guidGenerator.Create(),
                    chatMessage.Id,
                    conversation.Id,
                    agentRunRes.Usage.InputTokenCount,
                    agentRunRes.Usage.OutputTokenCount,
                    agentRunRes.Usage.TotalTokenCount,
                    agentRunRes.Usage.CachedInputTokenCount,
                    agentRunRes.Usage.ReasoningTokenCount,
                    chatMessage.TenantId);

                await _tokenUsageRecordRepository.InsertAsync(tokenUsageRecord);
            }
        }
    }
}
