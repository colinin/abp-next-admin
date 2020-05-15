using Volo.Abp.Domain.Entities;

namespace LINGYUN.ApiGateway.Ocelot
{
    public class RateLimitOptions : Entity<int>
    {
        /// <summary>
        /// 所属配置项主键
        /// </summary>
        public virtual long ItemId { get; protected set; }
        /// <summary>
        /// 客户端标头
        /// </summary>
        public virtual string ClientIdHeader { get; set; }
        /// <summary>
        /// 过载错误消息
        /// </summary>
        public virtual string QuotaExceededMessage { get; set; }
        /// <summary>
        /// 限速计数器前缀
        /// </summary>
        public virtual string RateLimitCounterPrefix { get; set; }
        /// <summary>
        /// 禁用限速标头
        /// </summary>
        public virtual bool DisableRateLimitHeaders { get; set; }
        /// <summary>
        /// HTTP状态码
        /// </summary>
        public virtual int? HttpStatusCode { get; set; }
        public virtual GlobalConfiguration GlobalConfiguration { get; private set; }
        protected RateLimitOptions()
        {

        }

        public RateLimitOptions(long itemId)
        {
            ItemId = itemId;
            Initl();
        }

        public void ApplyRateLimitOptions(string clientHeader, string excepMessage, int? httpStatusCode = 429)
        {
            DisableRateLimitHeaders = true;
            ClientIdHeader = clientHeader;
            QuotaExceededMessage = excepMessage;
            HttpStatusCode = httpStatusCode;
        }

        public void SetLimitHeadersStatus(bool status)
        {
            DisableRateLimitHeaders = status;
        }

        private void Initl()
        {
            ClientIdHeader = "ClientId";
            RateLimitCounterPrefix = "ocelot";
            HttpStatusCode = 429;
        }
    }
}
