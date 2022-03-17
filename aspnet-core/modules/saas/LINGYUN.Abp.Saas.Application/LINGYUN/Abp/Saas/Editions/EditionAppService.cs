using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Data;

namespace LINGYUN.Abp.Saas.Editions;

[Authorize(AbpSaasPermissions.Editions.Default)]
public class EditionAppService : AbpSaasAppServiceBase, IEditionAppService
{
    protected EditionManager EditionManager { get; }
    protected IEditionRepository EditionRepository { get; }

    public EditionAppService(
        EditionManager editionManager,
        IEditionRepository editionRepository)
    {
        EditionManager = editionManager;
        EditionRepository = editionRepository;
    }

    [Authorize(AbpSaasPermissions.Editions.Create)]
    public async virtual Task<EditionDto> CreateAsync(EditionCreateDto input)
    {
        var edition = await EditionManager.CreateAsync(input.DisplayName);
        input.MapExtraPropertiesTo(edition);

        await EditionRepository.InsertAsync(edition);

        await CurrentUnitOfWork.SaveChangesAsync();

        return ObjectMapper.Map<Edition, EditionDto>(edition);
    }

    [Authorize(AbpSaasPermissions.Editions.Delete)]
    public async virtual Task DeleteAsync(Guid id)
    {
        var edition = await EditionRepository.GetAsync(id);

        await EditionManager.DeleteAsync(edition);
    }

    public async virtual Task<EditionDto> GetAsync(Guid id)
    {
        var edition = await EditionRepository.GetAsync(id, false);

        return ObjectMapper.Map<Edition, EditionDto>(edition);
    }

    public async virtual Task<PagedResultDto<EditionDto>> GetListAsync(EditionGetListInput input)
    {
        var totalCount = await EditionRepository.GetCountAsync(input.Filter);
        var editions = await EditionRepository.GetListAsync(
            input.Sorting,
            input.MaxResultCount,
            input.SkipCount,
            input.Filter
        );

        return new PagedResultDto<EditionDto>(
            totalCount,
            ObjectMapper.Map<List<Edition>, List<EditionDto>>(editions)
        );
    }

    [Authorize(AbpSaasPermissions.Editions.Update)]
    public async virtual Task<EditionDto> UpdateAsync(Guid id, EditionUpdateDto input)
    {
        var edition = await EditionRepository.GetAsync(id, false);

        if (!string.Equals(edition.DisplayName, input.DisplayName))
        {
            await EditionManager.ChangeDisplayNameAsync(edition, input.DisplayName);
        }
        edition.SetConcurrencyStampIfNotNull(input.ConcurrencyStamp);
        input.MapExtraPropertiesTo(edition);

        await EditionRepository.UpdateAsync(edition);

        await CurrentUnitOfWork.SaveChangesAsync();

        return ObjectMapper.Map<Edition, EditionDto>(edition);
    }
}
