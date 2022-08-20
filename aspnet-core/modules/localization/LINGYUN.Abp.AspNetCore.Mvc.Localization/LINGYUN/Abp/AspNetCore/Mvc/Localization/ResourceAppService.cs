using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.AspNetCore.Mvc.Localization
{
    [Authorize]
    public class ResourceAppService : ApplicationService, IResourceAppService
    {
        private readonly AbpLocalizationOptions _localizationOptions;

        public ResourceAppService(
            IOptions<AbpLocalizationOptions> localizationOptions)
        {
            _localizationOptions = localizationOptions.Value;
        }

        public virtual Task<ListResultDto<ResourceDto>> GetListAsync()
        {
            var resources = _localizationOptions
                .Resources
                .Select(x => new ResourceDto
                {
                    Name = x.Value.ResourceName,
                    DisplayName = x.Value.ResourceName,
                    Description = x.Value.ResourceName,
                })
                .OrderBy(l => l.Name)
                .DistinctBy(l => l.Name);

            return Task.FromResult(new ListResultDto<ResourceDto>(resources.ToList()));
        }
    }
}
