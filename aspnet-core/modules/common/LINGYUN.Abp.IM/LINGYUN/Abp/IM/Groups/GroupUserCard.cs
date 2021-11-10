namespace LINGYUN.Abp.IM.Groups
{
    public class GroupUserCard : UserCard
    {
        public long GroupId { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsSuperAdmin { get; set; }
        public GroupUserCard()
        {
        }
    }
}
