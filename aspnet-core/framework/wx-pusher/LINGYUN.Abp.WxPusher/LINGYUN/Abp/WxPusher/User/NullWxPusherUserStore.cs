using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WxPusher.User;

public sealed class NullWxPusherUserStore : IWxPusherUserStore
{
    public readonly static IWxPusherUserStore Instance = new NullWxPusherUserStore();

    private NullWxPusherUserStore()
    {
    }

    public Task<List<int>> GetSubscribeTopicsAsync(
        IEnumerable<Guid> userIds,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new List<int>());
    }

    public Task<List<string>> GetBindUidsAsync(
        IEnumerable<Guid> userIds,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new List<string>());
    }
}
