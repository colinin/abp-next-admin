using JetBrains.Annotations;
using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace LINGYUN.Abp.LocalizationManagement
{
    public class Resource : AuditedEntity<Guid>
    {
        public virtual bool Enable { get; set; }
        public virtual string Name { get; set; }
        public virtual string DisplayName { get; set; }
        public virtual string Description { get; set; }
        protected Resource() { }
        public Resource(
            [NotNull] string name,
            [CanBeNull] string displayName = null,
            [CanBeNull] string description = null)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), ResourceConsts.MaxNameLength);

            DisplayName = displayName ?? Name;
            Description = description;

            Enable = true;
        }
    }
}
