using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Abp.TextTemplating;
public interface ITextTemplateDefinitionRepository : IBasicRepository<TextTemplateDefinition, Guid>
{
    Task<TextTemplateDefinition> FindByNameAsync(
        string name,
        CancellationToken cancellationToken = default);

    Task<long> GetCountAsync(
        string filter = null,
        CancellationToken cancellationToken = default);

    Task<List<TextTemplateDefinition>> GetListAsync(
       string filter = null,
       string sorting = nameof(TextTemplateDefinition.Name),
       int skipCount = 0,
       int maxResultCount = 10,
       CancellationToken cancellationToken = default);
}
