using System;

namespace LINGYUN.Abp.AIManagement.Chats;
public class ConversationCleanupOptions
{
    public bool IsCleanupEnabled { get; set; }
    public TimeSpan ExpiredTime { get; set; }
    public int CleanupPeriod { get; set; }
    public ConversationCleanupOptions()
    {
        IsCleanupEnabled = true;
        CleanupPeriod = 3_600_000;
        ExpiredTime = TimeSpan.FromHours(2);
    }
}
