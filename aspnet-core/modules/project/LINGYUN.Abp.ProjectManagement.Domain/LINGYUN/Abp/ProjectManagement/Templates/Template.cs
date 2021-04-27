using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.Abp.ProjectManagement.Templates
{
    public class Template : AggregateRoot<Guid>
    {
        public virtual string Name { get; protected set; }
        public virtual ICollection<TemplateOptions> Options { get; protected set; }
        protected Template() { }
        public Template(
            Guid id,
            string name)
            : base(id)
        {
            Name = name;
            Options = new Collection<TemplateOptions>();
        }

        public IEnumerable<TemplateOptions> GetMustOptions()
        {
            return Options.Where(x => !x.Optional);
        }
    }
}
