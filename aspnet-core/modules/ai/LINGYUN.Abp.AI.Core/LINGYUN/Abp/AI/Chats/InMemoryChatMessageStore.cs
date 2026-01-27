using LINGYUN.Abp.AI.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.AI.Chats;

[Dependency(ServiceLifetime.Singleton, TryRegister = true)]
public class InMemoryChatMessageStore : IChatMessageStore
{
    private static readonly ConcurrentDictionary<Guid, List<ChatMessage>> _userMessageCache = new ConcurrentDictionary<Guid, List<ChatMessage>>();

    public Task<IEnumerable<ChatMessage>> GetHistoryMessagesAsync(Guid conversationId)
    {
        var messages = new List<ChatMessage>();

        foreach (var userMessages in _userMessageCache.Values)
        {
            messages.AddRange(userMessages.Where(x => x.ConversationId == conversationId));
        }

        return Task.FromResult<IEnumerable<ChatMessage>>(
            messages
            .OrderByDescending(x => x.CreatedAt)
            .Take(5)
            .OrderBy(x => x.CreatedAt));
    }

    public Task<Guid> SaveMessageAsync(ChatMessage message)
    {
        var messageId = message.Id;
        if (!messageId.HasValue)
        {
            messageId = Guid.NewGuid();
            message.WithMessageId(messageId.Value);
        }
        if (_userMessageCache.ContainsKey(messageId.Value))
        {
            _userMessageCache[messageId.Value].Add(message);
        }
        else
        {
            _userMessageCache[messageId.Value] = new List<ChatMessage>() { message };
        }

        return Task.FromResult(messageId.Value);
    }
}
