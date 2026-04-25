using JetBrains.Annotations;
using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.Notifications;

#nullable enable
public class NotificationPublishContext
{
    [NotNull]
    public NotificationInfo Notification { get; }

    [CanBeNull]
    public IEnumerable<UserIdentifier> Users { get; }

    [CanBeNull]
    public string? Reason { get; private set; }

    [CanBeNull]
    public Exception? Exception { get; private set; }
    public NotificationPublishContext(
        NotificationInfo notification, 
        IEnumerable<UserIdentifier> users)
    {
        Notification = notification;
        Users = users;
    }

    public void Cancel(string reason, Exception? exception = null)
    {
        Reason = reason;
        Exception = exception;
    }
}
#nullable disable
