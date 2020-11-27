using JetBrains.Annotations;
using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.ApiGateWay.Admin.Http
{
    public class HttpDelegatingHandler : Entity<Guid>
    {
        public virtual string Name { get; protected set; }
        protected HttpDelegatingHandler()
        {

        }

        public HttpDelegatingHandler(Guid id, [NotNull] string name)
        {
            Id = id;
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), HttpDelegatingHandlerConsts.MaxNameLength);
        }

        public override int GetHashCode()
        {
            if (!Name.IsNullOrWhiteSpace())
            {
                return Name.GetHashCode();
            }
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null ||
               Name.IsNullOrWhiteSpace())
            {
                return false;
            }
            if (obj is HttpDelegatingHandler handler)
            {
                return Name.Equals(handler.Name, StringComparison.CurrentCultureIgnoreCase);
            }
            return false;
        }
    }
}
