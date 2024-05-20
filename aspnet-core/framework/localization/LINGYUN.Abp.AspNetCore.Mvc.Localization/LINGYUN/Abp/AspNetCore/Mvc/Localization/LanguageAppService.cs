using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
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

        public async virtual Task<ListResultDto<LanguageDto>> GetListAsync(GetLanguageWithFilterDto input)
        {
            var languages = (await _languageProvider.GetLanguagesAsync())
                .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x => x.CultureName.IndexOf(input.Filter, StringComparison.OrdinalIgnoreCase) >= 0
                         || x.UiCultureName.IndexOf(input.Filter, StringComparison.OrdinalIgnoreCase) >= 0
                         || x.DisplayName.IndexOf(input.Filter, StringComparison.OrdinalIgnoreCase) >= 0);

            return new ListResultDto<LanguageDto>(
                languages.Select(l => new LanguageDto
                {
                    CultureName = l.CultureName,
                    UiCultureName = l.UiCultureName,
                    DisplayName = l.DisplayName,
                    FlagIcon = l.FlagIcon
                })
                .OrderBy(l => l.CultureName)
                .DistinctBy(l => l.CultureName)
                .ToList());
        }
    }
}
