using LINGYUN.Abp.BlobManagement.Dtos;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Specifications;

namespace LINGYUN.Abp.BlobManagement;

public class BlobContainerAppService : BlobManagementApplicationService, IBlobContainerAppService
{
    private readonly AbpBlobManagementOptions _options;

    private readonly IBlobContainerRepository _blobContainerRepository;
    private readonly BlobManager _blobManager;
    public BlobContainerAppService(
        IOptions<AbpBlobManagementOptions> options,
        IBlobContainerRepository blobContainerRepository,
        BlobManager blobManager)
    {
        _blobContainerRepository = blobContainerRepository;
        _blobManager = blobManager;
        _options = options.Value;
    }

    public async virtual Task<BlobContainerDto> CreateAsync(BlobContainerCreateDto input)
    {
        var blobContainer = await _blobManager.CreateContainerAsync(input.Name);

        return ObjectMapper.Map<BlobContainer, BlobContainerDto>(blobContainer);
    }

    public async virtual Task DeleteAsync(Guid id)
    {
        var blobContainer = await _blobContainerRepository.GetAsync(id);

        if (_options.StaticContainers.Contains(blobContainer.Name))
        {
            throw new BusinessException(
                BlobManagementErrorCodes.Container.DeleteWithStatic,
                "The blob container is the system container and cannot be deleted!");
        }

        await _blobManager.DeleteContainerAsync(blobContainer);
    }

    public async virtual Task<BlobContainerDto> GetAsync(Guid id)
    {
        var blobContainer = await _blobContainerRepository.GetAsync(id);

        return ObjectMapper.Map<BlobContainer, BlobContainerDto>(blobContainer);
    }

    public async virtual Task<PagedResultDto<BlobContainerDto>> GetListAsync(BlobContainerGetPagedListInput input)
    {
        Expression<Func<BlobContainer, bool>> expression = x => x.Provider == _blobManager.GetBlobProvider();

        if (!input.Filter.IsNullOrWhiteSpace())
        {
            expression = PredicateBuilder.And(expression, x => x.Name.Contains(input.Filter));
        }

        var specification = new ExpressionSpecification<BlobContainer>(expression);

        var totalCount = await _blobContainerRepository.GetCountAsync(specification);
        var blobContainers = await _blobContainerRepository.GetListAsync(specification,
            input.Sorting, input.MaxResultCount, input.SkipCount);

        return new PagedResultDto<BlobContainerDto>(totalCount,
            ObjectMapper.Map<List<BlobContainer>, List<BlobContainerDto>>(blobContainers));
    }
}
