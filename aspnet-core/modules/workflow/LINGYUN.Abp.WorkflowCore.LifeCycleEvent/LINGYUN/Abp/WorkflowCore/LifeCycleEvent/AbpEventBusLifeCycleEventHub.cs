using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Volo.Abp.EventBus.Distributed;
using WorkflowCore.Interface;
using EventData = WorkflowCore.Models.LifeCycleEvents.LifeCycleEvent;

namespace LINGYUN.Abp.WorkflowCore.LifeCycleEvent
{
    public class AbpEventBusLifeCycleEventHub : ILifeCycleEventHub
    {
        private IDisposable _subscriber;

        private readonly IDistributedEventBus _eventBus;
        private readonly ILoggerFactory _loggerFactory;

        public AbpEventBusLifeCycleEventHub(
            ILoggerFactory loggerFactory,
            IDistributedEventBus distributedEventBus)
        {
            _loggerFactory = loggerFactory;
            _eventBus = distributedEventBus;
        }

        public async Task PublishNotification(EventData evt)
        {
            await _eventBus.PublishAsync(evt);
        }

        public Task Start()
        {
            _subscriber = _eventBus.Subscribe(new LifeCycleEventHandler(
                _loggerFactory.CreateLogger<LifeCycleEventHandler>()));

            return Task.CompletedTask;
        }

        public Task Stop()
        {
            // TODO
            _subscriber?.Dispose();

            return Task.CompletedTask;
        }

        public void Subscribe(Action<EventData> action)
        {
            LifeCycleEventHandler.Subscribers.Add(action);
        }
    }
}
