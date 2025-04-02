using System;
using Volo.Abp.Auditing;

namespace PackageName.CompanyName.ProjectName.TreeCodes
{
    /// <summary>
    /// 定义具有树形编码的实体接口
    /// </summary>
    public interface IHaveTreeCode : IHasCreationTime
    {
        /// <summary>
        /// 树形编码
        /// </summary>
        string TreeCode { get; set; }
        
        /// <summary>
        /// 父级Id
        /// </summary>
        Guid? ParentId { get; set; }
    }
}
