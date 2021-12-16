namespace LINGYUN.Abp.WorkflowManagement
{
    public static class WorkflowManagementErrorCodes
    {
        public const string Namespace = "Workflow";

        public const string ResumeError = Namespace + ":10100";
        public const string SuspendError = Namespace + ":10101";
        public const string TerminateError = Namespace + ":10102";

        /// <summary>
        /// 缺失必要的属性 {Property}
        /// </summary>
        public const string MissingRequiredProperty = Namespace + ":10110";
        /// <summary>
        /// 必输字段 {Property} 不能为空
        /// </summary>
        public const string InvalidInputNullable = Namespace + ":10111";
        /// <summary>
        /// 字段 {Property} 不是有效的 {DataType} 类型
        /// </summary>
        public const string InvalidInputDataType = Namespace + ":10112";
    }
}
