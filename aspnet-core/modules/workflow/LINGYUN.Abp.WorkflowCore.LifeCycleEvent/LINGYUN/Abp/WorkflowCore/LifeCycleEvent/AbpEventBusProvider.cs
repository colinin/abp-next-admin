using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.EventBus.Distributed;
using WorkflowCore.Interface;
using EventData = WorkflowCore.Models.LifeCycleEvents.LifeCycleEvent;

namespace LINGYUN.Abp.WorkflowCore.LifeCycleEvent
{
    public class AbpEventBusProvider : ILifeCycleEventHub
    {
        private bool _started = false;
        private Queue<Action<EventData>> _deferredSubscribers = new Queue<Action<EventData>>();

        private readonly IDistributedEventBus _eventBus;

        private readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings 
        { 
            TypeNameHandling = TypeNameHandling.All
        };

        public AbpEventBusProvider(
            IDistributedEventBus distributedEventBus)
        {
            _eventBus = distributedEventBus;
        }

        public async Task PublishNotification(EventData evt)
        {
            var data = JsonConvert.SerializeObject(evt, _serializerSettings);
            var wrapEvent = new LifeCycleEventWrap(data);
            await _eventBus.PublishAsync(wrapEvent);
        }

        public Task Start()
        {
            _started = true;
            while (_deferredSubscribers.Count > 0)
            {
                var action = _deferredSubscribers.Dequeue();
                _eventBus.Subscribe<LifeCycleEventWrap>((data) =>
                {
                    var unWrapData = JsonConvert.DeserializeObject(data.Data, _serializerSettings);
                    action(unWrapData as EventData);

                    return Task.CompletedTask;
                });
            }

            return Task.CompletedTask;
        }

        public Task Stop()
        {
            // TODO
            _started = false;

            return Task.CompletedTask;
        }

        public void Subscribe(Action<EventData> action)
        {
            if (_started)
            {
                _eventBus.Subscribe<LifeCycleEventWrap>((data) =>
                {
                    var unWrapData = JsonConvert.DeserializeObject(data.Data, _serializerSettings);
                    action(unWrapData as EventData);

                    return Task.CompletedTask;
                });
            }
            else
            {
                _deferredSubscribers.Enqueue(action);
            }
        }
    }
}
