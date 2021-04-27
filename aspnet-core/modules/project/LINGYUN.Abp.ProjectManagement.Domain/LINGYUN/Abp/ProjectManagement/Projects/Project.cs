using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.ProjectManagement.Projects
{
    public class Project : AuditedAggregateRoot<Guid>
    {
        public virtual string Name { get; protected set; }
        public virtual string Version { get; protected set; }
        public virtual BuildStatus Status { get; protected set; }
        public virtual DateTime? BuildTime { get; protected set; }
        public virtual string BuildError { get; protected set; }
        public virtual string PackageIconUrl { get; protected set; }
        public virtual string PackageProjectUrl { get; protected set; }
        public virtual string PackageLicenseExpression { get; protected set; }
        public virtual RepositoryType? RepositoryType { get; protected set; }
        public virtual string RepositoryUrl { get; protected set; }
        public virtual string Template { get; protected set; }
        protected Project() { }
        public Project(
            Guid id,
            string template,
            string name,
            string version = "1.0.0.0",
            string packageIconUrl = "",
            string packageProjectUrl = "",
            string packageLicenseExpression = "MIT",
            RepositoryType? repositoryType = null,
            string repositoryUrl = "")
            : base(id)
        {
            Name = name;
            Template = template;
            Version = version;
            PackageIconUrl = packageIconUrl;
            PackageProjectUrl = packageProjectUrl;
            PackageLicenseExpression = packageLicenseExpression;
            RepositoryType = repositoryType;
            RepositoryUrl = repositoryUrl;

            Status = BuildStatus.Created;
        }

        public void BuildSuccess(IClock clock)
        {
            BuildError = "";
            BuildTime = clock.Now;
            Status = BuildStatus.Successed;
        }

        public void BuildFailed(IClock clock, Exception ex)
        {
            BuildTime = clock.Now;
            Status = BuildStatus.Failed;
            BuildError = GetBaseExceptionError(ex);
        }

        private static string GetBaseExceptionError(Exception ex)
        {
            if (ex == null)
            {
                return "";
            }
            return string.Concat(ex?.Message ?? "", GetBaseExceptionError(ex?.InnerException));
        }
    }
}
