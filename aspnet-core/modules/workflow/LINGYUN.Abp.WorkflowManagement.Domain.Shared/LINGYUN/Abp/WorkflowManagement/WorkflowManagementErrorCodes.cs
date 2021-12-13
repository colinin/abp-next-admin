namespace LINGYUN.Abp.WorkflowManagement
{
    public static class WorkflowManagementErrorCodes
    {
        public const string Namespace = "Workflow";

        public const string ResumeError = Namespace + ":10100";
        public const string SuspendError = Namespace + ":10101";
        public const string TerminateError = Namespace + ":10102";
    }
}
