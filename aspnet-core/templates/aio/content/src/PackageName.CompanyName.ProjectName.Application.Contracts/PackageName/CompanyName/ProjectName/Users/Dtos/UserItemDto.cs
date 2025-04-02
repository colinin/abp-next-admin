using System;
using Volo.Abp.Application.Dtos;

namespace PackageName.CompanyName.ProjectName.Users.Dtos
{
    [Serializable]
    public class UserItemDto : FullAuditedEntityDto<Guid>
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// Identity用户Id
        /// </summary>
        public Guid IdentityUserId { get; set; }

        /// <summary>
        /// 用户状态
        /// </summary>
        public bool IsActive { get; set; }
        
        /// <summary>
        /// 联系方式
        /// </summary>
        public string ContactInfo { get; set; }
        
        /// <summary>
        /// 职位
        /// </summary>
        public string Position { get; set; }
        
        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleNames { get; set; }
    }
}