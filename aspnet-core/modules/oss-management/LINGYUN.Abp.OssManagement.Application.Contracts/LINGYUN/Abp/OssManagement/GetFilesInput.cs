using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.OssManagement
{
    public class GetFilesInput: LimitedResultRequestDto
    {
        public string Filter { get; set; }
        public string Path { get; set; }
    }
}
