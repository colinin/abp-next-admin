using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.MessageService.Subscriptions
{
    public interface IMySubscriptionAppService : IApplicationService
    {
        /// <summary>
        /// 是否已订阅消息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<UserSubscriptionsResult> IsSubscribedAsync(SubscriptionsGetByNameDto input);
        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task SubscribeAsync(SubscriptionsGetByNameDto input);
        /// <summary>
        /// 退订消息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UnSubscribeAsync(SubscriptionsGetByNameDto input);
        /// <summary>
        /// 获取订阅列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<UserSubscreNotificationDto>> GetListAsync(SubscriptionsGetByPagedDto input);
        /// <summary>
        /// 获取所有订阅列表
        /// </summary>
        /// <returns></returns>
        Task<ListResultDto<UserSubscreNotificationDto>> GetAllListAsync();
    }
}
