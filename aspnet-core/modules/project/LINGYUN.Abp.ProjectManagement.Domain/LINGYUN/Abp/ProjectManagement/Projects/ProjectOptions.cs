using System;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.Abp.ProjectManagement.Projects
{
    public class ProjectOptions : Entity<int>
    {
        public virtual Guid ProjectId { get; protected set; }
        public virtual string Key { get; protected set; }
        public virtual string Value { get; protected set; }
        protected ProjectOptions() { }
        public ProjectOptions(
            Guid projectId,
            string key,
            string value = "")
        {
            ProjectId = projectId;
            Key = key;
            Value = value;
        }
    }
}
