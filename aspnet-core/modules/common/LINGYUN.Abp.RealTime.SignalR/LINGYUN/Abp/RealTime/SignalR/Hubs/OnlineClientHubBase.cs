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
        protected IWebClientInfoProvider WebClientInfoProvider => LazyServiceProvider.LazyGetRequiredService<IWebClientInfoProvider>();
        protected IOnlineClientManager OnlineClientManager => LazyServiceProvider.LazyGetRequiredService<IOnlineClientManager>();

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();

            IOnlineClient onlineClient = CreateClientForCurrentConnection();
            OnlineClientManager.Add(onlineClient);

            await OnConnectedAsync(onlineClient);
        }

        public async Task OnConnectedAsync(IOnlineClient client)
        {
            Logger.LogDebug("A client is connected: " + client.ToString());

            // 角色添加进组
            foreach (var role in client.Roles)
            {
                await Groups.AddToGroupAsync(client.ConnectionId, role);
            }

            await OnClientConnectedAsync(client);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            // 从通讯组移除
            var onlineClient = OnlineClientManager.GetByConnectionIdOrNull(Context.ConnectionId);
            await OnDisconnectedAsync(onlineClient);

            await base.OnDisconnectedAsync(exception);
        }

        public async Task OnDisconnectedAsync(IOnlineClient client)
        {
            if (client != null)
            {
                try
                {
                    // 角色添加进组
                    foreach (var role in client.Roles)
                    {
                        await Groups.RemoveFromGroupAsync(client.ConnectionId, role);
                    }

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

        protected virtual Task OnClientConnectedAsync(IOnlineClient client)
        {
            return Task.CompletedTask;   
        }

        protected virtual Task OnClientDisconnectedAsync(IOnlineClient client)
        {
            return Task.CompletedTask;
        }
    }
}
