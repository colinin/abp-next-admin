using System.Threading.Tasks;

namespace LINGYUN.Abp.AuditLogging.Elasticsearch
{
    public interface IIndexInitializer
    {
        Task InitializeAsync();
    }
}
