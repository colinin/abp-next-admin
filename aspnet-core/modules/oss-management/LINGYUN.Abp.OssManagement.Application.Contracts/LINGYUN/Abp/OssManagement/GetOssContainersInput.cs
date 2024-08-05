using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.OssManagement;

public class GetOssContainersInput : PagedAndSortedResultRequestDto
{
    public string Prefix { get; set; }
    public string Marker { get; set; }
}
