namespace LINGYUN.Abp.WorkflowCore.Elasticsearch
{
    public class AbpWorkflowCoreElasticsearchOptions
    {
        /// <summary>
        /// Default value: "workflows".
        /// </summary>
        public string IndexFormat { get; set; }
        public AbpWorkflowCoreElasticsearchOptions()
        {
            IndexFormat = "workflows";
        }
    }
}
