using System;
using System.Linq;
using System.Linq.Expressions;

namespace LINGYUN.Abp.DataProtection
{
    public class ProtectedFieldRule
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
        /// <summary>
        /// 值
        /// </summary>
        public object Value { get; set; }
        /// <summary>
        /// 连接类型
        /// Or 或
        /// And 且
        /// </summary>
        public PredicateOperator Logic { get; set; }
        /// <summary>
        /// 操作符
        /// </summary>
        public ExpressionType Operator { get; set; }
    }
}
