using LINGYUN.Abp.LocalizationManagement.Features;
using LINGYUN.Abp.LocalizationManagement.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Features;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.LocalizationManagement;

[RequiresFeature(LocalizationManagementFeatures.Enable)]
[Authorize(LocalizationManagementPermissions.Language.Default)]
public class LanguageAppService : LocalizationAppServiceBase, ILanguageAppService
{

    private readonly ILanguageRepository _repository;

    public LanguageAppService(ILanguageRepository repository)
    {
        _repository = repository;
    }

    public async virtual Task<LanguageDto> GetByNameAsync(string name)
    {
        var language = await InternalGetByNameAsync(name);

        return ObjectMapper.Map<Language, LanguageDto>(language);
    }

    [Authorize(LocalizationManagementPermissions.Language.Create)]
    public async virtual Task<LanguageDto> CreateAsync(LanguageCreateDto input)
    {
        if (await _repository.FindByCultureNameAsync(input.CultureName) != null)
        {
            throw new BusinessException(LocalizationErrorCodes.Language.NameAlreadyExists)
                .WithData(nameof(Language.CultureName), input.CultureName);
        }

        using (CultureHelper.Use(input.CultureName, input.UiCultureName))
        {

            var language = new Language(
                GuidGenerator.Create(),
                input.CultureName,
                input.UiCultureName,
                input.DisplayName,
                CultureInfo.CurrentCulture.TwoLetterISOLanguageName);

            language = await _repository.InsertAsync(language);

            await PublishDynamicLocalizationRefreshEvent(new DynamicLanguageRefreshEventData(language.CultureName));

            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<Language, LanguageDto>(language);
        }
    }

    [Authorize(LocalizationManagementPermissions.Language.Delete)]
    public async virtual Task DeleteAsync(string name)
    {
        var language = await InternalGetByNameAsync(name);

        await _repository.DeleteAsync(language);

        await PublishDynamicLocalizationRefreshEvent(new DynamicLanguageRefreshEventData(language.CultureName));

        await CurrentUnitOfWork.SaveChangesAsync();
    }

    [Authorize(LocalizationManagementPermissions.Language.Update)]
    public async virtual Task<LanguageDto> UpdateAsync(string name, LanguageUpdateDto input)
    {
        var language = await InternalGetByNameAsync(name);

        language.SetDisplayName(input.DisplayName);

        await _repository.UpdateAsync(language);

        await PublishDynamicLocalizationRefreshEvent(new DynamicLanguageRefreshEventData(language.CultureName));

        await CurrentUnitOfWork.SaveChangesAsync();

        return ObjectMapper.Map<Language, LanguageDto>(language);
    }

    public async virtual Task<PagedResultDto<LanguageDto>> GetListAsync(LanguageGetPagedListInput input)
    {
        Expression<Func<Language, bool>> predicate = _ => true;
        if (!input.Filter.IsNullOrWhiteSpace())
        {
            predicate = predicate.And(x => x.CultureName.Contains(input.Filter) ||
                x.UiCultureName.Contains(input.Filter) || x.DisplayName.Contains(input.Filter));
        }

        var specification = new Volo.Abp.Specifications.ExpressionSpecification<Language>(predicate);
        var totalCount = await _repository.GetCountAsync(specification);
        var languages = await _repository.GetListAsync(specification,
            input.Sorting, input.MaxResultCount, input.SkipCount);

        return new PagedResultDto<LanguageDto>(totalCount,
            ObjectMapper.Map<List<Language>, List<LanguageDto>>(languages));
    }

    private async Task<Language> InternalGetByNameAsync(string name)
    {
        var language = await _repository.FindByCultureNameAsync(name);

        return language ?? throw new BusinessException(LocalizationErrorCodes.Language.NameNotFoundOrStaticNotAllowed)
                .WithData(nameof(Language.CultureName), name);
    }
}
