using LINGYUN.Abp.LocalizationManagement.Permissions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.LocalizationManagement
{
    public class TextAppService :
        CrudAppService<
            Text,
            TextDto,
            TextDifferenceDto,
            int,
            GetTextsInput,
            CreateTextInput,
            UpdateTextInput>,
        ITextAppService
    {
        private readonly ITextRepository _textRepository;
        public TextAppService(ITextRepository repository) : base(repository)
        {
            _textRepository = repository;

            GetPolicyName = LocalizationManagementPermissions.Text.Default;
            GetListPolicyName = LocalizationManagementPermissions.Text.Default;
            CreatePolicyName = LocalizationManagementPermissions.Text.Create;
            UpdatePolicyName = LocalizationManagementPermissions.Text.Update;
            DeletePolicyName = LocalizationManagementPermissions.Text.Delete;
        }

        public virtual async Task<TextDto> GetByCultureKeyAsync(GetTextByKeyInput input)
        {
            await CheckGetPolicyAsync();

            var text = await _textRepository.GetByCultureKeyAsync(
                input.ResourceName, input.CultureName, input.Key);

            return await MapToGetOutputDtoAsync(text);
        }

        public override async Task<PagedResultDto<TextDifferenceDto>> GetListAsync(GetTextsInput input)
        {
            await CheckGetListPolicyAsync();

            var count = await _textRepository.GetDifferenceCountAsync(
                input.CultureName, input.TargetCultureName,
                input.ResourceName, input.OnlyNull, input.Filter);

            var texts = await _textRepository.GetDifferencePagedListAsync(
                input.CultureName, input.TargetCultureName,
                input.ResourceName, input.OnlyNull, input.Filter,
                input.Sorting, input.SkipCount, input.MaxResultCount);

            return new PagedResultDto<TextDifferenceDto>(count, 
                ObjectMapper.Map<List<TextDifference>, List<TextDifferenceDto>>(texts));
        }

        protected override Text MapToEntity(CreateTextInput createInput)
        {
            return new Text(
                createInput.ResourceName,
                createInput.CultureName,
                createInput.Key,
                createInput.Value);
        }

        protected override void MapToEntity(UpdateTextInput updateInput, Text entity)
        {
            entity.SetValue(updateInput.Value);
        }
    }
}
