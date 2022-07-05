using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.AspNetCore.Mvc.Localization
{
    [Authorize]
    public class LanguageAppService : ApplicationService, ILanguageAppService
    {
        private readonly ILanguageProvider _languageProvider;
        public LanguageAppService(ILanguageProvider languageProvider)
        {
            _languageProvider = languageProvider;
        }

        public virtual async Task<ListResultDto<LanguageDto>> GetListAsync()
        {
            var languages = await _languageProvider.GetLanguagesAsync();

            return new ListResultDto<LanguageDto>(
                languages.Select(l => new LanguageDto
                {
                    CultureName = l.CultureName,
                    UiCultureName = l.UiCultureName,
                    DisplayName = l.DisplayName,
                    FlagIcon = l.FlagIcon
                }).ToList());
        }
    }
}
