using DotNetCore.CAP;
using DotNetCore.CAP.Internal;
using DotNetCore.CAP.Transport;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Threading;

namespace LINGYUN.Abp.EventBus.CAP
{
    internal class CustomDistributedEventSubscriber : ICustomDistributedEventSubscriber, ISingletonDependency
    {
        protected CapOptions CapOptions { get; }
        protected IConsumerClientFactory ConsumerClientFactory { get; }

        protected ConcurrentDictionary<Type, List<IEventHandlerFactory>> HandlerFactories { get; }
        protected ConcurrentDictionary<string, CancellationTokenSource> EventStopingTokens { get; }
        public CustomDistributedEventSubscriber(
            IOptions<CapOptions> capOptions,
            IConsumerClientFactory consumerClientFactory)
        {
            CapOptions = capOptions.Value;
            ConsumerClientFactory = consumerClientFactory;

            HandlerFactories = new ConcurrentDictionary<Type, List<IEventHandlerFactory>>();
            EventStopingTokens = new ConcurrentDictionary<string, CancellationTokenSource>();
        }

        public void Subscribe(Type eventType, IEventHandlerFactory factory)
        {
            GetOrCreateHandlerFactories(eventType)
                .Locking(factories =>
                {
                    if (!factories.Contains(factory))
                    {
                        factories.Add(factory);
                        // TODO 客户端订阅
                    }
                });
        }

        public void UnSubscribe(Type eventType, IEventHandlerFactory factory)
        {
            GetOrCreateHandlerFactories(eventType)
                .Locking(factories =>
                {
                    if (factories.Contains(factory))
                    {
                        factories.Remove(factory);
                        // TODO 客户端退订
                    }
                });
        }

        private List<IEventHandlerFactory> GetOrCreateHandlerFactories(Type eventType)
        {
            return HandlerFactories.GetOrAdd(
                eventType,
                type =>
                {
                    var eventName = EventNameAttribute.GetNameOrDefault(type);
                    EventStopingTokens[eventName] = new CancellationTokenSource();
                    return new List<IEventHandlerFactory>();
                }
            );
        }

        private IEnumerable<ConsumerExecutorDescriptor> GetHandlerDescription(Type eventType, Type typeInfo)
        {
            var serviceTypeInfo = typeof(IDistributedEventHandler<>)
                .MakeGenericType(eventType);
            var method = typeInfo
                .GetMethod(
                    nameof(IDistributedEventHandler<object>.HandleEventAsync),
                    new[] { eventType }
                );
            var eventName = EventNameAttribute.GetNameOrDefault(eventType);
            var topicAttr = method.GetCustomAttributes<TopicAttribute>(true);
            var topicAttributes = topicAttr.ToList();

            topicAttributes.Add(new CapSubscribeAttribute(eventName));

            foreach (var attr in topicAttributes)
            {
                SetSubscribeAttribute(attr);

                var parameters = method.GetParameters()
                    .Select(parameter => new ParameterDescriptor
                    {
                        Name = parameter.Name,
                        ParameterType = parameter.ParameterType,
                        IsFromCap = parameter.GetCustomAttributes(typeof(FromCapAttribute)).Any()
                    }).ToList();

                yield return InitDescriptor(attr, method, typeInfo.GetTypeInfo(), serviceTypeInfo.GetTypeInfo(), parameters);
            }
        }

        private void SetSubscribeAttribute(TopicAttribute attribute)
        {
            if (attribute.Group == null)
            {
                attribute.Group = CapOptions.DefaultGroupName + "." + CapOptions.Version;
            }
            else
            {
                attribute.Group = attribute.Group + "." + CapOptions.Version;
            }
        }

        private ConsumerExecutorDescriptor InitDescriptor(
            TopicAttribute attr,
            MethodInfo methodInfo,
            TypeInfo implType,
            TypeInfo serviceTypeInfo,
            IList<ParameterDescriptor> parameters)
        {
            var descriptor = new ConsumerExecutorDescriptor
            {
                Attribute = attr,
                MethodInfo = methodInfo,
                ImplTypeInfo = implType,
                ServiceTypeInfo = serviceTypeInfo,
                Parameters = parameters
            };

            return descriptor;
        }
    }
}
