using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.OssManagement
{
    public interface IShareFileAppService : IApplicationService
    {
        Task<GetFileShareDto> GetAsync(string url);
    }
}
