using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.Abp.ProjectManagement.Projects
{
    public class ProjectTemplate : AggregateRoot<Guid>
    {
        public virtual Guid ProjectId { get; protected set; }
        public virtual Guid TemplateId { get; protected set; }
        public virtual ICollection<ProjectOptions> Options { get; protected set; }
        protected ProjectTemplate() { }
        public ProjectTemplate(
            Guid id,
            Guid projectId,
            Guid templateId)
            : base(id)
        {
            ProjectId = projectId;
            TemplateId = templateId;

            Options = new Collection<ProjectOptions>();
        }
    }
}
