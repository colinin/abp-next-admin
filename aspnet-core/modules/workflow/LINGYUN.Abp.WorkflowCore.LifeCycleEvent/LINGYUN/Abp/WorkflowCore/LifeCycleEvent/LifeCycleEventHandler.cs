using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.EventBus.Distributed;
using EventData = WorkflowCore.Models.LifeCycleEvents.LifeCycleEvent;

namespace LINGYUN.Abp.WorkflowCore.LifeCycleEvent
{
    public class LifeCycleEventHandler : IDistributedEventHandler<EventData>
    {
        private readonly ILogger<LifeCycleEventHandler> _logger;

        internal static readonly ICollection<Action<EventData>> Subscribers = new HashSet<Action<EventData>>();

        public LifeCycleEventHandler(
            ILogger<LifeCycleEventHandler> logger)
        {
            _logger = logger;
        }

        public Task HandleEventAsync(EventData eventData)
        {
            NotifySubscribers(eventData);

            return Task.CompletedTask;
        }

        private void NotifySubscribers(EventData evt)
        {
            foreach (var subscriber in Subscribers)
            {
                try
                {
                    subscriber(evt);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(
                        default, ex, $"Error on event subscriber: {ex.Message}");
                }
            }
        }
    }
}
