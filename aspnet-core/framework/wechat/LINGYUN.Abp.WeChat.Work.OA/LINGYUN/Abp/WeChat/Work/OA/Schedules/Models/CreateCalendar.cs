namespace LINGYUN.Abp.WeChat.Work.OA.Schedules.Models;
/// <summary>
/// 新增日历
/// </summary>
public class CreateCalendar : CreateOrUpdateCalendar
{
    public CreateCalendar(string summary, string color, string? description)
        : base(summary, color, description)
    {
    }
}
