using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Localization;
using Volo.Abp.Localization.External;

namespace LINGYUN.Abp.AspNetCore.Mvc.Localization
{
    [Authorize]
    public class ResourceAppService : ApplicationService, IResourceAppService
    {
        private readonly IExternalLocalizationStore _externalLocalizationStore;
        private readonly AbpLocalizationOptions _localizationOptions;

        public ResourceAppService(
            IOptions<AbpLocalizationOptions> localizationOptions,
            IExternalLocalizationStore externalLocalizationStore)
        {
            _localizationOptions = localizationOptions.Value;
            _externalLocalizationStore = externalLocalizationStore;
        }

        public virtual async Task<ListResultDto<ResourceDto>> GetListAsync(GetResourceWithFilterDto input)
        {
            var externalResources = (await _externalLocalizationStore.GetResourcesAsync())
                .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x => x.ResourceName.Contains(input.Filter, StringComparison.OrdinalIgnoreCase));

            var resources = _localizationOptions
                .Resources
                .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x => x.Value.ResourceName.Contains(input.Filter, StringComparison.OrdinalIgnoreCase))
                .Select(x => new ResourceDto
                {
                    Name = x.Value.ResourceName,
                    DisplayName = x.Value.ResourceName,
                    Description = x.Value.ResourceName,
                })
                .Union(externalResources.Select(resource => new ResourceDto
                {
                    Name = resource.ResourceName,
                    DisplayName = resource.ResourceName,
                    Description = resource.ResourceName,
                }))
                .OrderBy(l => l.Name)
                .DistinctBy(l => l.Name);

            return new ListResultDto<ResourceDto>(resources.ToList());
        }
    }
}
