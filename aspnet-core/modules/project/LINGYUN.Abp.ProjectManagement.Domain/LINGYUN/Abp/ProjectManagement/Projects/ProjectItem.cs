using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.Abp.ProjectManagement.Projects
{
    public class ProjectItem : AggregateRoot<Guid>
    {
        public virtual string Path { get; protected set; }
        public virtual string Name { get; protected set; }
    }
}
