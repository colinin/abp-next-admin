using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.OssManagement;

public interface IOssContainerAppService: IApplicationService
{
    Task<OssContainerDto> CreateAsync(string name);

    Task<OssContainerDto> GetAsync(string name);

    Task DeleteAsync(string name);

    Task<OssContainersResultDto> GetListAsync(GetOssContainersInput input);

    Task<OssObjectsResultDto> GetObjectListAsync(GetOssObjectsInput input);
}
