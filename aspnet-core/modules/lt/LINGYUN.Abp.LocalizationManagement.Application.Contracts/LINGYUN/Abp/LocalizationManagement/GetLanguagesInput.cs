using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.LocalizationManagement
{
    public class GetLanguagesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}
