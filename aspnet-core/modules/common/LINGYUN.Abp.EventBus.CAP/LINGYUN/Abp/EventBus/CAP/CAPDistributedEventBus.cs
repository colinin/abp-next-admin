using DotNetCore.CAP;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Threading;

namespace LINGYUN.Abp.EventBus.CAP
{
    /// <summary>
    /// CAP分布式事件总线
    /// </summary>
    [Dependency(ServiceLifetime.Singleton, ReplaceServices = true)]
    [ExposeServices(typeof(IDistributedEventBus), typeof(CAPDistributedEventBus))]
    public class CAPDistributedEventBus : EventBusBase, IDistributedEventBus
    {
        /// <summary>
        /// Abp分布式事件总线配置
        /// </summary>
        protected AbpDistributedEventBusOptions AbpDistributedEventBusOptions { get; }
        /// <summary>
        /// CAP消息发布接口
        /// </summary>
        protected readonly ICapPublisher CapPublisher;

        /// <summary>
        /// 本地事件处理器工厂对象集合
        /// </summary>
        protected ConcurrentDictionary<Type, List<IEventHandlerFactory>> HandlerFactories { get; }
        /// <summary>
        /// 本地事件集合
        /// </summary>
        protected ConcurrentDictionary<string, Type> EventTypes { get; }
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="serviceScopeFactory"></param>
        /// <param name="distributedEventBusOptions"></param>
        /// <param name="capPublisher"></param>
        public CAPDistributedEventBus(IServiceScopeFactory serviceScopeFactory,
            IOptions<AbpDistributedEventBusOptions> distributedEventBusOptions,
            ICapPublisher capPublisher) : base(serviceScopeFactory)
        {
            CapPublisher = capPublisher;
            AbpDistributedEventBusOptions = distributedEventBusOptions.Value;
            HandlerFactories = new ConcurrentDictionary<Type, List<IEventHandlerFactory>>();
            EventTypes = new ConcurrentDictionary<string, Type>();
        }
        /// <summary>
        /// 订阅事件
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        public override IDisposable Subscribe(Type eventType, IEventHandlerFactory factory)
        {
            //This is handled by CAP ConsumerServiceSelector
            throw new NotImplementedException();
        }
        /// <summary>
        /// 退订事件
        /// </summary>
        /// <typeparam name="TEvent">事件类型</typeparam>
        /// <param name="action"></param>
        public override void Unsubscribe<TEvent>(Func<TEvent, Task> action)
        {
            Check.NotNull(action, nameof(action));

            GetOrCreateHandlerFactories(typeof(TEvent))
                .Locking(factories =>
                {
                    factories.RemoveAll(
                        factory =>
                        {
                            if (!(factory is SingleInstanceHandlerFactory singleInstanceFactory))
                            {
                                return false;
                            }

                            if (!(singleInstanceFactory.HandlerInstance is ActionEventHandler<TEvent> actionHandler))
                            {
                                return false;
                            }

                            return actionHandler.Action == action;
                        });
                });
        }
        /// <summary>
        /// 退订事件
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="handler">事件处理器</param>
        public override void Unsubscribe(Type eventType, IEventHandler handler)
        {
            GetOrCreateHandlerFactories(eventType)
                .Locking(factories =>
                {
                    factories.RemoveAll(
                        factory =>
                            factory is SingleInstanceHandlerFactory &&
                            (factory as SingleInstanceHandlerFactory).HandlerInstance == handler
                    );
                });
        }
        /// <summary>
        /// 退订事件
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="factory">事件处理器工厂</param>
        public override void Unsubscribe(Type eventType, IEventHandlerFactory factory)
        {
            GetOrCreateHandlerFactories(eventType).Locking(factories => factories.Remove(factory));
        }
        /// <summary>
        /// 退订所有事件
        /// </summary>
        /// <param name="eventType">事件类型</param>
        public override void UnsubscribeAll(Type eventType)
        {
            GetOrCreateHandlerFactories(eventType).Locking(factories => factories.Clear());
        }
        /// <summary>
        /// 订阅事件
        /// </summary>
        /// <typeparam name="TEvent">事件类型</typeparam>
        /// <param name="handler">事件处理器</param>
        /// <returns></returns>
        public IDisposable Subscribe<TEvent>(IDistributedEventHandler<TEvent> handler) where TEvent : class
        {
            return Subscribe(typeof(TEvent), handler);
        }

        /// <summary>
        /// 发布事件
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="eventData">事件数据对象</param>
        /// <returns></returns>
        public override async Task PublishAsync(Type eventType, object eventData)
        {
            var eventName = EventNameAttribute.GetNameOrDefault(eventType);
            await CapPublisher.PublishAsync(eventName, eventData);
        }
        /// <summary>
        /// 获取事件处理器工厂列表
        /// </summary>
        /// <param name="eventType"></param>
        /// <returns></returns>
        protected override IEnumerable<EventTypeWithEventHandlerFactories> GetHandlerFactories(Type eventType)
        {
            var handlerFactoryList = new List<EventTypeWithEventHandlerFactories>();

            foreach (var handlerFactory in HandlerFactories.Where(hf => ShouldTriggerEventForHandler(eventType, hf.Key)))
            {
                handlerFactoryList.Add(new EventTypeWithEventHandlerFactories(handlerFactory.Key, handlerFactory.Value));
            }

            return handlerFactoryList.ToArray();
        }

        private List<IEventHandlerFactory> GetOrCreateHandlerFactories(Type eventType)
        {
            return HandlerFactories.GetOrAdd(
                eventType,
                type =>
                {
                    var eventName = EventNameAttribute.GetNameOrDefault(type);
                    EventTypes[eventName] = type;
                    return new List<IEventHandlerFactory>();
                }
            );
        }

        private static bool ShouldTriggerEventForHandler(Type targetEventType, Type handlerEventType)
        {
            //Should trigger same type
            if (handlerEventType == targetEventType)
            {
                return true;
            }

            //TODO: Support inheritance? But it does not support on subscription to RabbitMq!
            //Should trigger for inherited types
            if (handlerEventType.IsAssignableFrom(targetEventType))
            {
                return true;
            }

            return false;
        }
    }
}
