using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.DynamicProxy;
using Volo.Abp.EventBus;
using Volo.Abp.Uow;

namespace LINGYUN.Abp.Domain.Entities.Events
{
    [Dependency(Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient, ReplaceServices = true)]
    [ExposeServices(typeof(IEntityChangeEventHelper), typeof(Volo.Abp.Domain.Entities.Events.EntityChangeEventHelper))]
    [Obsolete("the component will be removed when the abp framework is upgraded to 3.1.0")]
    public class EntityChangeEventHelper : Volo.Abp.Domain.Entities.Events.EntityChangeEventHelper
    {
        public EntityChangeEventHelper(
            IUnitOfWorkManager unitOfWorkManager, 
            IEntityToEtoMapper entityToEtoMapper, 
            IOptions<AbpDistributedEntityEventOptions> distributedEntityEventOptions) 
            : base(unitOfWorkManager, entityToEtoMapper, distributedEntityEventOptions)
        {
        }

        protected override async Task TriggerEventWithEntity(
            IEventBus eventPublisher, 
            Type genericEventType, 
            object entityOrEto, 
            object originalEntity, 
            bool triggerInCurrentUnitOfWork)
        {
            var entityType = ProxyHelper.UnProxy(entityOrEto).GetType();
            var eventType = genericEventType.MakeGenericType(entityType);
            var currentUow = UnitOfWorkManager.Current;

            if (triggerInCurrentUnitOfWork || currentUow == null)
            {
                await eventPublisher.PublishAsync(
                    eventType,
                    Activator.CreateInstance(eventType, entityOrEto)
                );

                return;
            }

            var eventList = GetEventList(currentUow);
            var isFirstEvent = !eventList.Any();

            eventList.AddUniqueEvent(eventPublisher, eventType, entityOrEto, originalEntity);

            /* Register to OnCompleted if this is the first item.
             * Other items will already be in the list once the UOW completes.
             */
            if (isFirstEvent)
            {
                currentUow.OnCompleted(
                    async () =>
                    {
                        foreach (var eventEntry in eventList)
                        {
                            try
                            {
                                // TODO: abp.io 3.1修复
                                await eventEntry.EventBus.PublishAsync(
                                    eventEntry.EventType,
                                    Activator.CreateInstance(eventEntry.EventType, eventEntry.EntityOrEto)
                                );
                            }
                            catch (Exception ex)
                            {
                                Logger.LogError(
                                    $"Caught an exception while publishing the event '{eventType.FullName}' for the entity '{entityOrEto}'");
                                Logger.LogException(ex);
                            }
                        }
                    }
                );
            }
        }

        private EntityChangeEventList GetEventList(IUnitOfWork currentUow)
        {
            return (EntityChangeEventList)currentUow.Items.GetOrAdd(
                "AbpEntityChangeEventList",
                () => new EntityChangeEventList()
            );
        }

        private class EntityChangeEventList : List<EntityChangeEventEntry>
        {
            public void AddUniqueEvent(IEventBus eventBus, Type eventType, object entityOrEto, object originalEntity)
            {
                var newEntry = new EntityChangeEventEntry(eventBus, eventType, entityOrEto, originalEntity);

                //Latest "same" event overrides the previous events.
                for (var i = 0; i < Count; i++)
                {
                    if (this[i].IsSameEvent(newEntry))
                    {
                        this[i] = newEntry;
                        return;
                    }
                }

                //If this is a "new" event, add to the end
                Add(newEntry);
            }
        }

        private class EntityChangeEventEntry
        {
            public IEventBus EventBus { get; }

            public Type EventType { get; }

            public object EntityOrEto { get; }

            public object OriginalEntity { get; }

            public EntityChangeEventEntry(IEventBus eventBus, Type eventType, object entityOrEto, object originalEntity)
            {
                EventType = eventType;
                EntityOrEto = entityOrEto;
                OriginalEntity = originalEntity;
                EventBus = eventBus;
            }

            public bool IsSameEvent(EntityChangeEventEntry otherEntry)
            {
                if (EventBus != otherEntry.EventBus || EventType != otherEntry.EventType)
                {
                    return false;
                }

                var originalEntityRef = OriginalEntity as IEntity;
                var otherOriginalEntityRef = otherEntry.OriginalEntity as IEntity;
                if (originalEntityRef == null || otherOriginalEntityRef == null)
                {
                    return false;
                }

                return EntityHelper.EntityEquals(originalEntityRef, otherOriginalEntityRef);
            }
        }
    }
}
