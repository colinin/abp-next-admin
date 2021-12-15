namespace LINGYUN.Abp.WorkflowManagement.Workflows
{
    public class WorkflowDataDto
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 数据类型
        /// </summary>
        public DataType DataType { get; set; }
        /// <summary>
        /// 是否必输
        /// 默认: false
        /// </summary>
        public bool IsRequired { get; set; }
        /// <summary>
        /// 是否区分大小写
        /// 默认: false
        /// 暂无用
        /// </summary>
        public bool IsCaseSensitive { get; set; }
    }
}
