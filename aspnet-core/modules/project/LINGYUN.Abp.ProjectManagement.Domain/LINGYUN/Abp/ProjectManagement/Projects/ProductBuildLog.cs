using Microsoft.Extensions.Logging;
using System;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.Abp.ProjectManagement.Projects
{
    public class ProductBuildLog : Entity<int>
    {
        public virtual Guid ProjectId { get; set; }
        public virtual string Message { get; set; }
        public virtual LogLevel Level { get; set; }
        protected ProductBuildLog() { }
        public ProductBuildLog(
            Guid projectId,
            string message,
            LogLevel level = LogLevel.Information)
        {
            ProjectId = projectId;
            Message = message;
            Level = level;
        }
    }
}
