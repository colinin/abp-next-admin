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
        public virtual string DefaultCultureName { get; set; }
        protected Resource() { }
        public Resource(
            [NotNull] string name,
            [CanBeNull] string displayName = null,
            [CanBeNull] string description = null,
            [CanBeNull] string defaultCultureName = null)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), ResourceConsts.MaxNameLength);

            DisplayName = Check.Length(displayName ?? Name, nameof(displayName), ResourceConsts.MaxDisplayNameLength);;
            Description = Check.Length(description, nameof(description), ResourceConsts.MaxDescriptionLength);
            DefaultCultureName = Check.Length(defaultCultureName, nameof(defaultCultureName), ResourceConsts.MaxDefaultCultureNameLength);

            Enable = true;
        }
    }
}
