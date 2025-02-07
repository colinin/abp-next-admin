using System.Collections.Generic;

namespace LINGYUN.Abp.Notifications.Templating;

public class NotificationSimpleModel
{
    public string Name { get; set; }
    public string Firend { get; set; }
}

public class NotificationModel
{
    public string Name { get; set; }
    public List<NotificationJob> Jobs { get; set; }
}

public class NotificationJob
{
    public string Name { get; set; }
    public NotificationJob()
    {

    }

    public NotificationJob(string name)
    {
        Name = name;
    }
}
