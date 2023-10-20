using System;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.EventBus.CAP
{
    /// <summary>
    /// 自定义事件订阅者
    /// </summary>
    public  interface ICustomDistributedEventSubscriber
    {
        /// <summary>
        /// 订阅事件
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="factory"></param>
        void Subscribe(Type eventType, IEventHandlerFactory factory);
        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="factory"></param>
        void UnSubscribe(Type eventType, IEventHandlerFactory factory);
    }
}
