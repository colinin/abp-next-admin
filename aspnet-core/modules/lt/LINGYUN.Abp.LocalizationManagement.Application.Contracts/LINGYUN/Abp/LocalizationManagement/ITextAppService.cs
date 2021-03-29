using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.LocalizationManagement
{
    public interface ITextAppService : 
        ICrudAppService<
            TextDto,
            TextDifferenceDto,
            int,
            GetTextsInput,
            CreateTextInput,
            UpdateTextInput>
    {
        Task<TextDto> GetByCultureKeyAsync(GetTextByKeyInput input);
    }
}
