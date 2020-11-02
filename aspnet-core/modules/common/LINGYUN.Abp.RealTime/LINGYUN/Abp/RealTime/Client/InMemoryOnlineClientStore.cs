using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.RealTime.Client
{
    public class InMemoryOnlineClientStore : IOnlineClientStore, ISingletonDependency
    {
        protected ConcurrentDictionary<string, IOnlineClient> Clients { get; }

        public InMemoryOnlineClientStore()
        {
            Clients = new ConcurrentDictionary<string, IOnlineClient>();
        }

        public void Add(IOnlineClient client)
        {
            Clients.AddOrUpdate(client.ConnectionId, client, (s, o) => client);
        }

        public bool Remove(string connectionId)
        {
            return TryRemove(connectionId, out _);
        }

        public bool TryRemove(string connectionId, out IOnlineClient client)
        {
            return Clients.TryRemove(connectionId, out client);
        }

        public bool TryGet(string connectionId, out IOnlineClient client)
        {
            return Clients.TryGetValue(connectionId, out client);
        }

        public bool Contains(string connectionId)
        {
            return Clients.ContainsKey(connectionId);
        }

        public IReadOnlyList<IOnlineClient> GetAll()
        {
            return Clients.Values.ToImmutableList();
        }

        public IReadOnlyList<IOnlineClient> GetAll(Expression<Func<IOnlineClient, bool>> predicate)
        {
            return Clients.Values
                .Where(predicate.Compile())
                .ToImmutableList();
        }
    }
}
