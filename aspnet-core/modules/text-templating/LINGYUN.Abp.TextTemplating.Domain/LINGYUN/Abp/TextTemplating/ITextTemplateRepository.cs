using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Abp.TextTemplating;

public interface ITextTemplateRepository : IRepository<TextTemplate, Guid>
{
    Task<TextTemplate> FindByNameAsync(string name, string culture = null, CancellationToken cancellationToken = default);
}
