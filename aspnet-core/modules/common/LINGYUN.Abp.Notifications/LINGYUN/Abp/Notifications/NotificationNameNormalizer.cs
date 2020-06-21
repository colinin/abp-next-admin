namespace LINGYUN.Abp.Notifications
{
    public static class NotificationNameNormalizer
    {
        public static NotificationName NormalizerName(string name)
        {
            return new NotificationName(name, name);
        }
        public static NotificationName NormalizerName(string category, string name)
        {
            var notifyName = string.Concat(category, ":", name);
            return new NotificationName(category, notifyName);
        }
    }
}
