using LINGYUN.Abp.ElsaNext.ApprovalFlow.FormDefinitions;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.FormDefinitions.Dtos;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Mappers;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class FormDefinitionMapper : MapperBase<FormDefinition, FormDefinitionDto>
{
    public override partial FormDefinitionDto Map(FormDefinition source);

    public override partial void Map(FormDefinition source, FormDefinitionDto destination);
}
