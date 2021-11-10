using System;

namespace LINGYUN.Abp.IM.Groups
{
    public class UserGroup
    {
        public Guid? TenantId { get; set; }
        public Guid UserId { get; set; }
        public long GroupId { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsSuperAdmin { get; set; }
    }
}
