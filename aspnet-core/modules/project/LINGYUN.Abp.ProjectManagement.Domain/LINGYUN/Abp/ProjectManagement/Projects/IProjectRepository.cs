using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Abp.ProjectManagement.Projects
{
    public interface IProjectRepository : IRepository<Project, Guid>
    {
        Task<bool> CheckNameAsync(
            string name,
            CancellationToken cancellationToken = default);
        Task<Project> FindByNameAsync(
            string name, 
            CancellationToken cancellationToken = default);

        Task<ProjectTemplate> FindTemplateAsync(
            Guid id,
            CancellationToken cancellationToken = default);
    }
}
