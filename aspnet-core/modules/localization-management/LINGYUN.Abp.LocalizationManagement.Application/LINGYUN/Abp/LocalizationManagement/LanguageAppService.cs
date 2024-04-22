using LINGYUN.Abp.LocalizationManagement.Permissions;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.ObjectMapping;

namespace LINGYUN.Abp.LocalizationManagement;

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
        if (_repository.FindByCultureNameAsync(input.CultureName) != null)
        {
            throw new BusinessException(LocalizationErrorCodes.Language.NameAlreadyExists)
                .WithData(nameof(Language.CultureName), input.CultureName);
        }

        var language = new Language(
            GuidGenerator.Create(),
            input.CultureName,
            input.UiCultureName,
            input.DisplayName,
            input.FlagIcon);

        language = await _repository.InsertAsync(language);

        await CurrentUnitOfWork.SaveChangesAsync();

        return ObjectMapper.Map<Language, LanguageDto>(language);
    }

    [Authorize(LocalizationManagementPermissions.Language.Delete)]
    public async virtual Task DeleteAsync(string name)
    {
        var language = await InternalGetByNameAsync(name);

        await _repository.DeleteAsync(language);

        await CurrentUnitOfWork.SaveChangesAsync();
    }

    [Authorize(LocalizationManagementPermissions.Language.Update)]
    public async virtual Task<LanguageDto> UpdateAsync(string name, LanguageUpdateDto input)
    {
        var language = await InternalGetByNameAsync(name);

        language.SetFlagIcon(input.FlagIcon);
        language.SetDisplayName(input.DisplayName);

        await _repository.UpdateAsync(language);

        await CurrentUnitOfWork.SaveChangesAsync();

        return ObjectMapper.Map<Language, LanguageDto>(language);
    }

    private async Task<Language> InternalGetByNameAsync(string name)
    {
        var language = await _repository.FindByCultureNameAsync(name);

        return language ?? throw new BusinessException(LocalizationErrorCodes.Language.NameNotFoundOrStaticNotAllowed)
                .WithData(nameof(Language.CultureName), name);
    }
}
