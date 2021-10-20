using LINGYUN.Abp.IM.Messages;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.MessageService.Chat
{
    public class GroupMessageGetByPagedDto : PagedAndSortedResultRequestDto
    {
        [Required]
        public long GroupId { get; set; }
        public string Filter { get; set; }
        public MessageType? MessageType { get; set; }
    }
}
