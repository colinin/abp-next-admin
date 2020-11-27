using System;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.ApiGateWay.Admin.Http
{
    public abstract class HttpMethod : Entity<Guid>
    {
        /// <summary>
        /// Http调用方法
        /// </summary>
        public virtual string Method { get; protected set; }
        protected HttpMethod()
        {

        }
        protected HttpMethod(Guid id, string method)
        {
            Id = id;
            Method = method;
        }

        public override int GetHashCode()
        {
            if (!Method.IsNullOrWhiteSpace())
            {
                return Method.GetHashCode();
            }
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null ||
               Method.IsNullOrWhiteSpace())
            {
                return false;
            }
            if (obj is HttpMethod httpMethod)
            {
                return httpMethod.Method.Equals(Method);
            }
            return false;
        }
    }
}
