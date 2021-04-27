using System;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.Abp.ProjectManagement.Templates
{
    public class TemplateOptions : AggregateRoot<Guid>
    {
        public virtual bool Optional { get; protected set; }
        public virtual string Key { get; protected set; }
        public virtual string FullKey { get; protected set; }
        public virtual OptionsType Type { get; protected set; }
        public virtual string Description { get; protected set; }
        protected TemplateOptions() { }
        public TemplateOptions(
            Guid id,
            string key,
            string fullKey,
            bool optional = true,
            OptionsType type = OptionsType.Empty,
            string description = "")
            : base(id)
        {
            Key = key;
            FullKey = fullKey;
            Optional = optional;
            Type = type;
            Description = description;
        }
    }
}
