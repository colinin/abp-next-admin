using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Identity;

namespace PackageName.CompanyName.ProjectName.Users
{
    /// <summary>
    /// 用户实体
    /// </summary>
    public class User : FullAuditedEntity<Guid>
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        [MaxLength(50)]
        public string NickName { get; set; }

        /// <summary>
        /// Identity用户Id
        /// </summary>
        public Guid IdentityUserId { get; set; }

        /// <summary>
        /// Identity用户
        /// </summary>
        public virtual IdentityUser IdentityUser { get; set; }
        
        /// <summary>
        /// 联系方式
        /// </summary>
        [MaxLength(50)]
        public string ContactInfo { get; set; }
        
        /// <summary>
        /// 职位
        /// </summary>
        [MaxLength(50)]
        public string Position { get; set; }

        protected User()
        {
        }

        public User(
            Guid id,
            string nickName,
            Guid identityUserId,
            string contactInfo = null,
            string position = null
        ) : base(id)
        {
            NickName = nickName;
            IdentityUserId = identityUserId;
            ContactInfo = contactInfo;
            Position = position;
        }
    }
}
