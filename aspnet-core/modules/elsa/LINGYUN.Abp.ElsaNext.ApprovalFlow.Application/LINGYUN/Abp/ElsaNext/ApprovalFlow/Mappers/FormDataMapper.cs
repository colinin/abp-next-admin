using LINGYUN.Abp.ElsaNext.ApprovalFlow.FormDatas;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.FormDefinitions;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.FormDefinitions.Dtos;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Mappers;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class FormFieldMapper : MapperBase<FormFieldDefinition, FormFieldDto>
{
    public override partial FormFieldDto Map(FormFieldDefinition source);

    public override partial void Map(FormFieldDefinition source, FormFieldDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class FormDataMapper : MapperBase<FormData, FormDataDto>
{
    public override partial FormDataDto Map(FormData source);

    public override partial void Map(FormData source, FormDataDto destination);
}
