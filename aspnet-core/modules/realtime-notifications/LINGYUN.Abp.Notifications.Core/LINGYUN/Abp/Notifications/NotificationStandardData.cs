namespace LINGYUN.Abp.Notifications;
public class NotificationStandardData
{
    public string Title { get; set; }
    public string Message { get; set; }
    public string Description { get; set; }
    public NotificationStandardData()
    {

    }

    public NotificationStandardData(
        string title, 
        string message, 
        string description = null)
    {
        Title = title; 
        Message = message; 
        Description = description;
    }
}
