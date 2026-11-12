using LINGYUN.Abp.PrintDesigner.Permissions;
using LINGYUN.Abp.PrintDesigner.Templates.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;
using Volo.Abp.Json;

namespace LINGYUN.Abp.PrintDesigner.Templates;

[Authorize(PrintDesignerPermissions.Template.Default)]
public class TemplateAppService : PrintDesignerAppServiceBase, ITemplateAppService
{
    private readonly IJsonSerializer _jsonSerializer;
    private readonly ITemplateRepository _templateRepository;

    public TemplateAppService(
        IJsonSerializer jsonSerializer,
        ITemplateRepository templateRepository)
    {
        _jsonSerializer = jsonSerializer;
        _templateRepository = templateRepository;
    }

    [Authorize(PrintDesignerPermissions.Template.Create)]
    public async virtual Task<TemplateDto> CreateAsync(TemplateCreateDto input)
    {
        if (await _templateRepository.FindByNameAsync(input.Name) != null)
        {
            throw new BusinessException(
                PrintDesignerErrorCodes.Template.NameAlreadyExists,
                $"A template definition named {input.Name} already exists.!")
                .WithData("Name", input.Name);
        }

        var template = new Template(
            GuidGenerator.Create(),
            input.Name,
            input.Description,
            CurrentTenant.Id);

        var elementData = _jsonSerializer.Serialize(input.Data);
        var extendedData = _jsonSerializer.Serialize(input.Ext);
        template.SetData(elementData, extendedData);

        template = await _templateRepository.InsertAsync(template);

        await CurrentUnitOfWork!.SaveChangesAsync();

        var templateDto = ObjectMapper.Map<Template, TemplateDto>(template);

        await SetTemplatePermissions(templateDto);

        return templateDto;
    }

    [Authorize(PrintDesignerPermissions.Template.Delete)]
    public async virtual Task<TemplateDeleteDto> DeleteAsync(Guid id)
    {
        bool success;
        try
        {
            var template = await _templateRepository.GetAsync(id);

            await _templateRepository.DeleteAsync(template);

            success = true;
        }
        catch (Exception ex)
        {
            Logger.LogWarning(ex, "Delete template: {id} failed: {message}!", id, ex.Message);
            success = false;
        }

        return new TemplateDeleteDto
        {
            Success = success,
        };
    }

    public async virtual Task<TemplateDto> GetAsync(Guid id)
    {
        var template = await _templateRepository.GetAsync(id);

        var templateDto = ObjectMapper.Map<Template, TemplateDto>(template);

        await SetTemplatePermissions(templateDto);

        return templateDto;
    }

    public async virtual Task<TemplateDto> GetByNameAsync(string name)
    {
        var template = await _templateRepository.FindByNameAsync(name, includeDetails: true)
            ?? throw new BusinessException(PrintDesignerErrorCodes.Template.NameNotFound)
                .WithData("Name", name);

        var templateDto = ObjectMapper.Map<Template, TemplateDto>(template);

        await SetTemplatePermissions(templateDto);

        return templateDto;
    }

    public async virtual Task<ListResultDto<TemplateDto>> GetAllAsync()
    {
        var templates = await _templateRepository.GetListAsync();

        var templateDtos = ObjectMapper.Map<List<Template>, List<TemplateDto>>(templates);

        await SetTemplatesPermissions(templateDtos);

        return new ListResultDto<TemplateDto>(templateDtos);
    }

    public async virtual Task<PagedResultDto<TemplateDto>> GetListAsync(TemplateGetPagedListInput input)
    {
        var specification = new TemplateGetPagedListSpecification(input);

        var totalCount = await _templateRepository.GetCountAsync(specification);
        var templates = await _templateRepository.GetListAsync(specification,
            input.Sorting, input.MaxResultCount, input.SkipCount);

        var templateDtos = ObjectMapper.Map<List<Template>, List<TemplateDto>>(templates);

        await SetTemplatesPermissions(templateDtos);

        return new PagedResultDto<TemplateDto>(totalCount, templateDtos);
    }

    [Authorize(PrintDesignerPermissions.Template.Update)]
    public async virtual Task<TemplateDto> UpdateAsync(Guid id, TemplateUpdateDto input)
    {
        var template = await _templateRepository.GetAsync(id);
        if (!string.Equals(template.Name, input.Name, StringComparison.CurrentCultureIgnoreCase))
        {
            if (await _templateRepository.FindByNameAsync(input.Name) != null)
            {
                throw new BusinessException(
                    PrintDesignerErrorCodes.Template.NameAlreadyExists,
                    $"A template definition named {input.Name} already exists.!")
                    .WithData("Name", input.Name);
            }
            template.SetName(input.Name);
        }
        var elementData = _jsonSerializer.Serialize(input.Data);
        var extendedData = _jsonSerializer.Serialize(input.Ext);
        template.SetData(elementData, extendedData);
        template.SetConcurrencyStampIfNotNull(input.ConcurrencyStamp);

        template = await _templateRepository.UpdateAsync(template);

        await CurrentUnitOfWork!.SaveChangesAsync();

        var templateDto = ObjectMapper.Map<Template, TemplateDto>(template);

        await SetTemplatePermissions(templateDto);

        return templateDto;
    }

    public async virtual Task<TemplateDto> UpsertAsync(TemplateUpsertDto input)
    {
        if (input.Id.HasValue &&
            await _templateRepository.FindAsync(input.Id.Value) != null)
        {
            await AuthorizationService.CheckAsync(PrintDesignerPermissions.Template.Update);

            return await UpdateAsync(
                input.Id.Value,
                new TemplateUpdateDto
                {
                    Name = input.Name,
                    Data = input.Data,
                    Ext = input.Ext,
                    Description = input.Description,
                    ConcurrencyStamp = input.ConcurrencyStamp,
                });
        }
        else
        {
            await AuthorizationService.CheckAsync(PrintDesignerPermissions.Template.Create);

            return await CreateAsync(
                new TemplateCreateDto
                {
                    Name = input.Name,
                    Data = input.Data,
                    Ext = input.Ext,
                    Description = input.Description,
                });
        }
    }

    protected async virtual Task SetTemplatePermissions(TemplateDto templateDto)
    {
        templateDto.Permissions ??= new TemplatePermissionDto();
        templateDto.Permissions.Editable = await AuthorizationService.IsGrantedAsync(PrintDesignerPermissions.Template.Update);
        templateDto.Permissions.Deletable = await AuthorizationService.IsGrantedAsync(PrintDesignerPermissions.Template.Delete);
        templateDto.Permissions.Copyable = await AuthorizationService.IsGrantedAsync(PrintDesignerPermissions.Template.Copy);
    }

    protected async virtual Task SetTemplatesPermissions(List<TemplateDto> templateDtos)
    {
        var editable = await AuthorizationService.IsGrantedAsync(PrintDesignerPermissions.Template.Update);
        var deletable = await AuthorizationService.IsGrantedAsync(PrintDesignerPermissions.Template.Delete);
        var copyable = await AuthorizationService.IsGrantedAsync(PrintDesignerPermissions.Template.Copy);

        foreach (var templateDto in templateDtos)
        {
            templateDto.Permissions ??= new TemplatePermissionDto();
            templateDto.Permissions.Editable = editable;
            templateDto.Permissions.Deletable = deletable;
            templateDto.Permissions.Copyable = copyable;
        }
    }
}
