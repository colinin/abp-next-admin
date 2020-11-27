using JetBrains.Annotations;
using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.ApiGateWay.Admin.Http
{
    public class HttpQuery : Entity<Guid>
    {
        public virtual string Key { get; protected set; }
        public virtual string Value { get; protected set; }

        protected HttpQuery()
        {

        }

        protected HttpQuery(Guid id, [NotNull] string key, [NotNull] string value)
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
            if (obj is HttpQuery httpQuery)
            {
                return Key.Equals(httpQuery.Key, StringComparison.CurrentCultureIgnoreCase) &&
                    Value.Equals(httpQuery.Value, StringComparison.CurrentCultureIgnoreCase);
            }
            return false;
        }
    }
}
