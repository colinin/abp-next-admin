using JetBrains.Annotations;
using System;
using System.Collections.Generic;

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

        IReadOnlyList<IOnlineClient> GetAllByContext([NotNull] OnlineClientContext context);
    }
}
