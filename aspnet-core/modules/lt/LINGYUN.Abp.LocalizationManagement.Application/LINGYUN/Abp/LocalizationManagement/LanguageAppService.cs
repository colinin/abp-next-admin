using LINGYUN.Abp.LocalizationManagement.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.LocalizationManagement
{
    public class LanguageAppService :
        CrudAppService<
            Language,
            LanguageDto,
            Guid,
            GetLanguagesInput,
            CreateOrUpdateLanguageInput,
            CreateOrUpdateLanguageInput>,
        ILanguageAppService
    {
        public LanguageAppService(ILanguageRepository repository) : base(repository)
        {
            GetPolicyName = LocalizationManagementPermissions.Language.Default;
            GetListPolicyName = LocalizationManagementPermissions.Language.Default;
            CreatePolicyName = LocalizationManagementPermissions.Language.Create;
            UpdatePolicyName = LocalizationManagementPermissions.Language.Update;
            DeletePolicyName = LocalizationManagementPermissions.Language.Delete;
        }

        public virtual async Task<ListResultDto<LanguageDto>> GetAllAsync()
        {
            await CheckGetListPolicyAsync();

            var languages = await Repository.GetListAsync();

            return new ListResultDto<LanguageDto>(
                ObjectMapper.Map<List<Language>, List<LanguageDto>>(languages));
        }

        protected override Language MapToEntity(CreateOrUpdateLanguageInput createInput)
        {
            return new Language(
                createInput.CultureName,
                createInput.UiCultureName,
                createInput.DisplayName,
                createInput.FlagIcon)
            {
                Enable = createInput.Enable
            };
        }

        protected override void MapToEntity(CreateOrUpdateLanguageInput updateInput, Language entity)
        {
            if (!string.Equals(entity.FlagIcon, updateInput.FlagIcon, StringComparison.InvariantCultureIgnoreCase))
            {
                entity.FlagIcon = updateInput.FlagIcon;
            }
            entity.ChangeCulture(updateInput.CultureName, updateInput.UiCultureName, updateInput.DisplayName);
            entity.Enable = updateInput.Enable;
        }

        protected override async Task<IQueryable<Language>> CreateFilteredQueryAsync(GetLanguagesInput input)
        {
            var query = await base.CreateFilteredQueryAsync(input);

            query = query.WhereIf(!input.Filter.IsNullOrWhiteSpace(),
                x => x.CultureName.Contains(input.Filter) || x.UiCultureName.Contains(input.Filter) ||
                     x.DisplayName.Contains(input.Filter));

            return query;
        }
    }
}
