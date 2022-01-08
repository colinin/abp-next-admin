namespace LINGYUN.Abp.TaskManagement;

public static class TaskManagementDbProperties
{
    public static string DbTablePrefix { get; set; } = "TK_";

    public static string DbSchema { get; set; } = null;


    public const string ConnectionStringName = "TaskManagement";
}
