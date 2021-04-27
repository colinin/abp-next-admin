using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Abp.ProjectManagement.Templates
{
    public interface ITemplateRepository : IRepository<Template, Guid>
    {
        Task<Template> FindByNameAsync(
            string name,
            CancellationToken cancellationToken = default);
    }
}
