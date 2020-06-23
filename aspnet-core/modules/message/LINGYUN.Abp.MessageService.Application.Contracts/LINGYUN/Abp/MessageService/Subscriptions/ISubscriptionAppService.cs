using LINGYUN.Abp.Notifications;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.MessageService.Subscriptions
{
    public interface ISubscriptionAppService : IApplicationService
    {
        /// <summary>
        /// 是否已订阅消息
        /// </summary>
        /// <param name="subscriptionsGetByName"></param>
        /// <returns></returns>
        Task<bool> IsSubscribedAsync(SubscriptionsGetByNameDto subscriptionsGetByName);
        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="subscriptionsGetByName"></param>
        /// <returns></returns>
        Task SubscribeAsync(SubscriptionsGetByNameDto subscriptionsGetByName);
        /// <summary>
        /// 退订消息
        /// </summary>
        /// <param name="subscriptionsGetByName"></param>
        /// <returns></returns>
        Task UnSubscribeAsync(SubscriptionsGetByNameDto subscriptionsGetByName);
        /// <summary>
        /// 获取订阅列表
        /// </summary>
        /// <param name="subscriptionsGetByPaged"></param>
        /// <returns></returns>
        Task<PagedResultDto<NotificationSubscriptionInfo>> GetSubscribedsAsync(SubscriptionsGetByPagedDto subscriptionsGetByPaged);
    }
}
