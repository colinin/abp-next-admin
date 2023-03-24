using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.AspNetCore.Mvc.Localization
{
    [Authorize]
    public class TextAppService : ApplicationService, ITextAppService
    {
        private readonly AbpLocalizationOptions _localizationOptions;
        private readonly IStringLocalizerFactory _localizerFactory;
        public TextAppService(
            IStringLocalizerFactory stringLocalizerFactory,
            IOptions<AbpLocalizationOptions> localizationOptions)
        {
            _localizerFactory = stringLocalizerFactory;
            _localizationOptions = localizationOptions.Value;
        }

        public async virtual Task<TextDto> GetByCultureKeyAsync(GetTextByKeyInput input)
        {
            var resource = _localizationOptions.Resources.GetOrDefault(input.ResourceName);

            IEnumerable<LocalizedString> localizedStrings = new List<LocalizedString>();
            var localizer = await _localizerFactory.CreateByResourceNameAsync(resource.ResourceName);

            using (CultureHelper.Use(input.CultureName, input.CultureName))
            {
                localizedStrings = localizer.GetAllStrings(true)
                    .OrderBy(l => l.Name);

                var result = new TextDto
                {
                    Key = input.Key,
                    CultureName = input.CultureName,
                    ResourceName = input.ResourceName,
                    Value = localizer[input.Key]?.Value
                };

                return result;
            }
        }

        public async virtual Task<ListResultDto<TextDifferenceDto>> GetListAsync(GetTextsInput input)
        {
            var result = new List<TextDifferenceDto>();

            if (input.ResourceName.IsNullOrWhiteSpace())
            {
                var filterResources = _localizationOptions.Resources
                    .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x => x.Value.ResourceName.Contains(input.Filter))
                    .OrderBy(r => r.Value.ResourceName);

                foreach (var resource in filterResources)
                {
                    result.AddRange(
                        await GetTextDifferences(resource.Value, input.CultureName, input.TargetCultureName, input.Filter, input.OnlyNull));
                }
            }
            else
            {
                var resource = _localizationOptions.Resources
                    .Where(l => l.Value.ResourceName.Equals(input.ResourceName))
                    .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x => x.Value.ResourceName.Contains(input.Filter))
                    .Select(l => l.Value)
                    .FirstOrDefault();
                if (resource != null)
                {
                    result.AddRange(
                        await GetTextDifferences(resource, input.CultureName, input.TargetCultureName, input.Filter, input.OnlyNull));
                }
            }

            return new ListResultDto<TextDifferenceDto>(result);
        }

        protected async virtual Task<IEnumerable<TextDifferenceDto>> GetTextDifferences(
            LocalizationResourceBase resource,
            string cultureName,
            string targetCultureName,
            string filter = null,
            bool? onlyNull = null)
        {
            var result = new List<TextDifferenceDto>();

            IEnumerable<LocalizedString> localizedStrings = new List<LocalizedString>();
            IEnumerable<LocalizedString> targetLocalizedStrings = new List<LocalizedString>();
            var localizer = await _localizerFactory.CreateByResourceNameAsync(resource.ResourceName);

            using (CultureHelper.Use(cultureName, cultureName))
            {
                localizedStrings = localizer.GetAllStrings(true)
                    .WhereIf(!filter.IsNullOrWhiteSpace(), x => x.Name.Contains(filter))
                    .OrderBy(l => l.Name);
            }

            if (Equals(cultureName, targetCultureName))
            {
                targetLocalizedStrings = localizedStrings;
            }
            else
            {
                using (CultureHelper.Use(targetCultureName, targetCultureName))
                {
                    targetLocalizedStrings = localizer.GetAllStrings(true)
                        .WhereIf(!filter.IsNullOrWhiteSpace(), x => x.Name.Contains(filter))
                        .OrderBy(l => l.Name);
                }
            }

            foreach (var localizedString in localizedStrings)
            {
                var targetLocalizedString = targetLocalizedStrings.FirstOrDefault(l => l.Name.Equals(localizedString.Name));
                if (onlyNull == true)
                {
                    if (targetLocalizedString == null || targetLocalizedString.Value.IsNullOrWhiteSpace())
                    {
                        result.Add(new TextDifferenceDto
                        {
                            CultureName = cultureName,
                            TargetCultureName = targetCultureName,
                            Key = localizedString.Name,
                            Value = localizedString.Value,
                            TargetValue = null,
                            ResourceName = resource.ResourceName
                        });
                    }
                }
                else
                {
                    result.Add(new TextDifferenceDto
                    {
                        CultureName = cultureName,
                        TargetCultureName = targetCultureName,
                        Key = localizedString.Name,
                        Value = localizedString.Value,
                        TargetValue = targetLocalizedString?.Value,
                        ResourceName = resource.ResourceName
                    });
                }
            }

            return result;
        }
    }
}
