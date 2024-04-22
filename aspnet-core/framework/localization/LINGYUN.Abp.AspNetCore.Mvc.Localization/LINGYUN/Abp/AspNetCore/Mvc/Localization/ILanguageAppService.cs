using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.AspNetCore.Mvc.Localization
{
    public interface ILanguageAppService : IApplicationService
    {
        Task<ListResultDto<LanguageDto>> GetListAsync(GetLanguageWithFilterDto input);
    }
}
