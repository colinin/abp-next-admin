using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.Abp.WebhooksManagement;
public static class WebhookDefinitionRecordRepositoryExtensions
{
    public async static Task<WebhookDefinitionRecord> GetByNameAsync(
        this IWebhookDefinitionRecordRepository repository,
        string name,
        CancellationToken cancellationToken = default)
    {
        return await repository.FindByNameAsync(name, cancellationToken)
            ?? throw new EntityNotFoundException(typeof(WebhookDefinitionRecord), name);
    }
}
