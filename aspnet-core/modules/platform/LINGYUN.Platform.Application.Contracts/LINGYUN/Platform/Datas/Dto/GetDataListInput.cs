using Volo.Abp.Application.Dtos;

namespace LINGYUN.Platform.Datas
{
    public class GetDataListInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}
