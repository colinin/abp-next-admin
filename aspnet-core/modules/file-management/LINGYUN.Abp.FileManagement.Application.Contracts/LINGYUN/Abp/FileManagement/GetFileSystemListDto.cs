using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.FileManagement
{
    public class GetFileSystemListDto : PagedAndSortedResultRequestDto
    {
        // TODO: Windows最大路径长度,超过了貌似也无效了吧
        [StringLength(255)]
        public string Parent { get; set; }

        public string Filter { get; set; }
    }
}
