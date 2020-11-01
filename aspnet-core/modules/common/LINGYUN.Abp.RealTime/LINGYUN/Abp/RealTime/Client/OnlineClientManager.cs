using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.RealTime.Client
{
    public class OnlineClientManager : IOnlineClientManager, ISingletonDependency
    {
        public event EventHandler<OnlineClientEventArgs> ClientConnected;
        public event EventHandler<OnlineClientEventArgs> ClientDisconnected;

        public event EventHandler<OnlineUserEventArgs> UserConnected;
        public event EventHandler<OnlineUserEventArgs> UserDisconnected;

        protected IOnlineClientStore Store { get; }

        protected readonly object SyncObj = new object();

        public OnlineClientManager(IOnlineClientStore store)
        {
            Store = store;
        }

        public virtual void Add(IOnlineClient client)
        {
            lock (SyncObj)
            {
                var userWasAlreadyOnline = false;
                var context = client.ToClientContextOrNull();

                if (context != null)
                {
                    userWasAlreadyOnline = this.IsOnline(context);
                }

                Store.Add(client);

                ClientConnected?.Invoke(this, new OnlineClientEventArgs(client));

                if (context != null && !userWasAlreadyOnline)
                {
                    UserConnected?.Invoke(this, new OnlineUserEventArgs(context, client));
                }
            }
        }

        public virtual bool Remove(string connectionId)
        {
            lock (SyncObj)
            {
                var result = Store.TryRemove(connectionId, out IOnlineClient client);
                if (result)
                {
                    if (UserDisconnected != null)
                    {
                        var context = client.ToClientContextOrNull();

                        if (context != null && !this.IsOnline(context))
                        {
                            UserDisconnected.Invoke(this, new OnlineUserEventArgs(context, client));
                        }
                    }

                    ClientDisconnected?.Invoke(this, new OnlineClientEventArgs(client));
                }

                return result;
            }
        }

        public virtual IOnlineClient GetByConnectionIdOrNull(string connectionId)
        {
            lock (SyncObj)
            {
                if (Store.TryGet(connectionId, out IOnlineClient client))
                {
                    return client;
                }
                else
                {
                    return null;
                }
            }
        }

        public virtual IReadOnlyList<IOnlineClient> GetAllClients()
        {
            return Store.GetAll();
        }

        public virtual IReadOnlyList<IOnlineClient> GetAllClients(Expression<Func<IOnlineClient, bool>> predicate)
        {
            return Store.GetAll(predicate);
        }

        [NotNull]
        public virtual IReadOnlyList<IOnlineClient> GetAllByContext([NotNull] OnlineClientContext context)
        {
            Check.NotNull(context, nameof(context));

            return GetAllClients()
                 .Where(c => c.TenantId == context.TenantId && c.UserId.Equals(context.UserId))
                 .ToImmutableList();
        }
    }
}
