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
            Guid id,
            [NotNull] string name,
            [CanBeNull] string displayName = null,
            [CanBeNull] string description = null,
            [CanBeNull] string defaultCultureName = null)
            : base(id)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), ResourceConsts.MaxNameLength);

            DisplayName = Check.Length(displayName ?? Name, nameof(displayName), ResourceConsts.MaxDisplayNameLength);;
            Description = Check.Length(description, nameof(description), ResourceConsts.MaxDescriptionLength);
            DefaultCultureName = Check.Length(defaultCultureName, nameof(defaultCultureName), ResourceConsts.MaxDefaultCultureNameLength);

            Enable = true;
        }

        public virtual void SetDisplayName(string displayName)
        {
            DisplayName = Check.Length(displayName, nameof(displayName), ResourceConsts.MaxDisplayNameLength);
        }

        public virtual void SetDescription(string description)
        {
            Description = Check.Length(description, nameof(description), ResourceConsts.MaxDescriptionLength);
        }

        public virtual void SetDefaultCultureName(string defaultCultureName)
        {
            DefaultCultureName = Check.Length(defaultCultureName, nameof(defaultCultureName), ResourceConsts.MaxDefaultCultureNameLength);
        }
    }
}
