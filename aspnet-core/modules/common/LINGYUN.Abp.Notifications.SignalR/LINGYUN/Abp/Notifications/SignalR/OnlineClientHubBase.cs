using LINGYUN.Abp.RealTime.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.SignalR;
using Volo.Abp.Security.Claims;

namespace LINGYUN.Abp.Notifications.SignalR
{
    public abstract class OnlineClientHubBase : AbpHub
    {
        private ICurrentPrincipalAccessor _currentPrincipalAccessor;
        protected ICurrentPrincipalAccessor CurrentPrincipalAccessor => LazyGetRequiredService(ref _currentPrincipalAccessor);

        private IOnlineClientManager _onlineClientManager;
        protected IOnlineClientManager OnlineClientManager => LazyGetRequiredService(ref _onlineClientManager);

        private IHttpContextAccessor _httpContextAccessor;
        protected IHttpContextAccessor HttpContextAccessor => LazyGetRequiredService(ref _httpContextAccessor);

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            IOnlineClient onlineClient = CreateClientForCurrentConnection();
            Logger.LogDebug("A client is connected: " + onlineClient.ToString());
            OnlineClientManager.Add(onlineClient);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
            Logger.LogDebug("A client is disconnected: " + Context.ConnectionId);
            try
            {
                // 从通讯组移除
                var onlineClient = OnlineClientManager.GetByConnectionIdOrNull(Context.ConnectionId);
                if(onlineClient != null)
                {
                    // 移除在线客户端
                    OnlineClientManager.Remove(Context.ConnectionId);
                }
            }
            catch (Exception ex)
            {
                Logger.LogWarning(ex.ToString(), ex);
            }
        }

        protected virtual IOnlineClient CreateClientForCurrentConnection()
        {
            // abp框架没有处理,需要切换一下用户身份令牌.否则无法获取用户信息
            using (CurrentPrincipalAccessor.Change(Context.User))
            {
                return new OnlineClient(Context.ConnectionId, GetClientIpAddress(),
                    CurrentTenant.Id, CurrentUser.Id)
                {
                    ConnectTime = Clock.Now,
                    UserName = CurrentUser.UserName
                };
            }
        }

        protected virtual string GetClientIpAddress()
        {
            try
            {
                return HttpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, LogLevel.Warning);
                return null;
            }
        }
    }
}
