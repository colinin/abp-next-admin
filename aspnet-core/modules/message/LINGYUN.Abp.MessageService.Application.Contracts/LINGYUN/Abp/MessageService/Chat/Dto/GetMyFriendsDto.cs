using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.MessageService.Chat
{
    public class GetMyFriendsDto : ISortedResultRequest
    {
        public string Sorting { get; set; }
    }
}
