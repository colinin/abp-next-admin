using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.LocalizationManagement;

public class LanguageGetPagedListInput : PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }
}
