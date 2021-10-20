using LINGYUN.Abp.Notifications;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.MessageService.Notifications
{
    public class UserNotificationGetByPagedDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public NotificationReadState? ReadState { get; set; }
    }
}
