using LINGYUN.Abp.PrintDesigner.CustomElements.Dtos;
using LINGYUN.Abp.PrintDesigner.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;
using Volo.Abp.Json;

namespace LINGYUN.Abp.PrintDesigner.CustomElements;

[Authorize(PrintDesignerPermissions.CustomElement.Default)]
public class CustomElementAppService : PrintDesignerAppServiceBase, ICustomElementAppService
{
    private readonly IJsonSerializer _jsonSerializer;
    private readonly ICustomElementRepository _customElementRepository;

    public CustomElementAppService(
        IJsonSerializer jsonSerializer,
        ICustomElementRepository customElementRepository)
    {
        _jsonSerializer = jsonSerializer;
        _customElementRepository = customElementRepository;
    }

    [Authorize(PrintDesignerPermissions.CustomElement.Create)]
    public async virtual Task<CustomElementDto> CreateAsync(CustomElementCreateDto input)
    {
        if (await _customElementRepository.FindByNameAsync(input.Name) != null)
        {
            throw new BusinessException(
                PrintDesignerErrorCodes.CustomElement.NameAlreadyExists,
                $"A custom element definition named {input.Name} already exists.!")
                .WithData("Name", input.Name);
        }

        var customElement = new CustomElement(
            GuidGenerator.Create(),
            input.Name,
            input.Description,
            CurrentTenant.Id);

        var elementData = _jsonSerializer.Serialize(input.Element);
        var extendedData = _jsonSerializer.Serialize(input.Ext);
        customElement.SetData(elementData, extendedData);

        customElement = await _customElementRepository.InsertAsync(customElement);

        await CurrentUnitOfWork!.SaveChangesAsync();

        var customElementDto = ObjectMapper.Map<CustomElement, CustomElementDto>(customElement);

        await SetCustomElementPermissions(customElementDto);

        return customElementDto;
    }

    [Authorize(PrintDesignerPermissions.CustomElement.Delete)]
    public async virtual Task<CustomElementDeleteDto> DeleteAsync(Guid id)
    {
        bool success;
        try
        {
            var customElement = await _customElementRepository.GetAsync(id);

            await _customElementRepository.DeleteAsync(customElement);

            success = true;
        }
        catch (Exception ex)
        {
            Logger.LogWarning(ex, "Deleting custom element: {id} failed: {message}!", id, ex.Message);
            success = false;
        }

        return new CustomElementDeleteDto
        {
            Success = success,
        };
    }

    public async virtual Task<CustomElementDto> GetAsync(Guid id)
    {
        var customElement = await _customElementRepository.GetAsync(id);

        var customElementDto = ObjectMapper.Map<CustomElement, CustomElementDto>(customElement);

        await SetCustomElementPermissions(customElementDto);

        return customElementDto;
    }

    public async virtual Task<ListResultDto<CustomElementDto>> GetAllAsync()
    {
        var customElements = await _customElementRepository.GetListAsync();

        var customElementDtos = ObjectMapper.Map<List<CustomElement>, List<CustomElementDto>>(customElements);

        await SetCustomElementsPermissions(customElementDtos);

        return new ListResultDto<CustomElementDto>(customElementDtos);
    }

    public async virtual Task<PagedResultDto<CustomElementDto>> GetListAsync(CustomElementGetPagedListInput input)
    {
        var specification = new CustomElementGetPagedListSpecification(input);

        var totalCount = await _customElementRepository.GetCountAsync(specification);
        var customElements = await _customElementRepository.GetListAsync(specification,
            input.Sorting, input.MaxResultCount, input.SkipCount);

        var customElementDtos = ObjectMapper.Map<List<CustomElement>, List<CustomElementDto>>(customElements);

        await SetCustomElementsPermissions(customElementDtos);

        return new PagedResultDto<CustomElementDto>(totalCount, customElementDtos);
    }

    [Authorize(PrintDesignerPermissions.CustomElement.Update)]
    public async virtual Task<CustomElementDto> UpdateAsync(Guid id, CustomElementUpdateDto input)
    {
        var customElement = await _customElementRepository.GetAsync(id);
        if (!string.Equals(customElement.Name, input.Name, StringComparison.CurrentCultureIgnoreCase))
        {
            if (await _customElementRepository.FindByNameAsync(input.Name) != null)
            {
                throw new BusinessException(
                    PrintDesignerErrorCodes.CustomElement.NameAlreadyExists,
                    $"A custom element definition named {input.Name} already exists.!")
                    .WithData("Name", input.Name);
            }
            customElement.SetName(input.Name);
        }
        var elementData = _jsonSerializer.Serialize(input.Element);
        var extendedData = _jsonSerializer.Serialize(input.Ext);
        customElement.SetData(elementData, extendedData);
        customElement.SetConcurrencyStampIfNotNull(input.ConcurrencyStamp);

        customElement = await _customElementRepository.UpdateAsync(customElement);

        await CurrentUnitOfWork!.SaveChangesAsync();

        var customElementDto = ObjectMapper.Map<CustomElement, CustomElementDto>(customElement);

        await SetCustomElementPermissions(customElementDto);

        return customElementDto;
    }

    public async virtual Task<CustomElementDto> UpsertAsync(CustomElementUpsertDto input)
    {
        if (input.Id.HasValue && 
            await _customElementRepository.FindAsync(input.Id.Value) != null)
        {
            await AuthorizationService.CheckAsync(PrintDesignerPermissions.CustomElement.Update);

            return await UpdateAsync(
                input.Id.Value,
                new CustomElementUpdateDto
                {
                    Name = input.Name,
                    Description = input.Description,
                    Element = input.Element,
                    Ext = input.Ext,
                    ConcurrencyStamp = input.ConcurrencyStamp,
                });
        }
        else
        {
            await AuthorizationService.CheckAsync(PrintDesignerPermissions.CustomElement.Create);

            return await CreateAsync(
                new CustomElementCreateDto
                {
                    Name = input.Name,
                    Description = input.Description,
                    Element = input.Element,
                    Ext = input.Ext,
                });
        }
    }

    protected async virtual Task SetCustomElementPermissions(CustomElementDto customElementDto)
    {
        customElementDto.Permissions ??= new CustomElementPermissionDto();
        customElementDto.Permissions.Editable = await AuthorizationService.IsGrantedAsync(PrintDesignerPermissions.CustomElement.Update);
        customElementDto.Permissions.Deletable = await AuthorizationService.IsGrantedAsync(PrintDesignerPermissions.CustomElement.Delete);
        customElementDto.Permissions.Copyable = await AuthorizationService.IsGrantedAsync(PrintDesignerPermissions.CustomElement.Copy);
    }

    protected async virtual Task SetCustomElementsPermissions(List<CustomElementDto> customElementDtos)
    {
        var editable = await AuthorizationService.IsGrantedAsync(PrintDesignerPermissions.CustomElement.Update);
        var deletable = await AuthorizationService.IsGrantedAsync(PrintDesignerPermissions.CustomElement.Delete);
        var copyable = await AuthorizationService.IsGrantedAsync(PrintDesignerPermissions.CustomElement.Copy);

        foreach (var customElementDto in customElementDtos)
        {
            customElementDto.Permissions ??= new CustomElementPermissionDto();
            customElementDto.Permissions.Editable = editable;
            customElementDto.Permissions.Deletable = deletable;
            customElementDto.Permissions.Copyable = copyable;
        }
    }
}
