using JetBrains.Annotations;
using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.ApiGateWay.Admin.Http
{
    public abstract class HttpHeader : Entity<Guid>
    {
        public virtual string Key { get; protected set; }
        public virtual string Value { get; protected set; }
        
        protected HttpHeader()
        {

        }

        protected HttpHeader(Guid id, [NotNull] string key, [NotNull]  string value)
        {
            Id = id;
            Key = Check.NotNullOrWhiteSpace(key, nameof(key));
            Value = Check.NotNullOrWhiteSpace(value, nameof(value));
        }

        public override int GetHashCode()
        {
            if (!Key.IsNullOrWhiteSpace() &&
                !Value.IsNullOrWhiteSpace())
            {
                return Key.GetHashCode() & Value.GetHashCode();
            }
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null ||
               Key.IsNullOrWhiteSpace() ||
               Value.IsNullOrWhiteSpace())
            {
                return false;
            }
            if (obj is HttpHeader httpHeader)
            {
                return httpHeader.Key.Equals(Key, StringComparison.CurrentCultureIgnoreCase) &&
                    httpHeader.Value.Equals(Value, StringComparison.CurrentCultureIgnoreCase);
            }
            return false;
        }
    }
}
