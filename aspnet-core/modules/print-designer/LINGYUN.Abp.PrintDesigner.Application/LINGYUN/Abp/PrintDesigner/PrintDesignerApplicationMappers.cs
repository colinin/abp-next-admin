using LINGYUN.Abp.PrintDesigner.CustomElements.Dtos;
using LINGYUN.Abp.PrintDesigner.Templates.Dtos;
using LINGYUN.Abp.PrintDesigner.CustomElements;
using LINGYUN.Abp.PrintDesigner.Templates;
using Riok.Mapperly.Abstractions;
using System;
using System.Collections.Generic;
using Volo.Abp.Json;
using Volo.Abp.Mapperly;
using Volo.Abp.ObjectExtending;

namespace LINGYUN.Abp.PrintDesigner.Admin;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties(DefinitionChecks = MappingPropertyDefinitionChecks.None)]
public partial class CustomElementToCustomElementDtoMapper : MapperBase<CustomElement, CustomElementDto>
{
    private readonly IJsonSerializer _jsonSerializer;
    public CustomElementToCustomElementDtoMapper(IJsonSerializer jsonSerializer)
    {
        _jsonSerializer = jsonSerializer;
    }

    [MapperIgnoreTarget(nameof(CustomElementDto.Ext))]
    [MapperIgnoreTarget(nameof(CustomElementDto.Element))]
    [MapperIgnoreTarget(nameof(CustomElementDto.UpdatedAt))]
    [MapperIgnoreTarget(nameof(CustomElementDto.Permissions))]
    public override partial CustomElementDto Map(CustomElement source);

    [MapperIgnoreTarget(nameof(CustomElementDto.Ext))]
    [MapperIgnoreTarget(nameof(CustomElementDto.Element))]
    [MapperIgnoreTarget(nameof(CustomElementDto.UpdatedAt))]
    [MapperIgnoreTarget(nameof(CustomElementDto.Permissions))]
    public override partial void Map(CustomElement source, CustomElementDto destination);

    public override void AfterMap(CustomElement source, CustomElementDto destination)
    {
        destination.UpdatedAt = new DateTimeOffset(source.CreationTime).ToUnixTimeMilliseconds();
        destination.Element = new Dictionary<string, object?>();
        destination.Ext = new Dictionary<string, object?>();
        if (source.Data != null)
        {
            if (!source.Data.ElementData.IsNullOrWhiteSpace())
            {
                destination.Element = _jsonSerializer.Deserialize<Dictionary<string, object?>>(source.Data.ElementData);
            }
            if (!source.Data.ExtendedData.IsNullOrWhiteSpace())
            {
                destination.Ext = _jsonSerializer.Deserialize<Dictionary<string, object?>>(source.Data.ExtendedData);
            }
        }
    }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties(DefinitionChecks = MappingPropertyDefinitionChecks.None)]
public partial class TemplateToTemplateDtoMapper : MapperBase<Template, TemplateDto>
{
    private readonly IJsonSerializer _jsonSerializer;
    public TemplateToTemplateDtoMapper(IJsonSerializer jsonSerializer)
    {
        _jsonSerializer = jsonSerializer;
    }

    [MapperIgnoreTarget(nameof(TemplateDto.Data))]
    [MapperIgnoreTarget(nameof(TemplateDto.Ext))]
    [MapperIgnoreTarget(nameof(TemplateDto.UpdatedAt))]
    [MapperIgnoreTarget(nameof(TemplateDto.Permissions))]
    public override partial TemplateDto Map(Template source);

    [MapperIgnoreTarget(nameof(TemplateDto.Data))]
    [MapperIgnoreTarget(nameof(TemplateDto.Ext))]
    [MapperIgnoreTarget(nameof(TemplateDto.UpdatedAt))]
    [MapperIgnoreTarget(nameof(TemplateDto.Permissions))]
    public override partial void Map(Template source, TemplateDto destination);

    public override void AfterMap(Template source, TemplateDto destination)
    {
        destination.UpdatedAt = new DateTimeOffset(source.CreationTime).ToUnixTimeMilliseconds();
        destination.Data = new Dictionary<string, object?>();
        destination.Ext = new Dictionary<string, object?>();
        if (source.Data != null)
        {
            if (!source.Data.DesignerData.IsNullOrWhiteSpace())
            {
                destination.Data = _jsonSerializer.Deserialize<Dictionary<string, object?>>(source.Data.DesignerData);
            }
            if (!source.Data.ExtendedData.IsNullOrWhiteSpace())
            {
                destination.Ext = _jsonSerializer.Deserialize<Dictionary<string, object?>>(source.Data.ExtendedData);
            }
        }
    }
}
