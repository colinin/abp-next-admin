namespace LINGYUN.Abp.TaskManagement
{
    public static class TaskManagementErrorCodes
    {
        public const string Namespace = "TaskManagement";
        /// <summary>
        /// 分组 {Group} 中已经存在一个名称为 {Name} 的作业!
        /// </summary>
        public const string JobNameAlreadyExists = Namespace + ":01000";
        /// <summary>
        /// 队列没有找到分组 {Group} 中名称为 {Name} 的作业, 请先将作业入队!
        /// </summary>
        public const string JobNotFoundInQueue = Namespace + ":01001";
        /// <summary>
        /// 仅允许删除已停止作业
        /// </summary>
        public const string OnlyDeletionOfStopJobsIsAllowed = Namespace + ":01002";
    }
}
