using System;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.ApiGateWay.Admin.Http
{
    public class HttpHandler : Entity<Guid>
    {
        /// <summary>
        /// 每台服务器最大连接数
        /// </summary>
        public virtual int? MaxConnectionsPerServer { get; protected set; }
        /// <summary>
        /// 允许自动重定向
        /// </summary>
        public virtual bool AllowAutoRedirect { get; set; }
        /// <summary>
        /// 使用Cookie容器
        /// </summary>
        public virtual bool UseCookieContainer { get; set; }
        /// <summary>
        /// 启用跟踪
        /// </summary>
        public virtual bool UseTracing { get; set; }
        /// <summary>
        /// 启用代理
        /// </summary>
        public virtual bool UseProxy { get; set; }
        protected HttpHandler()
        {

        }

        public HttpHandler(Guid id)
        {
            Id = id;
        }

        public void ChangeMaxConnection(int? maxConnection = 1000)
        {
            MaxConnectionsPerServer = maxConnection;
        }
    }
}
