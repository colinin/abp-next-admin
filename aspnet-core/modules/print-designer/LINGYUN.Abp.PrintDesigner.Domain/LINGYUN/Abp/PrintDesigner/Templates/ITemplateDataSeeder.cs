using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.PrintDesigner.Templates;

public interface ITemplateDataSeeder
{
    Task SeedAsync(
        string name,
        string? designerData = null,
        string? extendedData = null,
        CancellationToken cancellationToken = default);

    Task SeedAsync(
        string name,
        Dictionary<string, object>? designerData = null,
        Dictionary<string, object>? extendedData = null,
        CancellationToken cancellationToken = default);
}
