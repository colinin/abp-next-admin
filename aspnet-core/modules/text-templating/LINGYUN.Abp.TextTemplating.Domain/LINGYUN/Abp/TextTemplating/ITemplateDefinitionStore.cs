using JetBrains.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.TextTemplating;

namespace LINGYUN.Abp.TextTemplating;
public interface ITemplateDefinitionStore
{
    Task CreateAsync(TextTemplateDefinition template);

    Task UpdateAsync(TextTemplateDefinition template);

    Task DeleteAsync(string name, CancellationToken cancellationToken = default);

    [NotNull]
    Task<TemplateDefinition> GetAsync(string name, CancellationToken cancellationToken = default);

    [NotNull]
    Task<IReadOnlyList<TemplateDefinition>> GetAllAsync(CancellationToken cancellationToken = default);

    [CanBeNull]
    Task<TemplateDefinition> GetOrNullAsync(string name, CancellationToken cancellationToken = default);
}
