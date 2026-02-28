using System.Collections.Generic;

namespace LINGYUN.Abp.Elsa.EntityFrameworkCore.Migrations;

public class AbpElsaDataBaseInstallerOptions
{
    public IList<string> InstallTables { get; }
    public AbpElsaDataBaseInstallerOptions()
    {
        InstallTables = new List<string>
        {
            "Bookmarks",
            "WorkflowDefinitions",
            "WorkflowExecutionLogRecords",
            "WorkflowInstances",
            "Triggers",
            "WorkflowSettings",
            "Secrets",
            "WebhookDefinitions"
        };
    }
}
