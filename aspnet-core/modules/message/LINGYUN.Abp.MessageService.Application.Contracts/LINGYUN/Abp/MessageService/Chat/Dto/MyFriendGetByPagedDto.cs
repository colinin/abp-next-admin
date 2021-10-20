using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.MessageService.Chat
{
    public class MyFriendGetByPagedDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }

    public class MyLastContractFriendGetByPagedDto : PagedResultRequestDto
    {
    }
}
