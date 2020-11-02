using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace LINGYUN.Abp.RealTime.Client
{
    public interface IOnlineClientManager
    {
        event EventHandler<OnlineClientEventArgs> ClientConnected;

        event EventHandler<OnlineClientEventArgs> ClientDisconnected;

        event EventHandler<OnlineUserEventArgs> UserConnected;

        event EventHandler<OnlineUserEventArgs> UserDisconnected;

        void Add(IOnlineClient client);

        bool Remove(string connectionId);

        IOnlineClient GetByConnectionIdOrNull(string connectionId);

        IReadOnlyList<IOnlineClient> GetAllClients();

        IReadOnlyList<IOnlineClient> GetAllClients(Expression<Func<IOnlineClient, bool>> predicate);

        IReadOnlyList<IOnlineClient> GetAllByContext([NotNull] OnlineClientContext context);
    }
}
