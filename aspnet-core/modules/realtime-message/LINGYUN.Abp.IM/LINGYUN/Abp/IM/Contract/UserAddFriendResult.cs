namespace LINGYUN.Abp.IM.Contract
{
    public class UserAddFriendResult
    {
        public bool Successed => Status == UserFriendStatus.Added;
        public UserFriendStatus Status { get; }
        public UserAddFriendResult(UserFriendStatus status)
        {
            Status = status;
        }
    }
}
