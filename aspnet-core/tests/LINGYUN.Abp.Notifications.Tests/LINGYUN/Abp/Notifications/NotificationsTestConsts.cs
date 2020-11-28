using System;

namespace LINGYUN.Abp.Notifications
{
    public static class NotificationsTestConsts
    {
        public static Guid User1Id { get; } = Guid.NewGuid();

        public static Guid User2Id { get; } = Guid.NewGuid();

        public static long NotificationId1 { get; set; }
        public static long NotificationId2 { get; set; }
        public static long NotificationId3 { get; set; }
    }
}
