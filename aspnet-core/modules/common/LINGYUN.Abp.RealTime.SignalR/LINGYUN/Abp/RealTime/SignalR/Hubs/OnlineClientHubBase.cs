using LINGYUN.Abp.RealTime.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.SignalR;
using Volo.Abp.AspNetCore.WebClientInfo;

namespace LINGYUN.Abp.RealTime.SignalR
{
    public abstract class OnlineClientHubBase : AbpHub, IClient
    {
        private IWebClientInfoProvider _webClientInfoProvider;
        protected IWebClientInfoProvider WebClientInfoProvider => LazyGetRequiredService(ref _webClientInfoProvider);

        private IOnlineClientManager _onlineClientManager;
        protected IOnlineClientManager OnlineClientManager => LazyGetRequiredService(ref _onlineClientManager);

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();

            IOnlineClient onlineClient = CreateClientForCurrentConnection();
            await OnConnectedAsync(onlineClient);
        }

        public virtual async Task OnConnectedAsync(IOnlineClient client)
        {
            Logger.LogDebug("A client is connected: " + client.ToString());
            OnlineClientManager.Add(client);
            await OnClientConnectedAsync(client);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            // 从通讯组移除
            var onlineClient = OnlineClientManager.GetByConnectionIdOrNull(Context.ConnectionId);
            await OnDisconnectedAsync(onlineClient);

            await base.OnDisconnectedAsync(exception);
        }

        public virtual async Task OnDisconnectedAsync(IOnlineClient client)
        {
            if (client != null)
            {
                try
                {
                    Logger.LogDebug("A client is disconnected: " + client);
                    // 移除在线客户端
                    OnlineClientManager.Remove(Context.ConnectionId);
                    await OnClientDisconnectedAsync(client);
                }
                catch (Exception ex)
                {
                    Logger.LogWarning(ex.ToString(), ex);
                }
            }
        }

        protected virtual IOnlineClient CreateClientForCurrentConnection()
        {
            return new OnlineClient(
                Context.ConnectionId,
                WebClientInfoProvider.ClientIpAddress,
                CurrentTenant.Id, 
                CurrentUser.Id)
            {
                ConnectTime = Clock.Now,
                UserName = CurrentUser.UserName,
                UserAccount = CurrentUser.UserName,
                Roles = CurrentUser.Roles ?? new string[0],
                Properties = Context.Items
            };
        }

        protected virtual async Task OnClientConnectedAsync(IOnlineClient client)
        {
            // 角色添加进组
            foreach (var role in client.Roles)
            {
                await Groups.AddToGroupAsync(client.ConnectionId, role);
            }
        }

        protected virtual async Task OnClientDisconnectedAsync(IOnlineClient client)
        {
            // 角色添加进组
            foreach (var role in client.Roles)
            {
                await Groups.RemoveFromGroupAsync(client.ConnectionId, role);
            }
        }
    }
}
