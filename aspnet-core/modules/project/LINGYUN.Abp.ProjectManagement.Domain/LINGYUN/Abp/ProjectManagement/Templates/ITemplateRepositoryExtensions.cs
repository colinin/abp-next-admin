using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.Abp.ProjectManagement.Templates
{
    public static class ITemplateRepositoryExtensions
    {
        public static async Task<Template> GetByNameAsync(
            this ITemplateRepository repository,
            string name,
            CancellationToken cancellationToken = default)
        {
            var template = await repository.FindByNameAsync(name, cancellationToken);

            return template ??
                throw new EntityNotFoundException(typeof(Template), name);
        }
    }
}
