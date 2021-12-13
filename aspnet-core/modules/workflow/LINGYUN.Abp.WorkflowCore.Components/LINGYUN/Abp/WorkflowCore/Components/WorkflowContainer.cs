using Volo.Abp.BlobStoring;

namespace LINGYUN.Abp.WorkflowCore.Components
{
    [BlobContainerName(Name)]
    public class WorkflowContainer
    {
        public const string Name = "workflow";
    }
}
