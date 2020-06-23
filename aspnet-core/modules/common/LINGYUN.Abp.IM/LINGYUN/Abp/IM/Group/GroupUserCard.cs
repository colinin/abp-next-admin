using System.Collections.Generic;
using System.Linq;

namespace LINGYUN.Abp.IM.Group
{
    public class GroupUserCard : UserCard
    {
        public long GroupId { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsSuperAdmin { get; set; }
        public IDictionary<string, bool> Permissions { get; set; }
        public GroupUserCard()
        {
            Permissions = new Dictionary<string, bool>();
        }

        public bool IsGrant(string key)
        {
            return Permissions.Any(x => x.Equals(key) && x.Value);
        }
    }
}
