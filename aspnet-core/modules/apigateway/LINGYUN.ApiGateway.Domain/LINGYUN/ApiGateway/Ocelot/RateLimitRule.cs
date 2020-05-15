using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.ApiGateway.Ocelot
{
    public class RateLimitRule : Entity<int>
    {
        public virtual long? ReRouteId { get; private set; }
        public virtual long? DynamicReRouteId { get; private set; }
        /// <summary>
        /// 客户端白名单列表,多个以分号分隔
        /// </summary>
        public virtual string ClientWhitelist { get; private set; }
        /// <summary>
        /// 是否启用流量现值
        /// </summary>
        public virtual bool EnableRateLimiting { get; private set; }

        public virtual string Period { get; private set; }
        /// <summary>
        /// 速率极限周期
        /// </summary>
        public virtual double? PeriodTimespan { get; private set; }
        /// <summary>
        /// 客户端在定义的时间内可以发出的最大请求数
        /// </summary>
        public virtual long? Limit { get; private set; }
        public virtual ReRoute ReRoute { get; private set; }
        public virtual DynamicReRoute DynamicReRoute { get; private set; }
        protected RateLimitRule()
        {

        }
        public RateLimitRule(string period, double? timeSpan, long? limit)
        {
            SetPeriodTimespan(period, timeSpan, limit);
        }

        public void SetReRouteId(long rerouteId)
        {
            ReRouteId = rerouteId;
        }

        public void SetDynamicReRouteId(long rdynamicRerouteId)
        {
            DynamicReRouteId = rdynamicRerouteId;
        }

        public void ApplyRateLimit(bool enableRateLimiting)
        {
            EnableRateLimiting = enableRateLimiting;
        }

        public void SetClientWhileList(List<string> clientWhileList)
        {
            ClientWhitelist = clientWhileList.JoinAsString(",");
        }

        public void SetPeriodTimespan(string period, double? timeSpan, long? limit)
        {
            Period = period;
            PeriodTimespan = timeSpan;
            Limit = limit;
        }
    }
}
