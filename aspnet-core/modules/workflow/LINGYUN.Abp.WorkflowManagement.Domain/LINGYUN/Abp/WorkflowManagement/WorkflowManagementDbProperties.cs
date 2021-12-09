namespace LINGYUN.Abp.WorkflowManagement
{
    public static class WorkflowManagementDbProperties
    {
        public static string DbTablePrefix { get; set; } = "WorkflowManagement_";

        public static string DbSchema { get; set; } = null;


        public const string ConnectionStringName = "WorkflowManagement";
    }
}
