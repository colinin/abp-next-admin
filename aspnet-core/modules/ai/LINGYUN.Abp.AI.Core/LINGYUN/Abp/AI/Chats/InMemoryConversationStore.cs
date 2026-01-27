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
public class InMemoryConversationStore : IConversationStore
{
    private static readonly ConcurrentDictionary<Guid, Conversation> _conversationCache = new ConcurrentDictionary<Guid, Conversation>();
    public Task SaveAsync(Conversation conversation)
    {
        if (_conversationCache.ContainsKey(conversation.Id))
        {
            conversation.ExpiredAt = DateTime.Now.AddHours(2);
            _conversationCache[conversation.Id] = conversation;
        }
        else
        {
            _conversationCache.TryAdd(conversation.Id, conversation);
        }

        return Task.CompletedTask;
    }

    public Task<Conversation?> FindAsync(Guid conversationId)
    {
        _conversationCache.TryGetValue(conversationId, out var conversation);
        return Task.FromResult<Conversation?>(conversation);
    }

    public Task CleanupAsync()
    {
        // Configure it...
        var expiredTime = DateTime.Now.AddHours(-2);
        var expiredConversationIds = _conversationCache.Values
            .Where(x => x.UpdateAt <= expiredTime)
            .Select(x => x.Id);
        _conversationCache.RemoveAll(x => expiredConversationIds.Contains(x.Key));

        return Task.CompletedTask;
    }
}
