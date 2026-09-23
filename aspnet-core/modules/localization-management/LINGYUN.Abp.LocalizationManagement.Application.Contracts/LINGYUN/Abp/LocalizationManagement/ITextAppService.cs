using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.LocalizationManagement;

public interface ITextAppService : IApplicationService
{
    Task<TextDto> GetByKeyAsync(TextGetByKeyInput input);

    Task<ListResultDto<TextDifferenceDto>> GetDifferencesAsync(TextDifferenceGetListInput input);

    Task SetTextAsync(SetTextInput input);

    Task DeleteAsync(TextDeleteInput input);

    [Obsolete("This interface will be removed in the next version. Please use DeleteAsync instead.")]
    Task RestoreToDefaultAsync(RestoreDefaultTextInput input);
}
