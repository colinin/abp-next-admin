namespace LINGYUN.Abp.DataProtection
{
    public class ProtectedResource
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
        /// 优先级
        /// 值越大排名越靠前
        /// </summary>
        public int Priority { get; set; }
        /// <summary>
        /// 行为
        /// </summary>
        public ProtectBehavior Behavior { get; set; }
    }
}
