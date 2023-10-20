using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.AspNetCore.Mvc.Localization
{
    public interface IResourceAppService : IApplicationService
    {
        Task<ListResultDto<ResourceDto>> GetListAsync(GetResourceWithFilterDto input);
    }
}
