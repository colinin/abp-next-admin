using System.Threading.Tasks;

namespace LINGYUN.Abp.WebhooksManagement;

public interface IStaticWebhookSaver
{
    Task SaveAsync();
}
