using Volo.Abp.Domain.Entities;

namespace LINGYUN.ApiGateway.Ocelot
{
    public class QoSOptions : Entity<int>
    {
        public virtual long? ItemId { get; private set; }

        public virtual long? ReRouteId { get; private set; }

        public virtual int? ExceptionsAllowedBeforeBreaking { get; private set; }

        public virtual int? DurationOfBreak { get; private set; }

        public virtual int? TimeoutValue { get; private set; }

        public virtual ReRoute ReRoute { get; private set; }

        public virtual GlobalConfiguration GlobalConfiguration { get; private set; }

        protected QoSOptions()
        {

        }

        public QoSOptions(int? exceptionBreaking, int? duration, int? timeout)
        {
            ApplyQosOptions(exceptionBreaking, duration, timeout);
        }

        public QoSOptions SetReRouteId(long rerouteId)
        {
            ReRouteId = rerouteId;
            return this;
        }

        public QoSOptions SetItemId(long itemId)
        {
            ItemId = itemId;
            return this;
        }


        public void ApplyQosOptions(int? exceptionBreaking, int? duration, int? timeout)
        {
            ExceptionsAllowedBeforeBreaking = exceptionBreaking;
            DurationOfBreak = duration;
            TimeoutValue = timeout;
        }
    }
}
