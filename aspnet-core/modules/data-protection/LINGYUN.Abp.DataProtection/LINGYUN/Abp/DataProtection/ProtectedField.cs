namespace LINGYUN.Abp.DataProtection
{
    public class ProtectedField
    {
        /// <summary>
        /// 资源
        /// </summary>
        public string Resource { get; set; }
        /// <summary>
        /// 资源拥有者
        /// </summary>
        public string Owner { get; set; }
        /// <summary>
        /// 资源访问者
        /// </summary>
        public string Visitor { get; set; }
        /// <summary>
        /// 字段
        /// </summary>
        public string Field { get; set; }
    }
}
