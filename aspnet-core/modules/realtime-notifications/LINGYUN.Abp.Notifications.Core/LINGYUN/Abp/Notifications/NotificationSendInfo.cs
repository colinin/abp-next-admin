using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using Volo.Abp;

namespace LINGYUN.Abp.Notifications;
public class NotificationSendInfo
{
    public string Provider { get; }
    public DateTime SendTime { get; }
    public NotificationInfo NotificationInfo { get; }
    public IEnumerable<UserIdentifier> Users { get; }
    public NotificationSendState State { get; private set; }
    public string Reason { get; private set; }
    public NotificationSendInfo(
        [NotNull] string provider,
        DateTime sendTime, 
        NotificationInfo notificationInfo, 
        IEnumerable<UserIdentifier> users)
    {
        Check.NotNullOrWhiteSpace(provider, nameof(provider));
        Check.NotNull(notificationInfo, nameof(notificationInfo));
        Check.NotNull(users, nameof(users));

        Provider = provider;
        SendTime = sendTime;
        NotificationInfo = notificationInfo;
        Users = users;

        State = NotificationSendState.None;
    }

    public void Cancel(string reason)
    {
        State = NotificationSendState.None;
        Reason = reason;
    }

    public void Disbaled()
    {
        State = NotificationSendState.Disabled;
    }

    public void Sent(Exception exception = null)
    {
        State = exception != null ? NotificationSendState.Failed : NotificationSendState.Sent;
        Reason = exception?.Message;
    }
}
