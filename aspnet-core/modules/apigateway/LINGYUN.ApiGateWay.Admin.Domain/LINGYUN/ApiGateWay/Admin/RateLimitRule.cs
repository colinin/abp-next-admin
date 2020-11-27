using LINGYUN.ApiGateWay.Admin.Security;
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.ApiGateWay.Admin
{
    public class RateLimitRule : Entity<Guid>
    {
        /// <summary>
        /// 客户端白名单列表,多个以分号分隔
        /// </summary>
        public virtual ICollection<RouteClientWhite> ClientWhiteList { get; private set; }
        /// <summary>
        /// 是否启用流量现值
        /// </summary>
        public virtual bool EnableRateLimiting { get; private set; }
        /// <summary>
        /// 限速时段
        /// </summary>
        public virtual string Period { get; private set; }
        /// <summary>
        /// 速率极限周期
        /// </summary>
        public virtual double? PeriodTimespan { get; private set; }
        /// <summary>
        /// 客户端在定义的时间内可以发出的最大请求数
        /// </summary>
        public virtual long? Limit { get; private set; }

        protected RateLimitRule()
        {
            ClientWhiteList = new List<RouteClientWhite>();
        }

        public RateLimitRule(Guid id, string period = "", double? periodTimespan = null, 
            long? limit = null, bool enabled = true)
        {
            Id = id;
            ChangeRateLimitState(enabled);
            ApplyPolicy(period, periodTimespan, limit);
        }

        public void ChangeRateLimitState(bool enabled = true)
        {
            EnableRateLimiting = enabled;
        }

        public void ApplyPolicy(string period = "", double? periodTimespan = null,
            long? limit = null)
        {
            Period = period;
            PeriodTimespan = periodTimespan;
            Limit = limit;
        }
    }
}
