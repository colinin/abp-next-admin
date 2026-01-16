using LINGYUN.Abp.AI.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.AI.Messages;

[Dependency(ServiceLifetime.Singleton, TryRegister = true)]
public class InMemoryUserMessageStore : IUserMessageStore
{
    private static readonly ConcurrentDictionary<string, List<UserMessage>> _userMessageCache = new ConcurrentDictionary<string, List<UserMessage>>();

    public Task<IEnumerable<UserMessage>> GetHistoryMessagesAsync(string conversationId)
    {
        if (_userMessageCache.TryGetValue(conversationId, out var messages))
        {
            return Task.FromResult(messages.Take(5));
        }

        return Task.FromResult<IEnumerable<UserMessage>>(Array.Empty<UserMessage>());
    }

    public Task<string> SaveMessageAsync(UserMessage message)
    {
        var messageId = message.Id;
        if (messageId.IsNullOrWhiteSpace())
        {
            messageId = Guid.NewGuid().ToString();
            message.WithMessageId(messageId);
        }
        if (_userMessageCache.ContainsKey(messageId))
        {
            _userMessageCache[messageId].Add(message);
        }
        else
        {
            _userMessageCache[messageId] = new List<UserMessage>() { message };
        }

        return Task.FromResult(messageId);
    }
}
