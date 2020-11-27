using System;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.ApiGateWay.Admin
{
    public class QoS : Entity<Guid>
    {
        public virtual int? ExceptionsAllowedBeforeBreaking { get; private set; }

        public virtual int? DurationOfBreak { get; private set; }

        public virtual int? TimeoutValue { get; private set; }

        protected QoS()
        {

        }

        public QoS(Guid id, int? exceptionsAllowdBeforeBreaking = null, int? durationOfBreak = null, int? timeOut = null)
        {
            Id = id;
            ApplyPolicy(exceptionsAllowdBeforeBreaking, durationOfBreak, timeOut);
        }

        public void ApplyPolicy(int? exceptionsAllowdBeforeBreaking = null, int? durationOfBreak = null, int? timeOut = null)
        {
            ExceptionsAllowedBeforeBreaking = exceptionsAllowdBeforeBreaking;
            DurationOfBreak = durationOfBreak;
            TimeoutValue = timeOut;
        }
    }
}
