namespace LINGYUN.Abp.Notifications
{
    public class NotificationName
    {
        public string CateGory { get; }
        public string Name { get; }

        public NotificationName(string cateGory, string name)
        {
            Name = name;
            CateGory = cateGory;
        }
    }
}
