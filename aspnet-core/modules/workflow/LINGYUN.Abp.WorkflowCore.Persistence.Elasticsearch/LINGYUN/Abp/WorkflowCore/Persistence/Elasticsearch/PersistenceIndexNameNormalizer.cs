using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.WorkflowCore.Persistence.Elasticsearch
{
    public class PersistenceIndexNameNormalizer : IPersistenceIndexNameNormalizer, ISingletonDependency
    {
        private readonly AbpWorkflowCorePersistenceElasticsearchOptions _options;

        public PersistenceIndexNameNormalizer(
            IOptions<AbpWorkflowCorePersistenceElasticsearchOptions> options)
        {
            _options = options.Value;
        }

        public string NormalizeIndex(string index)
        {
            return string.Format(_options.IndexFormat, index);
        }
    }
}
