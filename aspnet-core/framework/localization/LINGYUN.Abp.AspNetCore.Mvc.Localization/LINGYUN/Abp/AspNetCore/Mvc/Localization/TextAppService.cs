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
using Volo.Abp.Localization.External;

namespace LINGYUN.Abp.AspNetCore.Mvc.Localization
{
    [Authorize]
    public class TextAppService : ApplicationService, ITextAppService
    {
        private readonly AbpLocalizationOptions _localizationOptions;
        private readonly IStringLocalizerFactory _localizerFactory;
        private readonly IExternalLocalizationStore _externalLocalizationStore;
        public TextAppService(
            IStringLocalizerFactory stringLocalizerFactory,
            IExternalLocalizationStore externalLocalizationStore,
            IOptions<AbpLocalizationOptions> localizationOptions)
        {
            _localizerFactory = stringLocalizerFactory;
            _externalLocalizationStore = externalLocalizationStore;
            _localizationOptions = localizationOptions.Value;
        }

        public async virtual Task<TextDto> GetByCultureKeyAsync(GetTextByKeyInput input)
        {
            var localizer = await _localizerFactory.CreateByResourceNameAsync(input.ResourceName);

            using (CultureHelper.Use(input.CultureName, input.CultureName))
            {
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
                    .Select(r => r.Value)
                    .Union(await _externalLocalizationStore.GetResourcesAsync())
                    .DistinctBy(r => r.ResourceName)
                    .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x => x.ResourceName.Contains(input.Filter))
                    .OrderBy(r => r.ResourceName);

                foreach (var resource in filterResources)
                {
                    result.AddRange(
                        await GetTextDifferences(resource, input.CultureName, input.TargetCultureName, input.Filter, input.OnlyNull));
                }
            }
            else
            {
                var resource = _localizationOptions.Resources
                    .Select(r => r.Value)
                    .Union(await _externalLocalizationStore.GetResourcesAsync())
                    .DistinctBy(r => r.ResourceName)
                    .Where(l => l.ResourceName.Equals(input.ResourceName))
                    .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x => x.ResourceName.Contains(input.Filter))
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
                localizedStrings = (await localizer.GetAllStringsAsync(true))
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
                    targetLocalizedStrings = (await localizer.GetAllStringsAsync(true))
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
