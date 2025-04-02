using System.Collections.Generic;

namespace LINGYUN.Abp.Quartz.SqlInstaller;

public class AbpQuartzSqlInstallerOptions
{
    public IList<string> InstallTables { get; }
    public AbpQuartzSqlInstallerOptions()
    {
        InstallTables = new List<string>
        {
            "FIRED_TRIGGERS",
            "PAUSED_TRIGGER_GRPS",
            "SCHEDULER_STATE",
            "LOCKS",
            "SIMPLE_TRIGGERS",
            "SIMPROP_TRIGGERS",
            "CRON_TRIGGERS",
            "BLOB_TRIGGERS",
            "TRIGGERS",
            "JOB_DETAILS",
            "CALENDARS",
        };
    }
}
