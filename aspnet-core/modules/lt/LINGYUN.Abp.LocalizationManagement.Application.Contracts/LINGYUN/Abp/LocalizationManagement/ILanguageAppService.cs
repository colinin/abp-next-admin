using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.LocalizationManagement
{
    public interface ILanguageAppService : 
        ICrudAppService<
            LanguageDto,
            Guid,
            GetLanguagesInput,
            CreateOrUpdateLanguageInput,
            CreateOrUpdateLanguageInput
            >
    {
        Task<ListResultDto<LanguageDto>> GetAllAsync();
    }
}
