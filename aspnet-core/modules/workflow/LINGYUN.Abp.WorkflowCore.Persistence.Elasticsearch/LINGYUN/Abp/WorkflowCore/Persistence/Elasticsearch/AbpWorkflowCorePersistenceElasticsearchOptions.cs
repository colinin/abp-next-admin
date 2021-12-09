namespace LINGYUN.Abp.WorkflowCore.Persistence.Elasticsearch
{
    public class AbpWorkflowCorePersistenceElasticsearchOptions
    {
        /// <summary>
        /// Default Value: abp.workflows.persistence
        /// </summary>
        public string IndexFormat { get; set; }
        public AbpWorkflowCorePersistenceElasticsearchOptions()
        {
            IndexFormat = "abp.workflows.persistence";
        }
    }
}
