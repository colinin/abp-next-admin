using LINGYUN.Abp.LocalizationManagement.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.LocalizationManagement
{
    public class ResourceAppService :
        CrudAppService<
            Resource,
            ResourceDto,
            Guid,
            GetResourcesInput,
            CreateOrUpdateResourceInput,
            CreateOrUpdateResourceInput>,
        IResourceAppService
    {
        public ResourceAppService(IResourceRepository repository) : base(repository)
        {
            GetPolicyName = LocalizationManagementPermissions.Resource.Default;
            GetListPolicyName = LocalizationManagementPermissions.Resource.Default;
            CreatePolicyName = LocalizationManagementPermissions.Resource.Create;
            UpdatePolicyName = LocalizationManagementPermissions.Resource.Update;
            DeletePolicyName = LocalizationManagementPermissions.Resource.Delete;
        }

        public virtual async Task<ListResultDto<ResourceDto>> GetAllAsync()
        {
            await CheckGetListPolicyAsync();

            var resources = await Repository.GetListAsync();

            return new ListResultDto<ResourceDto>(
                ObjectMapper.Map<List<Resource>, List<ResourceDto>>(resources));
        }

        protected override Resource MapToEntity(CreateOrUpdateResourceInput createInput)
        {
            return new Resource(
                createInput.Name,
                createInput.DisplayName,
                createInput.Description)
            {
                Enable = createInput.Enable
            };
        }

        protected override void MapToEntity(CreateOrUpdateResourceInput updateInput, Resource entity)
        {
            if (!string.Equals(entity.Name, updateInput.Name, StringComparison.InvariantCultureIgnoreCase))
            {
                entity.Name = updateInput.Name;
            }
            if (!string.Equals(entity.DisplayName, updateInput.DisplayName, StringComparison.InvariantCultureIgnoreCase))
            {
                entity.DisplayName = updateInput.DisplayName;
            }
            if (!string.Equals(entity.Description, updateInput.Description, StringComparison.InvariantCultureIgnoreCase))
            {
                entity.Description = updateInput.Description;
            }
            entity.Enable = updateInput.Enable;
        }

        protected override async Task<IQueryable<Resource>> CreateFilteredQueryAsync(GetResourcesInput input)
        {
            var query = await base.CreateFilteredQueryAsync(input);

            query = query.WhereIf(!input.Filter.IsNullOrWhiteSpace(),
                x => x.Name.Contains(input.Filter) || x.DisplayName.Contains(input.Filter));

            return query;
        }
    }
}
