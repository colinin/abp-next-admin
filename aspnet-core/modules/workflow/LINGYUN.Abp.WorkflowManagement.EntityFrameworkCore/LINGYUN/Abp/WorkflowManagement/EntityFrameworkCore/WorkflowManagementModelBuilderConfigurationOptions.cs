using JetBrains.Annotations;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace LINGYUN.Abp.WorkflowManagement.EntityFrameworkCore
{
    public class WorkflowManagementModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public WorkflowManagementModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "",
            [CanBeNull] string schema = null)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}
