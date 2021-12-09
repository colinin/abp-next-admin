using System.Threading.Tasks;

namespace LINGYUN.Abp.WorkflowCore.Persistence.Elasticsearch
{
    public interface IPersistenceIndexInitializer
    {
        Task InitializeAsync();
    }
}
