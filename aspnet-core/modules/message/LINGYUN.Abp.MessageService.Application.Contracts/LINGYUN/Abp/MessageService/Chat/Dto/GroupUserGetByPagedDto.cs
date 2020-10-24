using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.MessageService.Chat
{
    public class GroupUserGetByPagedDto : PagedAndSortedResultRequestDto
    {
        [Required]
        public long GroupId { get; set; }

        public bool Reverse { get; set; }

        public string Filter { get; set; }
    }
}
