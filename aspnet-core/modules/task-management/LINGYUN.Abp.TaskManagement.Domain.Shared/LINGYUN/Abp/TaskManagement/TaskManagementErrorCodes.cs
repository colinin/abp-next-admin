namespace LINGYUN.Abp.TaskManagement
{
    public static class TaskManagementErrorCodes
    {
        public const string Namespace = "TaskManagement";

        public const string JobNameAlreadyExists = Namespace + ":01000";

        public const string JobNotFoundInQueue = Namespace + ":01001";
    }
}
