using System;

namespace LINGYUN.Abp.IM.Group
{
    public class UserGroup
    {
        public Guid? TenantId { get; set; }
        public Guid UserId { get; set; }
        public long GroupId { get; set; }
    }
}
