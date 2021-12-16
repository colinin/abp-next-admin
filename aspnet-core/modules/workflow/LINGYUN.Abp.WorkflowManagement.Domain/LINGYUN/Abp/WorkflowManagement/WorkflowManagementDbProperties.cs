namespace LINGYUN.Abp.WorkflowManagement
{
    public static class WorkflowManagementDbProperties
    {
        public static string DbTablePrefix { get; set; } = "WF_";

        public static string DbSchema { get; set; } = null;


        public const string ConnectionStringName = "WorkflowManagement";
    }
}
