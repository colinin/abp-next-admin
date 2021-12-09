using Nest;

namespace LINGYUN.Abp.WorkflowCore.Persistence.Elasticsearch
{
    public class AbpWorkflowCorePersistenceElasticsearchOptions
    {
        /// <summary>
        /// Default Value: abp.workflows.persistence.{0}
        /// </summary>
        public string IndexFormat { get; set; }
        public IIndexSettings IndexSettings { get; set; }
        public AbpWorkflowCorePersistenceElasticsearchOptions()
        {
            IndexFormat = "abp.workflows.persistence.{0}";
            IndexSettings = new IndexSettings();
        }
    }
}
