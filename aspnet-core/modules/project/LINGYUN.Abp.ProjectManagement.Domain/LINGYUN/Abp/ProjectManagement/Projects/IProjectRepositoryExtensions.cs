using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.Abp.ProjectManagement.Projects
{
    public static class IProjectRepositoryExtensions
    {
        public static async Task<ProjectTemplate> GetTemplateAsync(
            this IProjectRepository repository,
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var template = await repository.FindTemplateAsync(id, cancellationToken);

            return template ??
                throw new EntityNotFoundException(typeof(ProjectTemplate), id);
        }
    }
}
