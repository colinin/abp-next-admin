using Microsoft.EntityFrameworkCore;
using System;
using Volo.Abp;

namespace LINGYUN.Abp.WorkflowManagement.EntityFrameworkCore
{
    public static class WorkflowManagementDbContextModelCreatingExtensions
    {
        public static void ConfigureWorkflowManagement(
            this ModelBuilder builder,
            Action<WorkflowManagementModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new WorkflowManagementModelBuilderConfigurationOptions(
                WorkflowManagementDbProperties.DbTablePrefix,
                WorkflowManagementDbProperties.DbSchema
            );
            optionsAction?.Invoke(options);
        }
    }
}
