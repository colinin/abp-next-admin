using JetBrains.Annotations;
using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.ApiGateWay.Admin.Security
{
    public class Claim : Entity<Guid>
    {
        public virtual string Key { get; protected set; }
        public virtual string Value { get; protected set; }
        protected Claim()
        {

        }

        protected Claim(Guid id, [NotNull] string key, [NotNull] string value)
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
            if (obj is Claim claim)
            {
                return claim.Key.Equals(Key, StringComparison.CurrentCultureIgnoreCase) &&
                    claim.Value.Equals(Value, StringComparison.CurrentCultureIgnoreCase);
            }
            return false;
        }
    }
}
